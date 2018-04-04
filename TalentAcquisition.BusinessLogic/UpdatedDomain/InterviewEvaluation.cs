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
        public int InterviewID { get; set; }
       // public int JobApplicationID { get; set; }
        public int StageID { get; set; }
        public int Score1 { get; set; }
        public int Score2 { get; set; }
        public int Score3 { get; set; }
        public string ApplicantStrength { get; set; }
        public string ApplicantWeakness { get; set; }
        public string Recommendation { get; set; }
        public bool RecommendForHire { get; set; }
        public bool RecommendForStage2 { get; set; }
        public bool RecommendForStage3 { get; set; }
        public bool JobAcceptance { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Interview Interview { get; set; }
       // public virtual JobApplication JobApplication { get; set; }
    }
}