using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Talent.HRM.Services.Notification;

namespace TalentAcquisition.Models.ViewModel
{
    public class DashboardViewModel
    {
        public GeneralNotificationDTO _generalNotifications { get; protected set; }
        public EmployeeApprovalDTO _employeeApprovals { get; protected set; }
        public DashboardViewModel(GeneralNotificationDTO generalNotifications, EmployeeApprovalDTO employeeApprovals)
        {
            _generalNotifications = generalNotifications;
            _employeeApprovals = employeeApprovals;
        }
    }
}