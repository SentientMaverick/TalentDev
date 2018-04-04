using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TalentAcquisition.BusinessLogic.UpdatedDomain
{
    //[Table("Employee")]
    public class JobOccupant
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Employee ID")]
        public string EmployeeNumber { get; set; }
        //public string UserId { get; set; }
        [Display(Name = "First name")]
        public string FirstName { get; private set; }
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; private set; }
        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return LastName + " " + FirstName;
            }
        }
    }
}