using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;

namespace Pharmacie2.Services
{
    public record KpiMaJournee(
        decimal EncaisseAujourdhui, decimal EncaisseHier,
        int VentesAujourdhui, int VentesHier,
        decimal ArgentARecuperer, int ClientsDebiteurs, int MutuellesDebitrices,
        decimal BeneficeMois, decimal BeneficeMoisPrecedent);

    public enum TypeAFaire
    {
        Perimes,
        Ruptures,
        PeremptionProche,
        StocksAVerifier,
        MutuellesEnRetard
    }

    public record AFaire(TypeAFaire Type, string Niveau, string Texte, int Nombre);

    /// <summary>
    /// Chiffres de l'écran « Ma journée » et des statistiques. Les produits archivés sont exclus partout.
    /// Un produit n'est « périmé » ou « bientôt périmé » que s'il en reste en stock.
    /// </summary>
    public static class TableauDeBordService
    {
        public const int JoursAvantPeremption = 30;
        public const int JoursRetardMutuelle = 30;

        // ───────────────────────── Chiffres clés ─────────────────────────

        public static KpiMaJournee Kpis(DateTime maintenant)
        {
            DateTime debutJour = maintenant.Date;
            DateTime debutHier = debutJour.AddDays(-1);
            DateTime memeHeureHier = debutHier + (maintenant - debutJour);

            using var ctx = new AppDbContext();

            decimal encaisseAuj = Encaisse(ctx, debutJour, maintenant);
            decimal encaisseHier = Encaisse(ctx, debutHier, memeHeureHier);
            int ventesAuj = CompterVentes(ctx, debutJour, maintenant);
            int ventesHier = CompterVentes(ctx, debutHier, memeHeureHier);

            var (recup, clients, mutuelles) = ArgentARecuperer(ctx);

            DateTime debutMois = new DateTime(maintenant.Year, maintenant.Month, 1);
            DateTime debutMoisPrec = debutMois.AddMonths(-1);
            DateTime finMoisPrec = debutMoisPrec.AddMonths(1).AddTicks(-1);
            DateTime memeJourPrec = debutMoisPrec.AddDays(maintenant.Day - 1) + maintenant.TimeOfDay;
            if (memeJourPrec > finMoisPrec) memeJourPrec = finMoisPrec;

            decimal beneficeMois = Benefice(ctx, debutMois, maintenant);
            decimal beneficePrec = Benefice(ctx, debutMoisPrec, memeJourPrec);

            return new KpiMaJournee(encaisseAuj, encaisseHier, ventesAuj, ventesHier,
                recup, clients, mutuelles, beneficeMois, beneficePrec);
        }

        /// <summary>Argent réellement reçu : paiements des ventes non annulées + règlements des mutuelles.</summary>
        public static decimal Encaisse(AppDbContext ctx, DateTime debut, DateTime fin)
        {
            var paiements = ctx.paiement.AsNoTracking()
                .Where(p => p.DatePaiement >= debut && p.DatePaiement <= fin && p.Vente.Statut != "Annulée")
                .Select(p => p.Montant)
                .ToList();   // ToList avant Sum : SQLite ne somme pas les decimal côté base

            var reglements = ctx.MutuelPaiements.AsNoTracking()
                .Where(m => m.DatePaiement >= debut && m.DatePaiement <= fin && m.Vente.Statut != "Annulée")
                .Select(m => m.Montant)
                .ToList();

            return paiements.Sum() + reglements.Sum();
        }

        public static int CompterVentes(AppDbContext ctx, DateTime debut, DateTime fin)
            => ctx.ventes.AsNoTracking().Count(v => v.Statut == "Active" && v.DateVente >= debut && v.DateVente <= fin);

        /// <summary>Crédits des patients + parts de mutuelles non réglées (ventes actives).</summary>
        public static (decimal total, int clients, int mutuelles) ArgentARecuperer(AppDbContext ctx)
        {
            var ventes = ctx.ventes.AsNoTracking()
                .Include(v => v.Paiements)
                .Where(v => v.Statut == "Active")
                .ToList();

            var avecReste = ventes.Where(v => v.MontantRestant > 0).ToList();
            decimal total = avecReste.Sum(v => v.MontantRestant);

            int clients = ventes.Where(v => v.RestantPatient > 0)
                .Select(v => string.Join("|", (v.NomClient ?? "").Trim().ToLowerInvariant(),
                                              (v.PrenomClient ?? "").Trim().ToLowerInvariant(),
                                              (v.TelephoneClient ?? "").Trim()) is { Length: > 2 } cle ? cle : "vente" + v.IdVente)
                .Distinct().Count();

            int mutuelles = ventes.Where(v => v.MutuelId != null && v.MontantMutuelle > 0 && !v.MutuelleReglee)
                .Select(v => v.MutuelId).Distinct().Count();

            return (total, clients, mutuelles);
        }

        /// <summary>
        /// Chiffre d'affaires réel (prix pratiqués à la vente, boîte ou unité) moins le coût d'achat des unités vendues.
        /// </summary>
        public static decimal MargeBrute(AppDbContext ctx, DateTime debut, DateTime fin)
        {
            var lignes = ctx.LigneVentes.AsNoTracking()
                .Include(l => l.Produit)
                .Where(l => l.Vente.Statut == "Active" && l.Vente.DateVente >= debut && l.Vente.DateVente <= fin)
                .ToList();

            decimal ca = lignes.Sum(l => l.Quantite * l.PrixUnitaire);
            decimal cout = lignes.Sum(l => CoutAchat(l));
            return ca - cout;
        }

