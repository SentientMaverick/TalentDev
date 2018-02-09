using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TalentAcquisition.Models.ViewModel
{
    public class AdminDashboardNotification
    {
        public AdminDashboardNotification()
        {
            Notifications = new List<DashboardNotification>();
        }
        public int TotalNotificationCount { get; set; }
        public int newJobRequisitionCount { get; set; }
        public int activeRequisitionCount { get; set; }
        public List<DashboardNotification> Notifications { get; set; }
    }
    public class DashboardNotification
    {
        [Key]
        public int ID { get; set; }
        public string url { get; set; }
        public string Title { get; set; }
        public int JobApplicationCount { get; set; }
    }
}