using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TalentAcquisition.Core.Domain;

namespace TalentAcquisition.BusinessLogic.UpdatedDomain
{
    public enum Status
    {
        Review,Published,Incomplete,Complete
    }
    public class WelcomeGuide
    {
        public int ID { get; set; }
        [Required]
        [Display(Name="Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Position")]
        public string Position { get; set; }
        public string previewurl { get; set; }
        public Status Status { get; set; }
        public string WelcomeMessage { get; set; }
        public string Location { get; set; }
        public int? BranchID { get; set; }
        public int? JobSeekerID { get; set; }
        public int? TemplateID { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime StartDate { get; set; }
        public virtual Branch Branch { get; set; }
        public virtual JobSeeker JobSeeker { get; set; }
        public virtual ICollection<CompletedActivity> CompletedActivities { get; set; }
        public virtual ICollection<OnboardActivity> OnboardActivities { get; set; }
    }
}