        /// <summary>Coût d'achat des unités vendues : la boîte est achetée PrixAchat, une unité en coûte PrixAchat / NbUniteParBoite.</summary>
        public static decimal CoutAchat(LigneVente l)
        {
            if (l.Produit == null) return 0m;
            int unites = l.QuantiteUnites > 0 ? l.QuantiteUnites : l.Quantite;
            return unites * l.Produit.PrixAchat / Math.Max(1, l.Produit.NbUniteParBoite);
        }

        /// <summary>Bénéfice net = marge brute − dépenses annexes de la période.</summary>
        public static decimal Benefice(AppDbContext ctx, DateTime debut, DateTime fin)
        {
            decimal depenses = ctx.DepensesAnnexes.AsNoTracking()
                .Where(d => d.DateDepense >= debut && d.DateDepense <= fin)
                .Select(d => d.Montant).ToList().Sum();
            return MargeBrute(ctx, debut, fin) - depenses;
        }

        // ───────────────────────── Graphique ─────────────────────────

        /// <summary>Encaissements des 7 derniers jours (aujourd'hui en dernier).</summary>
        public static List<(string etiquette, decimal montant)> Encaissements7Jours(DateTime maintenant)
        {
            using var ctx = new AppDbContext();
            var res = new List<(string, decimal)>();
            for (int i = 6; i >= 0; i--)
            {
                DateTime jour = maintenant.Date.AddDays(-i);
                DateTime fin = i == 0 ? maintenant : jour.AddDays(1).AddTicks(-1);
                res.Add((Etiquette(jour), Encaisse(ctx, jour, fin)));
            }
            return res;
        }

        private static string Etiquette(DateTime jour)
        {
            string[] noms = { "dim", "lun", "mar", "mer", "jeu", "ven", "sam" };
            return noms[(int)jour.DayOfWeek] + " " + jour.ToString("dd");
        }

        // ───────────────────────── À faire maintenant ─────────────────────────

        public static List<AFaire> AFaireMaintenant(DateTime maintenant)
        {
            DateTime aujourdhui = maintenant.Date;
            DateTime limite = aujourdhui.AddDays(JoursAvantPeremption);
            var liste = new List<AFaire>();

            using var ctx = new AppDbContext();
            var actifs = ctx.produits.AsNoTracking().Where(p => p.Actif).ToList();

            var perimes = actifs.Where(p => p.QuantiteEnStock > 0 && p.DateExpiration.Date < aujourdhui).OrderBy(p => p.DateExpiration).ToList();
            if (perimes.Count > 0)
                liste.Add(new AFaire(TypeAFaire.Perimes, "urgent", Phrase(perimes.Count, "produit périmé", "produits périmés", perimes.Select(p => p.Nom)), perimes.Count));

            var ruptures = actifs.Where(p => p.QuantiteEnStock <= 0).OrderBy(p => p.Nom).ToList();
            if (ruptures.Count > 0)
                liste.Add(new AFaire(TypeAFaire.Ruptures, "urgent", Phrase(ruptures.Count, "produit en rupture", "produits en rupture", ruptures.Select(p => p.Nom)), ruptures.Count));

            var proches = actifs.Where(p => p.QuantiteEnStock > 0 && p.DateExpiration.Date >= aujourdhui && p.DateExpiration.Date <= limite).OrderBy(p => p.DateExpiration).ToList();
            if (proches.Count > 0)
                liste.Add(new AFaire(TypeAFaire.PeremptionProche, "attention", Phrase(proches.Count, $"produit bientôt périmé (moins de {JoursAvantPeremption} jours)", $"produits bientôt périmés (moins de {JoursAvantPeremption} jours)", proches.Select(p => p.Nom)), proches.Count));

            var averifier = actifs.Where(p => p.StockAVerifier).OrderBy(p => p.Nom).ToList();
            if (averifier.Count > 0)
                liste.Add(new AFaire(TypeAFaire.StocksAVerifier, "attention", Phrase(averifier.Count, "stock à vérifier", "stocks à vérifier", averifier.Select(p => p.Nom)), averifier.Count));

            var retards = MutuellesEnRetard(ctx, maintenant);
            if (retards.Count > 0)
            {
                decimal somme = retards.Sum(r => r.montant);
                liste.Add(new AFaire(TypeAFaire.MutuellesEnRetard, "attention",
                    (retards.Count > 1 ? $"{retards.Count} mutuelles n'ont pas réglé leur part" : "1 mutuelle n'a pas réglé sa part") + $" depuis plus de {JoursRetardMutuelle} jours : {Format.Montant(somme)}", retards.Count));
            }

            return liste;
        }

        /// <summary>Mutuelles dont des parts de ventes actives sont impayées depuis plus de 30 jours.</summary>
        public static List<(int mutuelleId, decimal montant)> MutuellesEnRetard(AppDbContext ctx, DateTime maintenant)
        {
            DateTime seuil = maintenant.AddDays(-JoursRetardMutuelle);
            return ctx.ventes.AsNoTracking()
                .Where(v => v.Statut == "Active" && v.MutuelId != null && v.MontantMutuelle > 0 && !v.MutuelleReglee && v.DateVente < seuil)
                .Select(v => new { Id = v.MutuelId!.Value, v.MontantMutuelle })
                .ToList()
                .GroupBy(x => x.Id)
                .Select(g => (g.Key, g.Sum(x => x.MontantMutuelle)))
                .ToList();
        }

        private static string Phrase(int n, string singulier, string pluriel, IEnumerable<string> noms)
        {
            var liste = noms.Take(3).ToList();
            string suite = n > 3 ? $" et {Format.Compte(n - 3, "autre")}" : "";
            return Format.Insecable($"{n} {(n > 1 ? pluriel : singulier)} : {string.Join(", ", liste.Select(Format.Insecable))}{suite}");
        }
    }
}
