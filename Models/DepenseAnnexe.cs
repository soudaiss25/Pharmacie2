using System;
using System.ComponentModel.DataAnnotations;

namespace Pharmacie2.Models
{
    /// <summary>
    /// Dépense annexe de la pharmacie : salaires, factures, loyer, fournitures...
    /// Utilisée dans les Statistiques pour calculer le bénéfice net réel.
    /// </summary>
    public class DepenseAnnexe
    {
        [Key]
        public int Id { get; set; }

        /// <summary>Catégorie : "Salaire", "Facture", "Loyer", "Autre"</summary>
        [Required]
        public string Categorie { get; set; }

        /// <summary>Description libre : "Salaire Thomas - Mai 2026"</summary>
        public string Description { get; set; } = "";

        /// <summary>Montant en KMF</summary>
        public decimal Montant { get; set; }

        /// <summary>Date de la dépense</summary>
        public DateTime DateDepense { get; set; } = DateTime.Now;

        /// <summary>Mois/année pour regroupement facile</summary>
        public int Mois => DateDepense.Month;
        public int Annee => DateDepense.Year;

        /// <summary>Qui a saisi la dépense</summary>
        public int? UserId { get; set; }
        public User User { get; set; }
    }
}