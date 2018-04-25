using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TalentAcquisition.BusinessLogic.Domain;
using TalentAcquisition.BusinessLogic.UpdatedDomain;

namespace TalentAcquisition.Core.Domain
{
    public class JobRequisition
    {
        public enum JobRequisitionStatus
        {
            Created,
            Approved,
            Posted,
            Completed,
            Rejected
        }
        public enum JobPriority
        {
            Low,Medium, High
        }
        public JobRequisition()
        {
            JobApplications= new HashSet<JobApplication>();
            Skills = new HashSet<Skill>();
            MatchedApplicants = new HashSet<MatchedApplicant>();
        }
        public int JobRequisitionID { get; set; }
        [Display(Name = "Job")]
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
        [Display(Name = "Required Positions")]
        public int NoOfPositionsAvailable { get; set; }
        [Display(Name = "Role Description")]
        public string JobDescription { get; set; }
        [Display(Name = "Upper Age Limit")]
        public int AgeLimit { get; set; }
        [Display(Name = "Educational Qualifications")]
        public string EducationalRequirements { get; set; }
        [Display(Name = "Responsibilities")]
        public string JobResponsibilities { get; set; }
        [Display(Name = "Years Of Experience")]
        public int YearsOfExperience { get; set; }
        [Display(Name = "Job Supervisor / Manager")]
        public int HumanResourcePersonnelID { get; set; }
        [Display(Name = "Requestor")]
        public int HeadOfDepartmentID { get; set; }
        [Display(Name = "Date Approved")]
        public DateTime PublishedDate { get; set; }
        [Display(Name = "Starting Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Closing Date")]
        public DateTime ClosingDate { get; set; }
        //public string joburl { get; set; }
        public string RejectionNote { get; set; }
        //
        public JobPriority? Priority { get; set; }
        public string Stage { get; set; }
        public string Score { get; set; }
        public string StageCode { get; set; }
        [Display(Name = "Requisition No")]
        public string RequisitionNo { get; set; }
        [Display(Name = "Branch")]
        public string GlobalDimension { get; set; }
        public bool Closed { get; set; }
        public bool Qualified { get; set; }
        [Display(Name = "Turn Around Time")]
        public int? TurnAroundTime { get; set; }
        [Display(Name = "Grace Period")]
        public int? GracePeriod { get; set; }
        [Display(Name = "Required Positions")]
        public int? RequiredPositions { get; set; }
        [Display(Name = "Vacant Positions")]
        public int? VacantPositions { get; set; }
        [Display(Name = "RequisitionType")]
        public string RequisitionType { get; set; }
        [Display(Name = "Reason For Request")]
        public string ReasonForRequest { get; set; }
        [Display(Name = "Any Additional Information")]
        public string AnyAdditionalInformation { get; set; }
        [Display(Name = "Job Grade")]
        public string JobGrade { get; set; }
        [Display(Name = "Type Of Contract Required")]
        public string TypeOfContractRequired { get; set; }
        [Display(Name = "No Series")]
        public string NoSeries { get; set; }
        [Display(Name = "Responsibility Center")]
        public string ResponsibilityCenter { get; set; }
        //
        public virtual ICollection<JobApplication> JobApplications { get; set; }
        public  virtual OfficePosition OfficePosition { get; set; }
        public virtual ICollection<Skill> Skills { get; set; }
        public virtual ICollection<MatchedApplicant> MatchedApplicants { get; set; }

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