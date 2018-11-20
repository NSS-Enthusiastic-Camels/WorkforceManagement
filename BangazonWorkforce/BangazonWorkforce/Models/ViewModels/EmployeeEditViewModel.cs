using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWorkforce.Models.ViewModels
{
    public class EmployeeEditViewModel
    {// this gets the information from the sql database
        public Employee Employee { get; set; }
        public List<Department> AllDepartments { get; set; }
        public List<Computer> AllComputers { get; set; }

        public List<SelectListItem> AllComputerOptions
        {
            get
            {
                if (AllComputers == null)
                {
                    return null;
                }

                return AllComputers
                        .Select((c) => new SelectListItem(c.Make, c.Id.ToString()))
                        .ToList();
            }
        }

        public List<SelectListItem> AllDepartmentOptions
        {
            get
            {
                if (AllDepartments == null)
                {
                    return null;
                }

                return AllDepartments
                        .Select((d) => new SelectListItem(d.Name, d.Id.ToString()))
                        .ToList();
            }
        }
    }
}

