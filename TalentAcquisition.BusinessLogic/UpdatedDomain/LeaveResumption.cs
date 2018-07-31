using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TalentAcquisition.BusinessLogic.UpdatedDomain
{
    public class LeaveResumption
    {

        [Key]
        public int ID { get; set; }
        [Required]
        [Display(Description = "Leave Application")]
        public int LeaveAppID { get; set; }
        [Display(Description = "Employee Id")]
        public string EmployeeId { get; set; }
        [Display(Description = "Leave Type")]
        public string LeaveType { get; set; }
        [Display(Description = "Employee Name")]
        public string EmployeeName { get; set; }
        [Display(Description = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Description = "Resumption Date")]
        public DateTime ResumptionDate { get; set; }
        [Display(Description = "Application Date")]
        public DateTime ApplyDate { get; set; }
        public int Duration { get; set; }
        public enum LeaveResumptionStatus
        {
            Approved, Pending, Rejected
        }
        public LeaveResumptionStatus Status { get; set; }
    }
}