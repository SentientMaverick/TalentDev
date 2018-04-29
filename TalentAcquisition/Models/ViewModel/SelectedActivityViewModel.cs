using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TalentAcquisition.BusinessLogic.UpdatedDomain;

namespace TalentAcquisition.Models.ViewModel
{
    public class SelectedActivityViewModel
    {
        public SelectedActivityViewModel()
        {
            Activities = new List<ActivityViewModel>();
        }
        public List<ActivityViewModel> Activities { get; set; }
    }
    public class ActivityViewModel
    {
        public ActivityViewModel()
        {

        }
        [Key]
        public int ID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }
        public int OnboardActivityID { get; set; }
        public ActivityType Type { get; set; }
        public int WelcomeGuideID { get; set; }
        public int OnboardingTemplateID { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
    }
}