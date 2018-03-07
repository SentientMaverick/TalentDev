using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TalentAcquisition.BusinessLogic.UpdatedDomain
{
    public class JobQualification
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Qualification Type")]
        public string QualificationType { get; set; }
        [Display(Name = "Qualification Code")]
        public string QualificationCode { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}