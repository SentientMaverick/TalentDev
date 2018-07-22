using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TalentAcquisition.BusinessLogic.UpdatedDomain
{
    public class ExitInterview
    {
        public ExitInterview()
        {
            ExitCheckList = new HashSet<ExitActivityLine>();
        }
        [Key]
        public string No { get; set; }
        [Required]
        public string EmployeeNo { get; set; }
        [Required]
        public string EmployeeName { get; set; }
        [Required]
        [Display(Name = "Reason for Leaving")]
        public string Reason { get; set; }
        [Display(Name = "Reason for Leaving Other")]
        public string OtherReasons { get; set; }
        [Display(Name = "Date of Leaving")]
        public DateTime LeavingOn { get; set; }
        [Display(Name = "ReEmploy in Future")]
        public bool ReEmploy { get; set; }
        [Display(Name = "Interview Date")]
        public DateTime InterviewDate { get; set; }
        [Display(Name = "Interviewer No")]
        public string InterviewerNo { get; set; }
        [Display(Name = "Interviewer Name")]
        public string InterviewerName { get; set; }
        [Display(Name = "Comment")]
        public string Comment { get; set; }
        public string Status{ get; set; }
        [Display(Name = "Approve Process")]
        public bool ApproveProcess { get; set; }
        [Display(Name = "Process Complete")]
        public bool ProcessCompleted { get; set; }
        public virtual ICollection<ExitActivityLine> ExitCheckList {get;set;}
    }
    public class ExitActivityLine
    {
        public ExitActivityLine()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ExitInterviewNo { get; set; }
        public bool Completed { get; set; }
        public ExitInterview ExitInterview{ get; set; }
    }
    public class ExitActivity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}