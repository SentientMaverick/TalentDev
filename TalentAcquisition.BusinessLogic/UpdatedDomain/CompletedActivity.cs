using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TalentAcquisition.BusinessLogic.UpdatedDomain
{
    public class CompletedActivity
    {
        public int ID { get; set; }
        public int OnboardActivityID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasTaskBeenCompleted { get; set; }
        public ActivityType Type { get; set; }
        public int? WelcomeGuideID { get; set; }
        public int OnboardingTemplateID { get; set; }
        public DateTime DueDate { get; set; }
        public virtual WelcomeGuide WelcomeGuide { get; set; }
    }
}