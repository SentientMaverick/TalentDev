using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using TalentAcquisition.DataLayer;
using TalentAcquisition.Core.Domain;
using System.Data.Entity.Migrations;

[assembly: OwinStartupAttribute(typeof(TalentAcquisition.Startup))]
namespace TalentAcquisition
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //createEmployees();
            //createOfficePositions();
            createOfficePositions();
        }

        private void createOfficePositions()
        {
            TalentContext context = new TalentContext();
            for (int i = 1; i <= 5; i++)
            {
                var job = context.OfficePositions.Find(i);
                var requisition = new JobRequisition()
                {
                    JobRequisitionID = i,
                    HeadOfDepartmentID = 1,
                    HumanResourcePersonnelID = 2,
                    JobTitle = job.Title,
                    JobDescription = job.RoleSummary,
                    PublishedDate = DateTime.Now,
                    StartDate = DateTime.Now.AddDays(i),
                    ClosingDate = DateTime.Now.AddDays(14),
                    Location = "Lagos",
                    Status = JobRequisition.JobRequisitionStatus.Posted,
                    NoOfPositionsAvailable = 1,
                    OfficePositionID = job.OfficePositionID
                };

           context.JobRequisitions.Add(requisition);
        }
          // context.SaveChanges();
        }

        private void createEmployees()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Admin"))
            {
                // first we create Admin rool   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                  

                var user = new ApplicationUser();
                user.UserName = "shanu";
                user.Email = "syedshanumcain@gmail.com";
                //user.Address = "Ojodu Berger";
                //user.BirthDate = System.DateTime.Parse("12/12/1989");
                //user.City = "Lagos";
                //user.State = "Lagos";
                string userPWD = "Trixter34!";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");
                    var userrole = UserManager.GetRoles(user.Id);
                    if (userrole.Count > 0)
                    {
                        //redirect to Employees Page
                    }
                }
            }

            // creating Creating Manager role    
            if (!roleManager.RoleExists("Manager"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Manager";
                roleManager.Create(role);

            }

            // creating Creating Employee role    
            if (!roleManager.RoleExists("Employee"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Employee";
                roleManager.Create(role);

            }
        }
    }
}
