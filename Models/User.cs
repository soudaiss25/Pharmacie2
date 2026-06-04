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
    }
}
