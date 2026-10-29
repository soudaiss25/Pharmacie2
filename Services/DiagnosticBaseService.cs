using System.Globalization;
using System.Text;
using System.Text.Json;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using Pharmacie2.Models;

namespace Pharmacie2.Services
{
    public enum GroupeConstat { Corrige, AVerifier, Information }

    /// <summary>Un constat du diagnostic. <c>Objet</c> = numéro de vente, nom de produit… (jamais un nom de client).</summary>
    public sealed record Constat(string Code, GroupeConstat Groupe, string Objet, string Detail);

    public sealed class RapportDiagnostic
    {
        public DateTime Date { get; set; } = DateTime.Now;

        /// <summary>Vrai juste après un import (corrections automatiques appliquées) ; faux en lecture seule.</summary>
        public bool CorrectionsAppliquees { get; set; }

        public List<Constat> Constats { get; set; } = new();

        /// <summary>Sessions de caisse restées ouvertes (D22), clôturables depuis le rapport.</summary>
        public List<int> SessionsOuvertes { get; set; } = new();

        public List<Constat> De(GroupeConstat g) => Constats.Where(c => c.Groupe == g).ToList();
        public int Nombre(GroupeConstat g) => Constats.Count(c => c.Groupe == g);
    }

    /// <summary>
    /// Diagnostic d'une base issue de l'ancienne version (règles D1 à D23). Principe : correction automatique UNIQUEMENT si le résultat est
    /// certain (D1, D4, D11, D16, D17, et seulement juste après un import) ; sinon le cas est signalé dans le rapport, sans modification.
    /// En lecture seule (bouton Administration), aucune correction n'est appliquée : une anomalie trouvée plus tard signale un nouveau bug.
    /// </summary>
    public static class DiagnosticBaseService
    {
        public const string CommentaireCloture = "Clôturée lors de l'import";

        private static string M(decimal m) => Format.Montant(m);

        private static decimal Mediane(IEnumerable<decimal> valeurs)
        {
            var l = valeurs.OrderBy(x => x).ToList();
            if (l.Count == 0) return 0m;
            return l.Count % 2 == 1 ? l[l.Count / 2] : (l[l.Count / 2 - 1] + l[l.Count / 2]) / 2m;
        }

