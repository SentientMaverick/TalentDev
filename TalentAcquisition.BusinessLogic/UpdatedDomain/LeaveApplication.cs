using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TalentAcquisition.BusinessLogic.UpdatedDomain
{
    public class LeaveApplication
    {
        [Key]
        public int LeaveAppID { get; set; }
        [Display(Description = "Employee Id")]

        public int? LeavePlanID { get; set; }
        public string EmployeeId { get; set; }
        [Display(Description = "Employee Name")]
        public string EmployeeName { get; set; }

        public string LeaveType { get; set; }
        public int? LeaveLimit { get; set; }

        public virtual LeaveType_Limit LeaveType_Limit { get; set; }

        public int? TotalLeaveTaken { get; set; }
        public int? TotalLeaveAvailable { get; set; }

        [Display(Description = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Description = "End Date")]
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }

        [Display(Description = "Applied Date")]
        public DateTime ApplyDate { get; set; }
             
        public string LeavePlanStatus{ get; set; }
        public enum LeaveApplicationStatus
        {
            Approved, Pending, Rejected,Completed
        }
        public LeaveApplicationStatus LeaveAppStatus { get; set; }
    }
}