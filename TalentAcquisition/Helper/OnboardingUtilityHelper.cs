using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TalentAcquisition.BusinessLogic.UpdatedDomain;
using TalentAcquisition.Models.ViewModel;

namespace TalentAcquisition.Helper
{
    public static class OnboardingUtilityHelper
    {
        public static ActivityViewModel ConvertToActivityModel(OnboardActivity onboardactivity)
        {
            var activity = new ActivityViewModel();
            activity.OnboardActivityID = onboardactivity.ID;
            activity.Type = onboardactivity.Type;
            activity.Title = onboardactivity.Name;
            return activity;
        }
    }
}