        /// <summary>Nom normalisé : casse, accents, espaces, ponctuation (donc aussi « 500mg » = « 500 mg »).</summary>
        public static string NomNormalise(string? nom)
        {
            var sb = new StringBuilder();
            foreach (char c in (nom ?? "").Normalize(NormalizationForm.FormD))
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.NonSpacingMark) continue;
                if (char.IsLetterOrDigit(c)) sb.Append(char.ToLowerInvariant(c));
            }
            return sb.ToString();
        }

        private static readonly string[] TypesPayesEnTotalite =
            { ModesPaiement.Comptant, ModesPaiement.Cheque, ModesPaiement.CarteBancaire, ModesPaiement.Mvola, ModesPaiement.HuriMoney };

        public static RapportDiagnostic Executer(AppDbContext ctx, bool appliquer, DateTime? maintenant = null)
        {
            var now = maintenant ?? DateTime.Now;
            var r = new RapportDiagnostic { Date = now, CorrectionsAppliquees = appliquer };
            using var tx = appliquer ? ctx.Database.BeginTransaction() : null;

            void Ajouter(string code, GroupeConstat g, string objet, string detail) => r.Constats.Add(new Constat(code, g, objet, detail));
            // Correction automatique (après un import) ou simple signalement (lecture seule)
            void Corriger(string code, string objet, string avant, string apres, string phrase)
            {
                if (appliquer)
                {
                    Journal.Info($"Diagnostic {code} : {objet} : {avant} -> {apres}");
                    Ajouter(code, GroupeConstat.Corrige, objet, $"{phrase} ({avant} → {apres})");
                }
                else
                    Ajouter(code, GroupeConstat.AVerifier, objet, $"{phrase} — non corrigé (lecture seule) : {avant}");
            }

            // Marquage de la fiche produit : seulement à l'import (jamais en lecture seule), journalisé
            void Marquer(Produit p, string code, string motif)
            {
                if (FicheAVerifierService.Marquer(p, motif))
                    Journal.Info($"Diagnostic {code} : {p.Nom} : fiche marquée « À vérifier » ({motif})");
            }

            // ═══════════════ STOCK ═══════════════
            var produits = ctx.produits.ToList();
            // Produits déjà marqués « à vérifier » par l'ancienne réception ; ceux marqués par ce diagnostic (D1, D4) ont leur propre motif
            static bool MotifDiagnostic(string? m) => m != null && (m.StartsWith("stock négatif") || m.StartsWith("conditionnement"));
            var dejaMarques = produits.Where(p => p.StockAVerifier && !MotifDiagnostic(p.MotifVerification)).Select(p => p.Id).ToHashSet();

            foreach (var p in produits.Where(p => p.QuantiteEnStock < 0))                                          // D1
            {
                int x = -p.QuantiteEnStock;
                string motif = $"stock négatif (−{x}) dans l'ancienne base";
                Corriger("D1", p.Nom, p.QuantiteEnStock.ToString(), "0 et « À vérifier »", motif);
                if (appliquer)
                {
                    p.QuantiteEnStock = 0;
                    p.StockAVerifier = true;
                    p.UnitesManquantesEstimees = 0;
                    p.MotifVerification = motif;
                    dejaMarques.Remove(p.Id);
                }
            }

            foreach (var p in produits.Where(p => p.NbUniteParBoite < 1))                                          // D4
            {
                string motif = "conditionnement : nombre d'unités par boîte nul ou invalide";
                Corriger("D4", p.Nom, p.NbUniteParBoite.ToString(), "1 et « À vérifier »", motif);
                if (appliquer)
                {
                    p.NbUniteParBoite = 1;
                    p.StockAVerifier = true;
                    p.MotifVerification = string.IsNullOrEmpty(p.MotifVerification) ? motif : p.MotifVerification + " ; " + motif;
                    dejaMarques.Remove(p.Id);
                }
            }

            foreach (var p in produits.Where(p => dejaMarques.Contains(p.Id)))                                      // D2
                Ajouter("D2", GroupeConstat.Information, p.Nom, "stock peut-être faussé par l'ancienne réception des commandes : " + (p.MotifVerification ?? ""));

            var annulees = ctx.commandes.AsNoTracking().Include(c => c.Lignes).ThenInclude(l => l.Produit).Where(c => c.Statut == "Annulée").ToList();
            foreach (var c in annulees)                                                                             // D3
                foreach (var l in c.Lignes)
                    Ajouter("D3", GroupeConstat.Information, l.Produit?.Nom ?? "(produit supprimé)", $"commande n°{c.Id} annulée, peut-être après réception");

            // ═══════════════ CONDITIONNEMENT ET VENTES À L'UNITÉ ═══════════════
            var lignesUnite = ctx.LigneVentes.AsNoTracking().Where(l => l.Vente.Statut == "Active" && l.UniteVendue != StockService.UniteBoite)
                .Select(l => new { l.ProduitId, l.PrixUnitaire }).ToList();
            var ventesUniteParProduit = lignesUnite.GroupBy(l => l.ProduitId).ToDictionary(g => g.Key, g => g.ToList());

            foreach (var p in produits.Where(p => p.NbUniteParBoite > 1 && p.UniteVente == StockService.UniteBoite))        // D25
            {
                string motif = "l'unité de détail s'appelle « Boîte »";
                Ajouter("D25", GroupeConstat.AVerifier, p.Nom, $"conditionnement : {motif} ({p.NbUniteParBoite} par boîte)" + (appliquer ? " — fiche marquée « À vérifier »" : ""));
                if (appliquer) Marquer(p, "D25", FicheAVerifierService.PrefixeConditionnement + " : " + motif);
            }

            foreach (var p in produits.Where(p => p.NbUniteParBoite == 1 && ventesUniteParProduit.ContainsKey(p.Id)))        // D27
            {
                string motif = "conditionnement modifié après des ventes à l'unité";
                Corriger("D27", p.Nom, "stock affiché", "stock « À vérifier »", $"{ventesUniteParProduit[p.Id].Count} vente(s) passée(s) à l'unité mais le produit est maintenant vendu à la boîte seulement");
                if (appliquer)
                {
                    p.StockAVerifier = true;
                    p.MotifVerification = string.IsNullOrEmpty(p.MotifVerification) ? motif : p.MotifVerification + " ; " + motif;
                    dejaMarques.Remove(p.Id);
                }
            }

            foreach (var p in produits.Where(p => p.UniteVente == "Plaquette" && p.NbUniteParBoite >= 20))                   // D28
                Ajouter("D28", GroupeConstat.Information, p.Nom, $"unité probablement mal nommée : « Plaquette » pour {p.NbUniteParBoite} unités par boîte (comprimé ?)");

            foreach (var p in produits)                                                                                       // D29
            {
                if (!ventesUniteParProduit.TryGetValue(p.Id, out var l)) continue;
                var avecCentimes = l.Where(x => x.PrixUnitaire != Math.Truncate(x.PrixUnitaire)).ToList();
                if (avecCentimes.Count == 0) continue;
                Ajouter("D29", GroupeConstat.Information, p.Nom,
                    $"{avecCentimes.Count} vente(s) passée(s) à l'unité avec un prix à centimes (ex. {avecCentimes[0].PrixUnitaire:0.##} KMF) : prix calculé par l'ancienne version, à faire vérifier ; prix à l'unité actuel : {(p.PrixVenteUnite.HasValue ? M(p.PrixVenteUnite.Value) : "non renseigné")}");
            }

            // ═══════════════ PRIX ═══════════════
            foreach (var p in produits)                                                                             // D5
            {
                var motifs = new List<string>();
                if (p.PrixVente <= p.PrixAchat) motifs.Add($"prix de vente ({M(p.PrixVente)}) inférieur ou égal au prix d'achat ({M(p.PrixAchat)})");
                if (p.NbUniteParBoite > 1)
                {
                    decimal coutUnite = Math.Round(p.PrixAchat / p.NbUniteParBoite, 2);
                    decimal unite = p.PrixVenteUnite ?? 0m;
                    if (unite <= coutUnite) motifs.Add($"prix à l'unité ({M(unite)}) inférieur ou égal au coût d'achat d'une unité ({M(coutUnite)})");
                }
                if (motifs.Count > 0)
                {
                    Ajouter("D5", GroupeConstat.AVerifier, p.Nom, string.Join(" ; ", motifs) + (appliquer ? " — fiche marquée « À vérifier »" : ""));
                    if (appliquer) foreach (var m in motifs) Marquer(p, "D5", FicheAVerifierService.PrefixePrix + " : " + m);
                }
            }

            var parType = produits.GroupBy(p => p.Type ?? "").ToDictionary(
                g => g.Key,
                g => (achat: Mediane(g.Select(p => p.PrixAchat).Where(v => v > 0)), vente: Mediane(g.Select(p => p.PrixVente).Where(v => v > 0))));
            foreach (var p in produits)                                                                             // D6
            {
                var (medA, medV) = parType[p.Type ?? ""];
                bool aberrant = (p.PrixAchat > 0 && medA > 0 && p.PrixAchat >= 50 * medA)
                             || (p.PrixVente > 0 && medV > 0 && p.PrixVente >= 50 * medV)
                             || (p.PrixAchat > 0 && p.PrixVente >= 10 * p.PrixAchat);
                if (aberrant)
                {
                    string motif = $"prix peut-être multiplié par 100 à la saisie (achat {M(p.PrixAchat)}, vente {M(p.PrixVente)})";
                    Ajouter("D6", GroupeConstat.AVerifier, p.Nom, motif + (appliquer ? " — fiche marquée « À vérifier »" : ""));
                    if (appliquer) Marquer(p, "D6", FicheAVerifierService.PrefixePrix + " : " + motif);
                }
            }

            foreach (var p in produits.Where(p => p.NbUniteParBoite > 1 && p.PrixVenteUnite != null))              // D7
                Ajouter("D7", GroupeConstat.Information, p.Nom, $"prix à l'unité repris à la mise à jour : {M(p.PrixVenteUnite!.Value)} (boîte {M(p.PrixVente)}, {p.NbUniteParBoite} par boîte) : à faire vérifier par le client");

            // D24 : l'ancienne fiche initialisait la date de péremption à la date de création. Une date passée qui n'a jamais été
            // saisie se reconnaît à deux signes : le produit a été vendu APRÈS cette date, ou la date n'est pas antérieure de plus de 7 jours
            // à la première vente de la base. Ces produits ne sont pas « périmés » : leur date est simplement inconnue.
            var peremptionFausse = new HashSet<int>();
            {
                var datesVente = ctx.ventes.AsNoTracking().Where(v => v.Statut == "Active").Select(v => v.DateVente).ToList();
                DateTime? premiereVente = datesVente.Count > 0 ? datesVente.Min() : null;
                var derniereVenteParProduit = ctx.LigneVentes.AsNoTracking().Where(l => l.Vente.Statut == "Active")
                    .Select(l => new { l.ProduitId, l.Vente.DateVente }).ToList()
                    .GroupBy(x => x.ProduitId).ToDictionary(g => g.Key, g => g.Max(x => x.DateVente));
                // Deux listes : D24 = vendu après la date (date certainement fausse) ; D24b = seulement proche de la première vente et
                // jamais vendu après la date (peut être un vrai périmé : à vérifier en priorité)
                var candidats = new List<(Produit P, bool VenduApres)>();
                foreach (var p in produits.Where(p => p.PeremptionConnue && p.DateExpiration.Date < now.Date))
                {
                    bool venduApres = derniereVenteParProduit.TryGetValue(p.Id, out var derniere) && derniere > p.DateExpiration;
                    bool dateDeCreation = premiereVente.HasValue && p.DateExpiration >= premiereVente.Value.AddDays(-7);
                    if (venduApres || dateDeCreation) candidats.Add((p, venduApres));
                }
                foreach (var (p, venduApres) in candidats.OrderByDescending(c => c.VenduApres))
                {
                    peremptionFausse.Add(p.Id);
                    string code = venduApres ? "D24" : "D24b";
                    string raison = venduApres ? "vendu après cette date : date certainement fausse" : "jamais vendu après cette date : à vérifier en priorité";
                    Corriger(code, p.Nom, $"date {p.DateExpiration:dd/MM/yyyy} ({raison})", "péremption non renseignée", "date de péremption jamais saisie");
                    if (appliquer)
                    {
                        p.PeremptionConnue = false;
                        Marquer(p, code, venduApres ? FicheAVerifierService.PrefixePeremption + " : jamais saisie" : FicheAVerifierService.MotifPeremptionPrioritaire);
                    }
                }
                if (peremptionFausse.Count > 0)
                    Journal.Info($"Diagnostic D24 : {peremptionFausse.Count} produit(s) avec une date de péremption jamais saisie" + (appliquer ? " : marqués « Non renseignée »." : " (lecture seule : rien n'est modifié)."));
            }

            foreach (var p in produits)                                                                             // D8
            {
                if (!p.PeremptionConnue || peremptionFausse.Contains(p.Id)) continue;   // date inconnue : ni périmé ni invraisemblable (D24)
                if (p.DateExpiration.Year < 2000 || p.DateExpiration.Year > 2100)
                {
                    Ajouter("D8", GroupeConstat.AVerifier, p.Nom, $"date de péremption vide ou invraisemblable ({p.DateExpiration:dd/MM/yyyy})" + (appliquer ? " — fiche marquée « À vérifier »" : ""));
                    if (appliquer) Marquer(p, "D8", FicheAVerifierService.PrefixePeremption + " : date vide ou invraisemblable");
                }
                else if (p.DateExpiration.Date < now.Date && p.QuantiteEnStock > 0)
                    Ajouter("D8", GroupeConstat.Information, p.Nom, $"périmé depuis le {p.DateExpiration:dd/MM/yyyy}, encore {p.QuantiteEnStock} unité(s) en stock");
            }

            foreach (var g in produits.GroupBy(p => NomNormalise(p.Nom)).Where(g => g.Key.Length > 0 && g.Count() > 1))   // D9
                Ajouter("D9", GroupeConstat.Information, string.Join(" / ", g.Select(p => p.Nom)), "doublon probable (aucune fusion automatique)");

            // ═══════════════ VENTES ET PAIEMENTS ═══════════════
            var ventes = ctx.ventes.Include(v => v.Paiements).Include(v => v.Lignes).ToList();
            var actives = ventes.Where(v => v.Statut == "Active").ToList();

            foreach (var numero in RepriseVentesService.NumerosRepares(ctx))                                         // D10
                Ajouter("D10", GroupeConstat.Information, numero, "vente payée en totalité sans paiement complet : paiement de la différence ajouté par la mise à jour");

            foreach (var v in actives.Where(v => TypesPayesEnTotalite.Contains(v.Type ?? "")))                      // D11
            {
                decimal somme = v.Paiements.Sum(p => p.Montant);
                if (somme <= v.MontantTotal + 0.005m) continue;
                decimal diff = Math.Round(somme - v.MontantTotal, 2);
                string numero = $"P-AJUST-{v.IdVente}";
                if (ctx.paiement.Any(p => p.NumeroPaiement == numero)) continue;       // idempotent
                var dernier = v.Paiements.OrderBy(p => p.DatePaiement).ThenBy(p => p.Id).LastOrDefault();
                string mode = dernier?.Mode ?? ModesPaiement.Especes;
                Corriger("D11", v.numeroVente, $"payée {M(somme)} pour {M(v.MontantTotal)}", $"paiement de −{M(diff)} ({mode})", "vente sur-payée");
                if (appliquer)
                    ctx.paiement.Add(new Paiement { NumeroPaiement = numero, Montant = -diff, VenteId = v.IdVente, DatePaiement = v.DateVente, Mode = mode, UserId = dernier?.UserId ?? v.UserId });
            }

            foreach (var v in actives.Where(v => v.Type == ModesPaiement.Credit && v.Paiements.Sum(p => p.Montant) > v.MontantTotal + 0.005m))   // D12
                Ajouter("D12", GroupeConstat.AVerifier, v.numeroVente, $"crédit sur-payé de {M(v.Paiements.Sum(p => p.Montant) - v.MontantTotal)} (le client a peut-être été remboursé hors application)");

            foreach (var v in actives.Where(v => v.Type == ModesPaiement.Mutuelle && v.MutuelId == null))           // D13
                Ajouter("D13", GroupeConstat.AVerifier, v.numeroVente, v.MontantMutuelle == 0
                    ? "vente Mutuelle enregistrée sans mutuelle : la totalité apparaît comme dette du patient"
                    : $"part d'une mutuelle supprimée : {M(v.MontantMutuelle)}");

            // D11 / D12 étendus aux ventes Mutuelle : la part du patient (total − part mutuelle) est sur-payée ou non réglée.
            // Deux cas à lister, jamais de correction automatique (la part de l'entreprise n'est pas un paiement de la caisse).
            foreach (var v in actives.Where(v => v.Type == ModesPaiement.Mutuelle))
            {
                decimal partPatient = v.MontantTotal - v.MontantMutuelle;
                decimal payee = v.Paiements.Sum(p => p.Montant);
                if (payee > partPatient + 0.005m)                                                                     // D11 (Mutuelle)
                    Ajouter("D11", GroupeConstat.AVerifier, v.numeroVente, $"vente Mutuelle : part du patient sur-payée ({M(payee)} pour {M(partPatient)}) : aucune correction automatique");
                else if (payee < partPatient - 0.005m)                                                                // D12 (Mutuelle)
                    Ajouter("D12", GroupeConstat.AVerifier, v.numeroVente, $"vente Mutuelle : part du patient non réglée (reçu {M(payee)} sur {M(partPatient)}) : aucune correction automatique");
            }

            {                                                                                                         // D26
                var noms = ctx.mutuels.AsNoTracking().ToDictionary(m => m.IdMutuel, m => m.NomEmployeur);
                foreach (var g in actives.Where(v => v.Type == ModesPaiement.Mutuelle && MutuelleService.PatientNonIdentifie(v)).GroupBy(v => v.MutuelId).OrderBy(g => g.Key))
                {
                    string nom = g.Key != null && noms.TryGetValue(g.Key.Value, out var n) ? n : "Mutuelle supprimée";
                    decimal enAttente = g.Where(v => !v.MutuelleReglee).Sum(v => v.MontantMutuelle);
                    Ajouter("D26", GroupeConstat.Information, nom, $"{g.Count()} vente(s) Mutuelle sans nom ni matricule du patient, part de la mutuelle en attente : {M(enAttente)}");
                }
            }

            foreach (var g in ventes.GroupBy(v => v.numeroVente).Where(g => !string.IsNullOrEmpty(g.Key) && g.Count() > 1))   // D14
                Ajouter("D14", GroupeConstat.AVerifier, g.Key, $"numéro de vente utilisé {g.Count()} fois (aucune renumérotation)");

            foreach (var v in ventes.Where(v => v.DateVente > now))                                                 // D15
                Ajouter("D15", GroupeConstat.AVerifier, v.numeroVente, $"vente datée dans le futur ({v.DateVente:dd/MM/yyyy HH:mm})");

            // ═══════════════ MUTUELLES ═══════════════
            var reglements = ctx.MutuelPaiements.ToList().GroupBy(m => m.VenteId).ToDictionary(g => g.Key, g => g.ToList());
            foreach (var v in ventes.Where(v => v.Statut != "Annulée"))
            {
                reglements.TryGetValue(v.IdVente, out var mps);
                decimal sommeMP = mps?.Sum(m => m.Montant) ?? 0m;
                bool reglee = v.MutuelleReglee;

                if (v.MontantMutuelle > 0 && sommeMP >= v.MontantMutuelle && !reglee)                               // D16
                {
                    Corriger("D16", v.numeroVente, "non réglée", "réglée", $"la mutuelle a payé {M(sommeMP)} pour une part de {M(v.MontantMutuelle)} (évite une double facturation)");
                    if (appliquer) v.MutuelleReglee = true;
                }

                if (mps is { Count: > 0 })                                                                          // D17
                {
                    var payeurs = mps.Where(m => m.MutuelId != null).Select(m => m.MutuelId!.Value).Distinct().ToList();
                    if (payeurs.Count == 1 && payeurs[0] != v.MutuelId)
                    {
                        Corriger("D17", v.numeroVente, $"mutuelle n°{v.MutuelId?.ToString() ?? "aucune"}", $"mutuelle n°{payeurs[0]}", "la mutuelle qui a payé n'est pas celle de la vente");
                        if (appliquer) v.MutuelId = payeurs[0];
                    }
                    else if (payeurs.Count > 1)
                        Ajouter("D17", GroupeConstat.AVerifier, v.numeroVente, "plusieurs mutuelles ont réglé cette vente : aucune correction automatique");
                }

                if (sommeMP > 0 && v.MontantMutuelle > 0 && sommeMP < v.MontantMutuelle)                           // D18
                    Ajouter("D18", GroupeConstat.AVerifier, v.numeroVente, $"part modifiée après règlement : reste {M(v.MontantMutuelle - sommeMP)}");

                if (reglee && v.MontantMutuelle > 0 && mps is null or { Count: 0 })                                 // D19
                    Ajouter("D19", GroupeConstat.AVerifier, v.numeroVente, "marquée réglée mais aucun règlement enregistré (effacés avec une mutuelle supprimée ?)");
            }

            // ═══════════════ HISTORIQUE INCOMPLET ═══════════════
            foreach (var v in ventes.Where(v => v.Statut != "Annulée"))                                             // D20
            {
                if (v.Lignes.Count == 0)
                    Ajouter("D20", GroupeConstat.AVerifier, v.numeroVente, "aucune ligne de vente : produit supprimé depuis la vente");
                else
                {
                    decimal lignes = v.Lignes.Sum(l => l.Quantite * l.PrixUnitaire);
                    if (Math.Abs(lignes - v.MontantTotal) > 1m)
                        Ajouter("D20", GroupeConstat.AVerifier, v.numeroVente, $"somme des lignes ({M(lignes)}) différente du total ({M(v.MontantTotal)}) : produit supprimé depuis la vente");
                }
            }

            int ventesSansVendeur = ventes.Count(v => v.UserId == null);                                            // D21
            int paiementsSansVendeur = ctx.paiement.Count(p => p.UserId == null);
            if (ventesSansVendeur + paiementsSansVendeur > 0)
                Ajouter("D21", GroupeConstat.Information, "Historique", $"{ventesSansVendeur} vente(s) et {paiementsSansVendeur} paiement(s) sans vendeur (utilisateur supprimé) : affichés « Vendeur inconnu »");

            // ═══════════════ CAISSE ET DÉPENSES ═══════════════
            foreach (var s in ctx.SessionsCaisse.AsNoTracking().Where(s => s.Statut == "Ouverte").ToList().Where(s => s.DateOuverture.Date < now.Date))   // D22
            {
                Ajouter("D22", GroupeConstat.AVerifier, $"Session n°{s.Id}", $"ouverte le {s.DateOuverture:dd/MM/yyyy HH:mm} et jamais clôturée");
                r.SessionsOuvertes.Add(s.Id);
            }

            var depenses = ctx.DepensesAnnexes.AsNoTracking().HorsRemboursements().ToList();                                             // D23
            decimal medDep = Mediane(depenses.Select(d => d.Montant).Where(m => m > 0));
            if (medDep > 0)
                foreach (var d in depenses.Where(d => d.Montant >= 50 * medDep))
                    Ajouter("D23", GroupeConstat.AVerifier, $"Dépense {d.Categorie} du {d.DateDepense:dd/MM/yyyy}", $"{M(d.Montant)} : montant peut-être multiplié par 100 à la saisie");

            if (appliquer)
            {
                ctx.SaveChanges();
                tx!.Commit();
            }
            Journal.Info($"Diagnostic de la base ({(appliquer ? "avec corrections automatiques" : "lecture seule")}) : " +
                         $"{r.Nombre(GroupeConstat.Corrige)} corrigé(s), {r.Nombre(GroupeConstat.AVerifier)} à vérifier, {r.Nombre(GroupeConstat.Information)} pour information.");
            return r;
        }

        /// <summary>Clôture les sessions restées ouvertes : à la fin de leur journée (ou avant l'ouverture suivante du même caissier), attendu figé.</summary>
        public static int CloturerSessions(AppDbContext ctx, IEnumerable<int> ids, DateTime? maintenant = null)
        {
            var now = maintenant ?? DateTime.Now;
            var liste = ids.ToList();
            var sessions = ctx.SessionsCaisse.Where(s => liste.Contains(s.Id) && s.Statut == "Ouverte").ToList();
            var toutes = ctx.SessionsCaisse.AsNoTracking().ToList();
            foreach (var s in sessions)
            {
                var fin = s.DateOuverture.Date.AddDays(1).AddSeconds(-1);
                var suivante = toutes.Where(t => t.UserId == s.UserId && t.DateOuverture > s.DateOuverture).OrderBy(t => t.DateOuverture).FirstOrDefault();
                if (suivante != null && suivante.DateOuverture.AddSeconds(-1) < fin) fin = suivante.DateOuverture.AddSeconds(-1);
                if (fin > now) fin = now;
                s.DateCloture = fin;
                s.Statut = "Clôturée";
                s.CommentaireCloture = CommentaireCloture;
                s.MontantAttenduCloture = CaisseService.EspecesAttendues(ctx, s);
                Journal.Info($"Diagnostic D22 : session n°{s.Id} clôturée le {fin:dd/MM/yyyy HH:mm}, attendu figé {s.MontantAttenduCloture}");
            }
            ctx.SaveChanges();
            return sessions.Count;
        }

        // ───────── Dernier rapport (affichage, ZIP du développeur) ─────────

        private static string CheminDernier => Path.Combine(CheminsApp.DossierDonnees, "dernier_diagnostic.json");

        public static void Enregistrer(RapportDiagnostic r)
        {
            try { File.WriteAllText(CheminDernier, JsonSerializer.Serialize(r), new UTF8Encoding(false)); }
            catch (Exception ex) { Journal.Erreur("Enregistrement du dernier diagnostic", ex); }
        }

        public static RapportDiagnostic? Dernier()
        {
            try { return File.Exists(CheminDernier) ? JsonSerializer.Deserialize<RapportDiagnostic>(File.ReadAllText(CheminDernier)) : null; }
            catch { return null; }
        }

        // ───────── Export Excel : un onglet par groupe, jamais de nom de client ─────────

        public static void ExporterExcel(RapportDiagnostic r, string chemin)
        {
            using var wb = new XLWorkbook();
            foreach (var (groupe, titre) in new[]
            {
                (GroupeConstat.Corrige, "Corrigés automatiquement"), (GroupeConstat.AVerifier, "À vérifier"), (GroupeConstat.Information, "Pour information")
            })
            {
                var ws = wb.AddWorksheet(titre);
                ws.Cell(1, 1).Value = "Règle";
                ws.Cell(1, 2).Value = "Vente / produit / session";
                ws.Cell(1, 3).Value = "Détail";
                ws.Range(1, 1, 1, 3).Style.Font.Bold = true;
                int ligne = 2;
                foreach (var c in r.De(groupe))
                {
                    ws.Cell(ligne, 1).Value = c.Code;
                    ws.Cell(ligne, 2).Value = c.Objet;
                    ws.Cell(ligne, 3).Value = c.Detail;
                    ligne++;
                }
                ws.Column(1).Width = 9;
                ws.Column(2).Width = 38;
                ws.Column(3).Width = 110;
                ws.SheetView.FreezeRows(1);
            }
            wb.SaveAs(chemin);
        }
    }
}
