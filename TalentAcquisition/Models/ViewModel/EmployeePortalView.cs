using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TalentAcquisition.Models.ViewModel
{
    public class EmployeePortalLoginView
    {
        [Key]
        public int ID { get; set; }
    }
    public class EmployeePortalJobRequisitionView
    {
        [Key]
        public int ID { get; set; }
        public ICollection<JobListing> JobListings;
    }
    public class JobApplication
    {
        public int ID { get; set; }
    }
    public class EmployeePortalManageApplicationsView
    {
        [Key]
        public int ID { get; set; }
        public ICollection<JobApplication> JobApplications;
    }
}