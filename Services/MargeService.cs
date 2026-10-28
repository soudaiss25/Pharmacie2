using Pharmacie2.Models;

namespace Pharmacie2.Services
{
    /// <summary>
    /// La marge n'est JAMAIS une saisie : le client tape ses deux prix, la marge s'affiche automatiquement.
    /// Le prix de vente n'est jamais recalculé à partir de la marge ni du prix d'achat.
    /// </summary>
    public static class MargeService
    {
        /// <summary>Marge en KMF = prix de vente − prix d'achat.</summary>
        public static decimal Kmf(decimal prixAchat, decimal prixVente) => prixVente - prixAchat;

        /// <summary>Marge en % du prix d'achat ; null si le prix d'achat n'est pas renseigné.</summary>
        public static decimal? Pourcent(decimal prixAchat, decimal prixVente)
            => prixAchat > 0 ? Math.Round((prixVente - prixAchat) / prixAchat * 100m, 1) : null;

        /// <summary>Valeur enregistrée dans Produit.MargeBeneficiaire (2 décimales) ; 0 si le prix d'achat est nul.</summary>
        public static decimal Enregistree(decimal prixAchat, decimal prixVente)
            => prixAchat > 0 ? Math.Round((prixVente - prixAchat) / prixAchat * 100m, 2) : 0m;

        /// <summary>Marge d'une unité de détail = prix à l'unité − (prix d'achat de la boîte ÷ nombre d'unités).</summary>
        public static decimal KmfUnite(decimal prixAchatBoite, int nbUnites, decimal prixVenteUnite)
            => prixVenteUnite - prixAchatBoite / Math.Max(1, nbUnites);

        public static string Texte(decimal prixAchat, decimal prixVente)
        {
            var pct = Pourcent(prixAchat, prixVente);
            return pct.HasValue
                ? $"{Format.Montant(Kmf(prixAchat, prixVente))} ({pct.Value:0.#} % du prix d'achat)"
                : $"{Format.Montant(Kmf(prixAchat, prixVente))} (prix d'achat non renseigné)";
        }

        public static string Texte(Produit p) => Texte(p.PrixAchat, p.PrixVente);
    }
}
