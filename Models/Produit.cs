using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacie2.Models
{
    /// <summary>
    /// Modèle Produit.
    ///
    /// IMPORTANT — Convention de stock :
    ///   QuantiteEnStock est TOUJOURS en unités de base.
    ///
    ///   Exemple Doliprane :
    ///     UniteVente      = "Plaquette"
    ///     NbUniteParBoite = 5
    ///     QuantiteEnStock = 10  → signifie 10 plaquettes (= 2 boîtes)
    ///
    ///   Exemple Doliprane vendu à la boîte :
    ///     UniteVente      = "Boîte"
    ///     NbUniteParBoite = 1
    ///     QuantiteEnStock = 3   → signifie 3 boîtes
    ///
    ///   Lors d'une vente de 3 plaquettes → QuantiteEnStock -= 3
    ///   Lors d'une vente de 1 boîte      → QuantiteEnStock -= 5 (NbUniteParBoite)
    /// </summary>
    public class Produit
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nom { get; set; }
        public string Type { get; set; }

        public decimal PrixAchat { get; set; }
        public decimal PrixVente { get; set; }
        public decimal MargeBeneficiaire { get; set; }

        /// <summary>
        /// Stock en UNITÉS DE BASE (plaquettes, comprimés, flacons…).
        /// Si NbUniteParBoite = 1 → c'est directement en boîtes.
        /// Si NbUniteParBoite = 5 → 10 ici = 10 plaquettes = 2 boîtes.
        /// </summary>
        public int QuantiteEnStock { get; set; }

        public int SeuilAlerte { get; set; }
        public DateTime DateExpiration { get; set; }

        // ── Vente en détail ────────────────────────────────────────────
        /// <summary>Unité de vente : "Boîte", "Plaquette", "Comprimé"…</summary>
        public string UniteVente { get; set; } = "Boîte";

        /// <summary>
        /// Nombre d'unités dans une boîte.
        /// 1 = vendu uniquement à la boîte.
        /// 5 = 1 boîte contient 5 plaquettes → on peut vendre à la plaquette.
        /// </summary>
        public int NbUniteParBoite { get; set; } = 1;

        // ── Propriétés calculées (non stockées) ───────────────────────

        /// <summary>Prix d'une unité (plaquette/comprimé).</summary>
        [NotMapped]
        public decimal PrixUnitaireVente =>
            NbUniteParBoite > 1
                ? Math.Round(PrixVente / NbUniteParBoite, 2)
                : PrixVente;

        /// <summary>
        /// Nombre de boîtes complètes disponibles.
        /// Ex : 10 plaquettes / 5 par boîte = 2 boîtes.
        /// </summary>
        [NotMapped]
        public int NbBoitesEnStock =>
            NbUniteParBoite > 1
                ? QuantiteEnStock / NbUniteParBoite
                : QuantiteEnStock;

        /// <summary>
        /// Nombre d'unités restantes après les boîtes complètes.
        /// Ex : 7 plaquettes / 5 par boîte → reste 2 plaquettes.
        /// </summary>
        [NotMapped]
        public int UnitesRestantes =>
            NbUniteParBoite > 1
                ? QuantiteEnStock % NbUniteParBoite
                : 0;

        // ── Informations médicament pour le caissier ─────────────────
        /// <summary>Pour quoi est ce médicament ? Ex : "Pour la toux", "Fièvre", "Hypertension"</summary>
        public string Indication { get; set; } = "";

        /// <summary>Comment le prendre ? Ex : "1 comprimé après le repas, avec de l'eau"</summary>
        public string Posologie { get; set; } = "";

        /// <summary>Nombre de prises par jour (0 = non renseigné)</summary>
        public int NbFoisParJour { get; set; } = 0;

        // ── Fournisseur ───────────────────────────────────────────────
        public int? FournisseurId { get; set; }
        [ForeignKey("FournisseurId")]
        public Fournisseur? Fournisseur { get; set; }

        public bool EstEnRupture() => QuantiteEnStock <= Pharmacie2.Services.StockService.SeuilEnUnites(this);
    }
}