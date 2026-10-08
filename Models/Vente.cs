using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Pharmacie2.Models
{
    public class Vente
    {
        [Key]
        public int IdVente { get; set; }

        public string numeroVente { get; set; }
        public DateTime DateVente { get; set; } = DateTime.Now;

        // ── Client ─────────────────────────────────────────────────────
        public string NomClient { get; set; }
        public string PrenomClient { get; set; }
        public string TelephoneClient { get; set; }
        public string MotifAchat { get; set; }
        public string MatriculeEmploye { get; set; }

        // ── Montants ───────────────────────────────────────────────────
        public decimal MontantTotal { get; set; }
        public decimal MontantEspeces { get; set; }  // espèces reçues du patient
        public decimal MontantRendu { get; set; }  // rendu au patient

        // ── Paiement ───────────────────────────────────────────────────
        public string MoyenPaiement { get; set; }
        public string Type { get; set; }

        // ── Statut ─────────────────────────────────────────────────────
        public string Statut { get; set; } = "Active";
        public DateTime? DateAnnulation { get; set; }
        public string? MotifAnnulation { get; set; }

        // ── Mutuelle ───────────────────────────────────────────────────
        public int? MutuelId { get; set; }
        [ForeignKey("MutuelId")]
        public Mutuel? Mutuel { get; set; }
        public decimal TauxMutuelle { get; set; }

        /// <summary>
        /// Part prise en charge par l'entreprise (crédit entreprise).
        /// Cette part est DUE par la mutuelle mais pas encore encaissée.
        /// Elle est soldée via MutuelPaiement quand l'entreprise paie.
        /// </summary>
        public decimal MontantMutuelle { get; set; }

        /// <summary>
        /// Indique si la part mutuelle a été réglée par l'entreprise.
        /// False = l'entreprise n'a pas encore payé sa part.
        /// True  = l'entreprise a payé (via bouton "Régler" dans Uc_Mutuelle).
        /// </summary>
        public bool MutuelleReglee { get; set; } = false;

        // ── Utilisateur ─────────────────────────────────────────────────
        public int? UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }

        // ── Navigation ──────────────────────────────────────────────────
        public List<Paiement> Paiements { get; set; } = new List<Paiement>();
        public List<LigneVente> Lignes { get; set; } = new List<LigneVente>();

        // ── Calculs ─────────────────────────────────────────────────────

        /// <summary>Total versé en espèces/CB par le patient.</summary>
        [NotMapped]
        public decimal MontantVerse => Paiements?.Sum(p => p.Montant) ?? 0m;

        /// <summary>
        /// Reste à payer TOTAL = part patient non encore versée
        ///                     + part mutuelle si l'entreprise n'a pas encore payé.
        ///
        /// Pour une vente Mutuelle :
        ///   - Si MutuelleReglee = false → RestantPatient + MontantMutuelle
        ///   - Si MutuelleReglee = true  → RestantPatient seulement
        ///
        /// Pour une vente normale ou crédit :
        ///   - MontantTotal - MontantVerse
        /// </summary>
        [NotMapped]
        public decimal MontantRestant
        {
            get
            {
                decimal partPatientRestante = MontantTotal - MontantMutuelle - MontantVerse;
                if (partPatientRestante < 0) partPatientRestante = 0;

                // Si mutuelle non réglée, ajouter la part entreprise dans le restant
                decimal partMutuelleRestante = (MontantMutuelle > 0 && !MutuelleReglee)
                    ? MontantMutuelle
                    : 0;

                return partPatientRestante + partMutuelleRestante;
            }
        }

        /// <summary>Part patient restante uniquement (hors mutuelle).</summary>
        [NotMapped]
        public decimal RestantPatient
        {
            get
            {
                decimal r = MontantTotal - MontantMutuelle - MontantVerse;
                return r < 0 ? 0 : r;
            }
        }

        /// <summary>True si tout est réglé (patient + mutuelle).</summary>
        [NotMapped]
        public bool EstSoldee => MontantRestant <= 0;
    }
}