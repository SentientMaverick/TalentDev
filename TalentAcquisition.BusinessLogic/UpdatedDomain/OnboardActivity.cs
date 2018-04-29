using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TalentAcquisition.BusinessLogic.UpdatedDomain
{
    public enum ActivityType
    {
        Custom,Bank,Tax,Health,
    }
    public class OnboardActivity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ActivityType Type { get; set; }
        public string Description { get; set; }
    }
}