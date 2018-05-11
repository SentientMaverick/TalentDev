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

        public static CompletedActivity ConvertToCompletedActivity(ActivityViewModel activitymodel)
        {
            var activity = new CompletedActivity();
            activity.OnboardingTemplateID = activitymodel.OnboardingTemplateID;
            activity.Type = activitymodel.Type;
            activity.Name = activitymodel.Title;
            activity.Description = activitymodel.Body;
            activity.DueDate = activitymodel.DueDate;
            return activity;
        }

        public static List<ActivityViewModel> ConvertToActivityModelList(List<CompletedActivity> activitylist)
        {
            var activities = new List<ActivityViewModel>();
            foreach(var activity in activitylist)
            {
                var activitymodel = new ActivityViewModel();
                activitymodel.ID = activity.ID;
                activitymodel.OnboardActivityID = activity.OnboardActivityID;
                activitymodel.Type = activity.Type;
                activitymodel.Title = activity.Name;
                activitymodel.Body = activity.Description;
                activitymodel.DueDate = activity.DueDate;
                activitymodel.Checked = activity.HasTaskBeenCompleted;
                activities.Add(activitymodel);
            }
            return activities;
        }

        public static List<CompletedActivity> ConvertToGuideActivities(List<CompletedActivity> activities,int guideid)
        {
            var guideactivities = new List<CompletedActivity>();
            guideactivities = activities.Select(o => new CompletedActivity
            { ID = o.ID,
              Name = o.Name,
              DueDate=o.DueDate,
              Description=o.Description,
              WelcomeGuideID=guideid
            }).ToList();
            return guideactivities;
        }
    }
}