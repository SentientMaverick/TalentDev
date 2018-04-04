using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TalentAcquisition.BusinessLogic.UpdatedDomain
{
    public class JobRequirement
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int QualificationID { get; set; }
        public int OfficePositionID { get; set; }
        [Display(Name = "Qualification Type")]
        public string QualificationType { get; set; }
        [Display(Name = "Qualification Code")]
        public string QualificationCode { get; set; }
        [Display(Name = "Qualification Description")]
        public string QualificationDescription { get; set; }
        [Display(Name = "Priority")]
        public string Priority { get; set; }
        [Display(Name = "Score ID")]
        public decimal ScoreID { get; set; }
        [Display(Name = "Need Code")]
        public double NeedCode { get; set; }
        [Display(Name = "Stage Code")]
        public double StageCode { get; set; }
        public bool Mandatory { get; set; }
        [Display(Name = "Desired Score")]
        public decimal DesiredScore { get; set; }
        public virtual Core.Domain.OfficePosition OfficePosition { get; set; }
    }
}