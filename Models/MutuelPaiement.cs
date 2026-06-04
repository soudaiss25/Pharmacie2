using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacie2.Models
{
    /// <summary>
    /// Enregistre le règlement de la PART MUTUELLE (côté entreprise/mutuelle).
    /// Distinct de Paiement qui gère la part patient.
    /// 
    /// Flux :
    ///   Vente.MontantTotal = MontantMutuelle (part entreprise) + part patient
    ///   La part patient → table Paiement
    ///   La part entreprise → table MutuelPaiement  ← cette classe
    /// </summary>
    public class MutuelPaiement
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string NumeroPaiement { get; set; }

        public DateTime DatePaiement { get; set; } = DateTime.Now;

        /// <summary>Montant versé par la mutuelle/entreprise pour solder sa part.</summary>
        public decimal Montant { get; set; }

        /// <summary>Référence de virement ou chèque reçu de la mutuelle.</summary>
        public string Reference { get; set; }

        /// <summary>Commentaire libre (ex: "Virement du 01/03/2026").</summary>
        public string Commentaire { get; set; }

        // ── FK vers la Vente concernée ──────────────────────────────────
        public int VenteId { get; set; }

        [ForeignKey("VenteId")]
        public Vente Vente { get; set; }

        // ── FK vers la Mutuelle (pour regrouper les paiements par mutuelle)
        public int? MutuelId { get; set; }

        [ForeignKey("MutuelId")]
        public Mutuel Mutuel { get; set; }

        // ── FK vers l'utilisateur qui a saisi le règlement ─────────────
        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}