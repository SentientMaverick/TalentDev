using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TalentAcquisition.BusinessLogic.UpdatedDomain
{
    public class ManageEmployeeLeave
    {
        [Key]
        public int ID { get; set; }
        [Display(Description = "Employee Id")]
        public string EmployeeId { get; set; }
        [Display(Description = "Employee Name")]
        public string EmployeeName { get; set; }

        public string LeaveType { get; set; }
        public int? LeaveLimit { get; set; }

        public virtual LeaveType_Limit LeaveType_Limit {get; set;}

        public int? TotalLeaveTaken { get; set; }
        public int? TotalLeaveAvailable { get; set; } 

        [Display(Description = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Description = "End Date")]
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }

        [Display(Description = "Applied Date")]
        public DateTime ApplyDate { get; set; }
        public enum LeaveStatus
        {
            Approved, Pending, Rejected,
        }

        public LeaveStatus Status { get; set; }
    }
}