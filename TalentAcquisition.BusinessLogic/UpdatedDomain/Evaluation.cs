using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TalentAcquisition.BusinessLogic.UpdatedDomain
{
    public class Evaluation
    {
        public int ID { get; set; }
        public int InterviewEvaluationID { get; set; }
        [Display(Name="Evaluation Code")]
        public string EvaluationCode { get; set; }
        [Display(Name = "Evaluation Description")]
        public string EvaluationDescription { get; set; }
        [Display(Name = "Stage 1 Score")]
        public int Score1 { get; set; }
        [Display(Name = "Stage 2 Score")]
        public int Score2 { get; set; }
        [Display(Name = "Stage 3 Score")]
        public int Score3 { get; set; }
        public virtual InterviewEvaluation InterviewEvaluation { get; set; }
    }
}