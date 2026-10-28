using Pharmacie2.Models;

namespace Pharmacie2.Services
{
    /// <param name="CaAvecCout">Chiffre d'affaires des lignes dont le coût d'achat est connu.</param>
    /// <param name="Cout">Coût d'achat des unités vendues (au prix d'achat du jour de la vente).</param>
    /// <param name="LignesSansCout">Lignes sans coût connu : exclues de la marge et signalées.</param>
    /// <param name="CaSansCout">Chiffre d'affaires de ces lignes (retiré du calcul de la marge).</param>
    public sealed record CalculMarge(decimal CaAvecCout, decimal Cout, int LignesSansCout, decimal CaSansCout)
    {
        public decimal MargeBrute => CaAvecCout - Cout;
        public string? Signalement => MargeVenteService.Signalement(LignesSansCout);
    }

    /// <summary>
    /// La marge d'une vente utilise le prix d'achat du JOUR de la vente (LigneVente.CoutAchatUnitaire). Mêmes calculs partout :
    /// Statistiques, Ma journée, Dépenses, exports.
    /// </summary>
    public static class MargeVenteService
    {
        public static int UnitesVendues(LigneVente l) => l.QuantiteUnites > 0 ? l.QuantiteUnites : l.Quantite;

        /// <summary>Coût d'achat d'une unité de base aujourd'hui : prix d'achat de la boîte ÷ nombre d'unités par boîte.</summary>
        public static decimal CoutUnitaireDuJour(Produit p) => p.PrixAchat / Math.Max(1, p.NbUniteParBoite);

        /// <summary>Coût d'achat de la ligne = unités vendues × coût unitaire figé ; null si le coût est inconnu.</summary>
        public static decimal? Cout(LigneVente l) => l.CoutAchatUnitaire.HasValue ? UnitesVendues(l) * l.CoutAchatUnitaire.Value : null;

        public static CalculMarge Calculer(IEnumerable<LigneVente> lignes)
        {
            decimal caAvec = 0, cout = 0, caSans = 0;
            int sans = 0;
            foreach (var l in lignes)
            {
                decimal ca = l.Quantite * l.PrixUnitaire;
                if (Cout(l) is { } c) { caAvec += ca; cout += c; }
                else { caSans += ca; sans++; }
            }
            return new CalculMarge(caAvec, cout, sans, caSans);
        }

        public static string? Signalement(int lignesSansCout)
            => lignesSansCout <= 0 ? null : lignesSansCout == 1 ? "1 ligne sans coût d'achat connu" : $"{lignesSansCout} lignes sans coût d'achat connu";
    }
}
