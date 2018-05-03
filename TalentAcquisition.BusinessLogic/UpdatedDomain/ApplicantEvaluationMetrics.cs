using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TalentAcquisition.BusinessLogic.UpdatedDomain
{
    public class ApplicantEvaluationMetrics
    {
        public int ID { get; set; }
        public int OfficePositionID { get; set; }
        [Required]
        [Display(Name = "Evaluation Code")]
        public string EvaluationCode { get; set; }
        [Required]
        [Display(Name = "Evaluation Description")]
        public string EvaluationDescription { get; set; }
        public int MaximumScore { get; set; }
        public bool Deleted { get; set; }
        public virtual Core.Domain.OfficePosition OfficePosition { get; set; }
    }
}