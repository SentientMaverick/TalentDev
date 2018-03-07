using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;
using TalentAcquisition.Core.Domain;

namespace TalentAcquisition.BusinessLogic.UpdatedDomain
{
    public class Job
    {
        [Key]
        public string JobID { get; set; }
        [Display(Name = "Job Description")]
        public string JobDescription { get; set; }
        [Display(Name = "No Of Posts")]
        [Required]
        public int? Posts { get; set; }
        [Display(Name = "Position Reporting To")]
        public int? SupervisorID { get; set; }
        [Display(Name = "Department")]
        public int DepartmentID { get; set; }
        [Display(Name = "Branch")]
        public int? BranchID { get; set; }
        [Display(Name = "Main Objective")]
        public string MainObjective { get; set; }
        public string Grade { get; set; }
        [Display(Name = "Employee Requisitions")]
        public int RequisitionCount { get; set; }
        public string Status { get; set; }
        public DateTime DateCreated { get; set; }
        public ICollection<JobOccupant> Employees { get; set; }
        public ICollection<JobQualification> Qualifications { get; set; }
        public ICollection<JobRequirement> Requirements { get; set; }
        public virtual Department Department { get; set; }
    }
}