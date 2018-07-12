using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TalentAcquisition.BusinessLogic.UpdatedDomain
{
    public class TrainingQuestion
    {
        [Key]
        public Guid Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Remarks { get; set; }
    }
    public class TrainingCourse
    {
        [Key]
        public string Code { get; set; }
        public string Description { get; set; }
    }
    public class TrainingProvider
    {
        [Key]   
        public string Code { get; set; }
        public string Name { get; set; }
    }
    public class Training
    {
        public Training()
        {
            this.TrainingQuestions = new HashSet<TrainingQuestion>();
        }
        [Key]
        [Display(Name = "Number")]
        public string ApplicationNo { get; set; }
        [Display(Name = "Application Date")]
        public DateTime ApplicationDate { get; set; }
        [Display(Name = "Employee Number")]
        public string EmployeeNo { get; set; }
        [Display(Name = "Training Group No")]
        public int TrainingGroupNo { get; set; }
        [Display(Name = "No Of Participants")]
        public int ParticipantNo { get; set; }
        [Display(Name = "Training Course Code")]
        public string TrainingCourseCode { get; set; }
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }
        [Display(Name = "Employee Department")]
        public string EmployeeDepartment { get; set; }
        public string Description { get; set; }
        public string Purpose { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Stop Date")]
        public DateTime StopDate { get; set; }
        public float Duration { get; set; }
        [Display(Name = "Duration Units")]
        public string DurationUnits { get; set; }
        public string Location { get; set; }
        [Display(Name ="Training Provider")]
        public string TrainingProviderCode { get; set; }
        [Display(Name = "Provider Name")]
        public string ProviderName { get; set; }
        [Display(Name = "Estimated Cost")]
        public decimal EstimatedCost { get; set; }
        [Display(Name = "Approved Cost")]
        public decimal ApprovedCost { get; set; }
        public string Status { get; set; }
        public virtual TrainingProvider TrainingProvider { get; set; }
        public virtual TrainingCourse TrainingCourse { get; set; }
        public virtual ICollection<TrainingQuestion> TrainingQuestions { get; set; }
    }
}