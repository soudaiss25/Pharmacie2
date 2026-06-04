using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Pharmacie2.Models
{
    public class Commande
    {
        [Key]
        public int Id { get; set; }

        public DateTime DateCommande { get; set; } = DateTime.Now;

        /// <summary>
        /// Statuts possibles :
        /// "En attente" | "Reçu partiellement" | "Reçu" | "Annulée"
        /// </summary>
        public string Statut { get; set; } = "En attente";

        // ── Fournisseur ─────────────────────────────────────────────────
        public int FournisseurId { get; set; }

        // ✅ Nullable pour éviter le CS8618
        [ForeignKey("FournisseurId")]
        public Fournisseur? Fournisseur { get; set; }

        // ── Informations optionnelles ────────────────────────────────────
        /// <summary>Note ou commentaire libre sur la commande.</summary>
        public string NoteCommande { get; set; } = "";

        /// <summary>Date de livraison estimée (optionnelle).</summary>
        public DateTime? DateLivraisonPrevue { get; set; }

        /// <summary>Date à laquelle la commande a été réellement reçue.</summary>
        public DateTime? DateReception { get; set; }

        // ── Lignes de commande ───────────────────────────────────────────
        public List<LigneCommande> Lignes { get; set; } = new List<LigneCommande>();

        /// <summary>Montant total calculé dynamiquement.</summary>
        [NotMapped]
        public decimal MontantTotal =>
            Lignes?.Sum(l => l.TotalLigne) ?? 0m;
    }
}