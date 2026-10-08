using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Pharmacie2.Models
{
    public class Fournisseur
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nom { get; set; }

        public string Contact { get; set; }

        /// <summary>false = archivé : n'apparaît plus dans les choix, mais reste dans l'historique.</summary>
        public bool Actif { get; set; } = true;

        // Un fournisseur peut fournir plusieurs produits
        public List<Produit> Produits { get; set; } = new List<Produit>();

        // Un fournisseur peut avoir plusieurs commandes
        public List<Commande> Commandes { get; set; } = new List<Commande>();
    }
}