using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TalentAcquisition.BusinessLogic.UpdatedDomain
{
    public class EvaluationCategory
    {
        public int ID { get; set; }
        public int InterviewID { get; set; }
        [Required]
        [Display(Name = "Evaluation Code")]
        public string EvaluationCode { get; set; }
        [Required]
        [Display(Name = "Evaluation Description")]
        public string EvaluationDescription { get; set; }
        public bool Deleted { get; set; }
        public virtual Core.Domain.Interview Interview { get; set; }
    }
}