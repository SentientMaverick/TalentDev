using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TalentAcquisition.BusinessLogic.UpdatedDomain
{
    public class OnboardActivity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<OnboardingTemplate> OnboardingTemplates { get; set; }
    }
}