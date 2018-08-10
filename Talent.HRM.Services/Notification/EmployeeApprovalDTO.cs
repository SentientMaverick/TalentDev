using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talent.HRM.Services.Notification
{
    public class EmployeeApprovalDTO
    {
        public int Training { get; set; }
        public int Onboarding { get; set; }
        public int JobRequisition { get; set; }
        public int Cash { get; set; }
        public int PerformanceAppraisal { get; set; }
        public int Complaint { get; set; }
        public int LeaveApplication { get; set; }
        public int LeavePlan { get; set; }
    }
}
