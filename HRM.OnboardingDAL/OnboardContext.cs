using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalentAcquisition.BusinessLogic.UpdatedDomain;
using TalentAcquisition.Core.Domain;

namespace HRM.OnboardingDAL
{
        public class OnboardContext : DbContext
        {
            public OnboardContext() : base("name=TalentConnection")
            {

            }
            public static OnboardContext Create()
            {
                return new OnboardContext();
            }
            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            }
            public DbSet<JobSeeker> Applicants { get; set; }
            public DbSet<Employee> Employees { get; set; }
            public DbSet<JobApplication> JobApplications { get; set; }
            public DbSet<Branch> Branches { get; set; }
            public DbSet<OnboardingTemplate> OnboardingTemplates { get; set; }
            public DbSet<WelcomeGuide> WelcomeGuides { get; set; }
            public DbSet<OnboardActivity> OnboardActivities { get; set; }
            public DbSet<CompletedActivity> CompletedActivities { get; set; }
    }
}
