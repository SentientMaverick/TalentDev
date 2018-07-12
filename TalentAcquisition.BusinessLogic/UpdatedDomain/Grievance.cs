using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TalentAcquisition.BusinessLogic.UpdatedDomain
{
    public class GrievanceType
    {
        [Key]
        public string Code { get; set; }
        public string Description { get; set; }
    }
    public class GrievanceAction
    {
        [Key]
        public string Code { get; set; }
        public string Description { get; set; }
    }
    public class GrievanceReport
    {
        [Key]
        public string No { get; set; }
        [Display(Name = "Employee Number")]
        public string EmployeeNumber { get; set; }
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }
        [Display(Name = "Grievance Code")]
        public string GrievanceCode { get; set; }

        [Display(Name = "Grievance Description")]
        public string GrievanceDescription { get; set; }
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
        [Display(Name = "Date Approved")]
        public DateTime DateApproved { get; set; }
        [Display(Name = "Offender Code")]
        public string OffenderCode { get; set; }
        [Display(Name = "Offender Name")]
        public string OffenderName { get; set; }
        [Display(Name = "Supporting Document")]
        public string Document { get; set; }
        public bool IsApproved { get; set; }
        public bool IsClosed { get; set; }
    }
}