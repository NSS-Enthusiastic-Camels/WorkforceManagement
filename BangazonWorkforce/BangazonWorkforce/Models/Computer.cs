using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BangazonWorkforce.Models
{
    public class Computer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You must provide a Manufacturer for this computer.")]
        public string Manufacturer { get; set; }

        [Required(ErrorMessage = "You must provide a Make for this computer.")]
        public string Make { get; set; }

        [Required(ErrorMessage = "You must provide a purchase date for this computer.")]
        public DateTime PurchaseDate { get; set; }

        public DateTime DecomissionDate { get; set; }

    }
}
