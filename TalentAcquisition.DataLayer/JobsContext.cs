using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using TalentAcquisition.Core.Domain;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using TalentAcquisition.BusinessLogic.UpdatedDomain;

namespace TalentAcquisition.DataLayer
{
    public class JobsContext:DbContext
    {
        public JobsContext() : base("name=TalentConnection")
        {

        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Entity<JobRequisition>()
            //    .Property(s => s.Employee1)
            //    .HasColumnName("HeadOfDepartmentID");
            //modelBuilder.Entity<JobRequisition>().
            //    Property(s => s.Employee.ID).
            //    HasColumnName("HumanResourcePersonnelID");
            // modelBuilder.Entity<JobSeeker>().Property(e => e.RegistrationDate).HasColumnType("datetime2");
        }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobQualification> JobQualifications { get; set; }
        public DbSet<JobRequirement> JobRequirements { get; set; }
        public DbSet<JobOccupant> JobOccupants { get; set; }
    }
}