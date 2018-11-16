using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWorkforce.Models.ViewModels
{
    public class EmployeeDetailViewModel
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DepartmentName { get; set; }

        public string ComputerMake { get; set; }

        public string ComputerManufacturer { get; set; }
       
        public List<TrainingProgram> TrainingPrograms { get; set; } = new List<TrainingProgram>();
    }
}
