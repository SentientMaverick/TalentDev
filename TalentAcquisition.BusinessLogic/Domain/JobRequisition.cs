using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TalentAcquisition.BusinessLogic.Domain;


namespace TalentAcquisition.Core.Domain
{
    public class JobRequisition
    {
        public enum JobRequisitionStatus
        {
            Created,Posted,Completed,Rejected
        }
        public JobRequisition()
        {
            JobApplications= new HashSet<JobApplication>();
            Skills = new HashSet<Skill>();
        }
        public int JobRequisitionID { get; set; }
        public int OfficePositionID { get; set; }
        [Display(Name ="Title")]
        public string JobTitle { get; set; }
        [Display(Name = "Location")]
        public string Location { get; set; }
        [Display(Name = "Minimum Grade")]
        public GradeObtained? HighestQualification { get; set; }
        [Display(Name = "Maximum Grade")]
        public GradeObtained? MinimumQualification { get; set; }
        public JobRequisitionStatus? Status { get; set; }
        [Display(Name = "Positions Available")]
        public int NoOfPositionsAvailable { get; set; }
        [Display(Name = "Role Description")]
        public string JobDescription { get; set; }
        [Display(Name = "Upper Age Limit")]
        public int AgeLimit { get; set; }
        [Display(Name = "Educational Qualifications")]
        public string EducationalRequirements { get; set; }
        [Display(Name = "Responsibilities")]
        public string JobResponsibilities { get; set; }
        [Display(Name = "No of Years Of Experience")]
        public int YearsOfExperience { get; set; }
        public int HumanResourcePersonnelID { get; set; }
        public int HeadOfDepartmentID { get; set; }
        public DateTime PublishedDate { get; set; }
        [Display(Name = "Starting Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Closing Date")]
        public DateTime ClosingDate { get; set; }
        public string joburl { get; set; }
        public string RejectionNote { get; set; }
        public Employee Employee { get; set; }
        public Employee Employee1 { get; set; }
        public virtual ICollection<JobApplication> JobApplications { get; set; }
        public  virtual OfficePosition OfficePosition { get; set; }
        public virtual ICollection<Skill> Skills { get; set; }

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