using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BangazonWorkforce.Models
{
    public class TrainingProgram
    {
      
       public int Id { get; set; }

       [Required(ErrorMessage = "You must provide a name for this Name.")]
       public string Name { get; set; }

       [Required(ErrorMessage = "You must provide a Start Date for this Training Program.")]
       public DateTime StartDate { get; set; }

       [Required(ErrorMessage = "You must provide a End Date for this department.")]
       public DateTime EndDate { get; set; }

       [Required(ErrorMessage = "You must provide the Max Attendees for this Training Program.")]
       public int MaxAttendees { get; set; }
    }
}
