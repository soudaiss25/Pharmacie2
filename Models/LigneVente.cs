using System.ComponentModel.DataAnnotations;

namespace Pharmacie2.Models
{
    public class LigneVente
    {
        [Key]
        public int Id { get; set; }

        public int VenteId { get; set; }
        public Vente Vente { get; set; }

        public int ProduitId { get; set; }
        public Produit Produit { get; set; }

        public int Quantite { get; set; }

        /// <summary>Équivalent en unités de base, figé à la vente (sert à restituer le stock).</summary>
        public int QuantiteUnites { get; set; }
        public decimal PrixUnitaire { get; set; }

        /// <summary>
        /// Coût d'achat d'UNE unité de base au moment de la vente (prix d'achat de la boîte ÷ nombre d'unités). Figé : un changement
        /// ultérieur du prix d'achat ne modifie jamais la marge d'une vente passée. Null = coût inconnu (ligne exclue de la marge).
        /// </summary>
        public decimal? CoutAchatUnitaire { get; set; }

        // Unité vendue (Boîte, Plaquette, Comprimé...)
        // Sauvegardée au moment de la vente pour l'historique
        public string UniteVendue { get; set; } = "Boîte";

        public decimal SousTotal => Quantite * PrixUnitaire;
    }
}