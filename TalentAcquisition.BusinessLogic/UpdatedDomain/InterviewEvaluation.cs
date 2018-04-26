using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TalentAcquisition.Core.Domain;

namespace TalentAcquisition.BusinessLogic.UpdatedDomain
{
    public class InterviewEvaluation
    {
        [Key]
        public int ID { get; set; }
        public string EvaluationNo { get; set; }
        public int EmployeeID { get; set; }
        [Display(Name = "Interview ID")]
        public int InterviewID { get; set; }
        // public int JobApplicationID { get; set; }
        [Display(Name = "Stage ID")]
        public int StageID { get; set; }
        [Display(Name = "Stage 1 Score")]
        public int Score1 { get; set; }
        [Display(Name = "Stage 2 Score")]
        public int Score2 { get; set; }
        [Display(Name = "Stage 3 Score")]
        public int Score3 { get; set; }
        public string ApplicantStrength { get; set; }
        public string ApplicantWeakness { get; set; }
        public string Recommendation { get; set; }
        [Display(Name = "Recommend For Hire ")]
        public bool RecommendForHire { get; set; }
        [Display(Name = "Recommend For Stage 2 ")]
        public bool RecommendForStage2 { get; set; }
        [Display(Name = "Recommend For Stage 3 ")]
        public bool RecommendForStage3 { get; set; }
        [Display(Name = "Job Acceptance ")]
        public bool JobAcceptance { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Interview Interview { get; set; }
       // public virtual JobApplication JobApplication { get; set; }
    }
}