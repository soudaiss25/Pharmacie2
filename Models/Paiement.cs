using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacie2.Models
{
    public class Paiement
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string NumeroPaiement { get; set; }

        public DateTime DatePaiement { get; set; } = DateTime.Now;

        public decimal Montant { get; set; }

        /// <summary>Mode de ce paiement : Espèces, Chèque, Carte bancaire, Mvola ou Huri Money (seules les espèces entrent dans le tiroir). Un remboursement est un montant négatif.</summary>
        [Required]
        public string Mode { get; set; } = Pharmacie2.Services.ModesPaiement.Especes;

        public int VenteId { get; set; }
        public Vente Vente { get; set; }

        /// <summary>
        /// Tiroir (session de caisse) dans lequel l'argent est allé. Null pour les paiements d'avant cette fonction (règle d'origine : caissier + dates)
        /// et pour les paiements hors tiroir sans session ouverte.
        /// </summary>
        public int? SessionCaisseId { get; set; }

        // ← NOUVEAU : qui a encaissé ce paiement
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}