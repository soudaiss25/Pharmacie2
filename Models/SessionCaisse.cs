using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacie2.Models
{
    /// <summary>
    /// Session de travail d'un caissier (ouverture → clôture).
    /// 
    /// IMPORTANT : Ce fichier REMPLACE l'ancienne classe vide SessionCaisse.cs
    /// La classe passe de "internal" à "public" pour être accessible partout.
    /// </summary>
    public class SessionCaisse
    {
        [Key]
        public int Id { get; set; }

        // ── Qui a ouvert la caisse ──────────────────────────────────────
        public int? UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        // ── Ouverture ───────────────────────────────────────────────────
        public DateTime DateOuverture { get; set; } = DateTime.Now;

        /// <summary>Fond de caisse déclaré au début de la session (billets comptés).</summary>
        public decimal FondOuverture { get; set; } = 0m;

        // ── Clôture ─────────────────────────────────────────────────────
        public DateTime? DateCloture { get; set; }

        /// <summary>Montant physiquement compté dans la caisse à la clôture.</summary>
        public decimal? MontantCompteCloture { get; set; }

        /// <summary>Commentaire libre à la clôture (ex: "5×1000 + 3×500…").</summary>
        public string CommentaireCloture { get; set; }

        // ── Statut : "Ouverte" ou "Clôturée" ───────────────────────────
        public string Statut { get; set; } = "Ouverte";
    }
}