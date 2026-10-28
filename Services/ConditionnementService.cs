using Pharmacie2.Models;

namespace Pharmacie2.Services
{
    /// <summary>Proposition faite au vendeur quand il vend à l'unité autant ou plus qu'une boîte entière. Rien n'est imposé.</summary>
    public sealed record PropositionBoites(int Boites, int Reste, string Unite, int Quantite, decimal TotalUnites, decimal TotalBoites)
    {
        public string Message(string nomProduit)
            => $"Vous vendez {Format.Compte(Quantite, Unite.ToLowerInvariant())} de « {nomProduit} » à l'unité." + Environment.NewLine + Environment.NewLine +
               $"À l'unité :  {Format.Montant(TotalUnites)}" + Environment.NewLine +
               $"En boîtes :  {Format.Montant(TotalBoites)}   ({Boites} boîte{(Boites > 1 ? "s" : "")}{(Reste > 0 ? $" + {Format.Compte(Reste, Unite.ToLowerInvariant())}" : "")})" + Environment.NewLine + Environment.NewLine +
               $"Vendre {Boites} boîte{(Boites > 1 ? "s" : "")}{(Reste > 0 ? $" + {Format.Compte(Reste, Unite.ToLowerInvariant())}" : "")} à la place ?" + Environment.NewLine +
               "« Oui » : boîtes (+ unités restantes).  « Non » : garder la vente à l'unité.";
    }

    public static class ConditionnementService
    {
        /// <summary>
        /// Si la quantité vendue à l'unité atteint ou dépasse le contenu d'une boîte, calcule la même vente en boîtes + unités restantes,
        /// avec les deux totaux. Null dans tous les autres cas (produit vendu à la boîte seulement, vente en boîtes, quantité plus petite qu'une boîte).
        /// Les prix sont ceux saisis par le client : rien n'est recalculé.
        /// </summary>
        public static PropositionBoites? Proposer(Produit p, int quantite, string unite)
        {
            if (p.NbUniteParBoite <= 1 || unite == StockService.UniteBoite || quantite < p.NbUniteParBoite) return null;
            int boites = quantite / p.NbUniteParBoite, reste = quantite % p.NbUniteParBoite;
            decimal totalUnites = quantite * p.PrixUnitaireVente;
            decimal totalBoites = boites * p.PrixVente + reste * p.PrixUnitaireVente;
            return new PropositionBoites(boites, reste, unite, quantite, totalUnites, totalBoites);
        }
    }
}
