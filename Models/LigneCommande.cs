using System.ComponentModel.DataAnnotations;

namespace Pharmacie2.Models
{
    public class LigneCommande
    {
        [Key]
        public int Id { get; set; }

        public int CommandeId { get; set; }
        public Commande Commande { get; set; }

        public int ProduitId { get; set; }
        public Produit Produit { get; set; }

        /// <summary>Quantité commandée, en BOÎTES.</summary>
        public int Quantite { get; set; }

        /// <summary>Quantité déjà réceptionnée, en BOÎTES (≤ Quantite).</summary>
        public int QuantiteRecue { get; set; }

        // IMPORTANT : prix d'achat au moment de la commande
        public decimal PrixAchatUnitaire { get; set; }

        // Total calculé
        public decimal TotalLigne => Quantite * PrixAchatUnitaire;
    }
}