using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TalentAcquisition.BusinessLogic.UpdatedDomain;

namespace TalentAcquisition.Core.Domain
{
    public class OfficePosition
    {

        public int OfficePositionID { get; set; }
        [Display(Name = "Job ID")]
        public string JobID { get; set; }
        //  public int RoleID { get; set; }
        public int DepartmentID { get; set; }
        public int IndustryID { get; set; }
        public string Title { get; set; }
        public string RoleSummary { get; set; }
        [Display(Name ="Role Functions")]
        public string Reqirements { get; set; }
        //public string RoleNumber { get; set; }
        public bool IsAvailable
        { get {
                return false;
            }
            set { }
        }
        [Display(Name = "Job Description")]
        public string JobDescription { get; set; }
        [Display(Name = "No Of Posts")]
        [Required]
        public int? Posts { get; set; }
        [Display(Name = "Position Reporting To")]
        public int? SupervisorID { get; set; }
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
        //public ICollection<JobQualification> Qualifications { get; set; }
        public ICollection<JobRequirement> Requirements { get; set; }
        public virtual Department Department { get; set; }
       public virtual Industry Industry { get; set; }
    }
    /*
    public class Role
    {
        public int RoleID { get; set; }
       // public string Title { get; set; }
        public string RoleName { get; set; }
        public string RoleSummary { get; set; }
        public string Reqirements { get; set; }
    }*/
}