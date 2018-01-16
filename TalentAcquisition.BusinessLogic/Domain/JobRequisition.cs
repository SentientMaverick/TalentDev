using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TalentAcquisition.Core.Domain
{
    public class JobRequisition
    {
        public enum JobRequisitionStatus
        {
            Created,Posted
        }
        public JobRequisition()
        {
            JobApplications= new HashSet<JobApplication>();
        }
        public int JobRequisitionID { get; set; }
        public int OfficePositionID { get; set; }
        public string JobTitle { get; set; }
        public string Location { get; set; }
        public JobRequisitionStatus? Status { get; set; }
        public int NoOfPositionsAvailable { get; set; }
        public string JobDescription { get; set; }
        public int AgeLimit { get; set; }
        public string EducationalRequirements { get; set; }
        public string JobResponsibilities { get; set; }
        public int YearsOfExperience { get; set; }
        public int HumanResourcePersonnelID { get; set; }
        public int HeadOfDepartmentID { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ClosingDate { get; set; }
        public string joburl { get; set; }
        public Employee Employee { get; set; }
        public Employee Employee1 { get; set; }
        public virtual ICollection<JobApplication> JobApplications { get; set; }
        public  virtual OfficePosition OfficePosition { get; set; }

        public string ApplicationDeadline
        {
            get
            {
                var deadline = ClosingDate - StartDate;
                return (deadline.Days / 7).ToString() ;
            }
        }

    }
}