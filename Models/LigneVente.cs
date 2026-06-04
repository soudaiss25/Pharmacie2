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
        public decimal PrixUnitaire { get; set; }

        // Unité vendue (Boîte, Plaquette, Comprimé...)
        // Sauvegardée au moment de la vente pour l'historique
        public string UniteVendue { get; set; } = "Boîte";

        public decimal SousTotal => Quantite * PrixUnitaire;
    }
}