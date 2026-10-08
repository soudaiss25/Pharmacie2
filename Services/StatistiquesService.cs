using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;

namespace Pharmacie2.Services
{
    public record LigneMode(string Mode, int NbVentes, decimal Recu, decimal Pourcentage);

    public record LigneTop(string Produit, string Vendu, decimal CA);

    public record LigneCredit(string Client, string Telephone, int NbVentes, decimal Restant);

    public record LigneMutuelleDue(string Mutuelle, int NbVentes, decimal TotalDu);

    public record StatsPeriode(
        decimal CA, decimal MargeBrute, decimal Depenses, decimal BeneficeNet,
        int NbVentes, int NbVentesASolder, decimal ArgentARecuperer,
        List<LigneMode> Modes, List<LigneTop> TopProduits, List<LigneCredit> Credits, List<LigneMutuelleDue> Mutuelles);

    /// <summary>Statistiques d'une période. Les prix et quantités réellement pratiqués à la vente sont utilisés.</summary>
    public static class StatistiquesService
    {
        /// <summary>Période précédente de même durée, juste avant la période donnée.</summary>
        public static (DateTime debut, DateTime fin) PeriodePrecedente(DateTime debut, DateTime fin)
        {
            var duree = fin - debut;
            var finPrec = debut.AddTicks(-1);
            return (finPrec - duree, finPrec);
        }

        public static StatsPeriode Calculer(DateTime debut, DateTime fin)
        {
            using var ctx = new AppDbContext();

            var ventes = ctx.ventes.AsNoTracking()
                .Include(v => v.Paiements)
                .Include(v => v.Lignes).ThenInclude(l => l.Produit)
                .Where(v => v.DateVente >= debut && v.DateVente <= fin && v.Statut == "Active")
                .ToList();

            // Chiffre d'affaires réel : quantité × prix pratiqué (boîte ou unité)
            var lignes = ventes.SelectMany(v => v.Lignes).ToList();
            decimal ca = lignes.Sum(l => l.Quantite * l.PrixUnitaire);
            decimal cout = lignes.Sum(l => TableauDeBordService.CoutAchat(l));
            decimal marge = ca - cout;

            decimal depenses = ctx.DepensesAnnexes.AsNoTracking()
                .Where(d => d.DateDepense >= debut && d.DateDepense <= fin)
                .Select(d => d.Montant).ToList().Sum();

            var aSolder = ventes.Where(v => v.MontantRestant > 0).ToList();

            return new StatsPeriode(
                ca, marge, depenses, marge - depenses,
                ventes.Count, aSolder.Count, aSolder.Sum(v => v.MontantRestant),
                ArgentRecuParMode(ctx, ventes, debut, fin),
                TopProduits(lignes),
                Credits(ventes),
                MutuellesDues(ctx, ventes));
        }

        /// <summary>Argent réellement reçu pendant la période, par moyen de paiement de la vente d'origine.</summary>
        private static List<LigneMode> ArgentRecuParMode(AppDbContext ctx, List<Vente> ventesPeriode, DateTime debut, DateTime fin)
        {
            var paiements = ctx.paiement.AsNoTracking()
                .Where(p => p.DatePaiement >= debut && p.DatePaiement <= fin && p.Vente.Statut != "Annulée")
                .Select(p => new { p.Montant, p.Vente.Type })
                .ToList();

            var reglements = ctx.MutuelPaiements.AsNoTracking()
                .Where(m => m.DatePaiement >= debut && m.DatePaiement <= fin && m.Vente.Statut != "Annulée")
                .Select(m => m.Montant)
                .ToList();

            decimal Somme(Func<string?, bool> mode) => paiements.Where(p => mode(p.Type)).Sum(p => p.Montant);
            int Nb(Func<string?, bool> mode) => ventesPeriode.Count(v => mode(v.Type));

            var brut = new List<(string mode, int nb, decimal recu)>
            {
                ("Espèces", Nb(t => t == ModesPaiement.Comptant), Somme(t => t == ModesPaiement.Comptant)),
                ("Mvola / Huri Money", Nb(ModesPaiement.EstMobile), Somme(ModesPaiement.EstMobile)),
                ("Carte bancaire", Nb(t => t == ModesPaiement.CarteBancaire), Somme(t => t == ModesPaiement.CarteBancaire)),
                ("Chèque", Nb(t => t == ModesPaiement.Cheque), Somme(t => t == ModesPaiement.Cheque)),
                ("Mutuelle (patient et entreprise)", Nb(t => t == ModesPaiement.Mutuelle), Somme(t => t == ModesPaiement.Mutuelle) + reglements.Sum()),
                ("Crédit (versements des clients)", Nb(t => t == ModesPaiement.Credit), Somme(t => t == ModesPaiement.Credit)),
            };

            decimal total = brut.Sum(b => b.recu);
            return brut.Select(b => new LigneMode(b.mode, b.nb, b.recu, total > 0 ? Math.Round(b.recu / total * 100m, 1) : 0m)).ToList();
        }

        private static List<LigneTop> TopProduits(List<LigneVente> lignes)
            => lignes.Where(l => l.Produit != null)
                .GroupBy(l => l.ProduitId)
                .Select(g =>
                {
                    var p = g.First().Produit;
                    int unites = g.Sum(x => x.QuantiteUnites > 0 ? x.QuantiteUnites : x.Quantite);
                    return new LigneTop(p.Nom, StockService.Formater(p, unites), g.Sum(x => x.Quantite * x.PrixUnitaire));
                })
                .OrderByDescending(x => x.CA)
                .Take(10)
                .ToList();

        private static List<LigneCredit> Credits(List<Vente> ventes)
            => ventes.Where(v => v.RestantPatient > 0)
                .GroupBy(v => new { N = (v.NomClient ?? "").Trim(), P = (v.PrenomClient ?? "").Trim(), T = (v.TelephoneClient ?? "").Trim() })
                .Select(g => new LigneCredit(
                    $"{g.Key.P} {g.Key.N}".Trim() is { Length: > 0 } nom ? nom : "Client sans nom",
                    g.Key.T.Length > 0 ? g.Key.T : "—",
                    g.Count(),
                    g.Sum(x => x.RestantPatient)))
                .OrderByDescending(x => x.Restant)
                .ToList();

        private static List<LigneMutuelleDue> MutuellesDues(AppDbContext ctx, List<Vente> ventes)
        {
            var noms = ctx.mutuels.AsNoTracking().ToDictionary(m => m.IdMutuel, m => m.NomEmployeur);
            return ventes.Where(v => v.MutuelId != null && v.MontantMutuelle > 0 && !v.MutuelleReglee)
                .GroupBy(v => v.MutuelId!.Value)
                .Select(g => new LigneMutuelleDue(noms.TryGetValue(g.Key, out var n) ? n : "Mutuelle supprimée", g.Count(), g.Sum(v => v.MontantMutuelle)))
                .OrderByDescending(x => x.TotalDu)
                .ToList();
        }
    }
}
