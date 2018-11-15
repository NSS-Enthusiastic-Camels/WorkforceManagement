﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BangazonWorkforce.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You must provide a first name for this employee")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "You must provide a last name for this employee")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        //[Display(Name = "Full Name")]
        //public string FullName
        //{
        //    get
        //    {
        //        return $"{FirstName} {LastName}";
        //    }
        //}

        [Display(Name = "Is Supervisor?")]
        public bool IsSupervisor { get; set; }

        [Required(ErrorMessage = "Please select which department this employee is assigned to")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        public Department Department { get; set; }


        //this field exists to hold a field for the compueter assigned to this.Employee
        public Computer Computer { get; set; }
        public IEnumerable<TrainingProgram> TrainingProgram { get; set;}
    }
}
