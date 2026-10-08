using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacie2.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }


        [Required, MaxLength(50)]

        public string Nom { get; set; }
        [Required, MaxLength(50)]
        public string Prenom { get; set; }
        [Required]
        public string Role { get; set; }
        [Required, MaxLength(50)]
        public string Login { get; set; }
        [Required, MaxLength(250)]
        public string MotDePasse { get; set; }

        // ── Clé de secours (administrateurs) : seul son hash BCrypt est conservé ──
        public string? CleSecoursHash { get; set; }
        public int EssaisCleEchoues { get; set; }
        public DateTime? BlocageCleJusqua { get; set; }

        /// <summary>false = archivé : n'apparaît plus dans les choix, mais reste dans l'historique.</summary>
        public bool Actif { get; set; } = true;
    }
}
