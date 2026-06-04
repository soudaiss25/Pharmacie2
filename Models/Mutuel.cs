using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacie2.Models
{
    public class Mutuel
    {
        [Key]
        public int IdMutuel { get; set; }

       
   

        public string NomEmployeur { get; set; }

     
        public string EmailContact { get; set; }
        public string telephoneEmployeur { get; set; }

        [Range(0, 100)]
        public decimal TauxPriseEnCharge { get; set; }


    }
}
