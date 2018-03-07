using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TalentAcquisition.Core.Domain
{
    public class InterviewDetail
    {
        public InterviewDetail()
        {

        }
        [Key]
        public int InterviewDetailID {get;set;}

        public int InterviewID { get; set; }
        public int TeamMember1ID { get; set; }
        public int TeamMember2ID { get; set; }
        public int TeamMember3ID { get; set; }
        public int TeamMember4ID { get; set; }
        //
        public bool? TeamMember1Recommendation { get; set; }
        public bool? TeamMember2Recommendation { get; set; }
        public bool? TeamMember3Recommendation { get; set; }
        public bool? TeamMember4Recommendation { get; set; }
        //
        public string ApplicantStrengthE1 { get; set; }
        public string ApplicantStrengthE2 { get; set; }
        public string ApplicantStrengthE3 { get; set; }
        public string ApplicantStrengthE4 { get; set; }
        //
        public string ApplicantWeaknessE1 { get; set; }
        public string ApplicantWeaknessE2 { get; set; }
        public string ApplicantWeaknessE3 { get; set; }
        public string ApplicantWeaknessE4 { get; set; }
        [Required]
        public virtual Interview Interview { get; set; }
    }
}