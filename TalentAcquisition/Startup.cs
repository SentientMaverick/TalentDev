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
            //SetupBasicEnviroment();
            //createEmployees();
            //createOfficePositions();
            //createOfficePositions();
        }

        private void SetupBasicEnviroment()
        {
            TalentContext context = new TalentContext();
            var checkaccount = context.Employees.Find(1);
            if (checkaccount == null)
            {
                setupSuperAdminAccount();
                setupAllPriviledges();
            }
            context.SaveChanges();
        }
        private void setupAllPriviledges()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            // creating Creating Manager role    
            #region BroadPermissions
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
            if (!roleManager.RoleExists("HOD"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "HOD";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("CEO"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CEO";
                roleManager.Create(role);
            }
            #endregion
            #region StreamlinedPermissionsForCash
            if (!roleManager.RoleExists("CanCreateCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanCreateCash";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("CanApproveCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanApproveCash";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("CanViewCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanViewCash";
                roleManager.Create(role);
            }
            #endregion
            #region StreamlinedPermissionsForJobRole
            if (!roleManager.RoleExists("CanCreateCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanCreateCash";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("CanApproveCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanApproveCash";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("CanViewCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanViewCash";
                roleManager.Create(role);
            }
            #endregion
            #region StreamlinedPermissionsForPersonnel
            if (!roleManager.RoleExists("CanCreateCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanCreateCash";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("CanApproveCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanApproveCash";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("CanViewCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanViewCash";
                roleManager.Create(role);
            }
            #endregion
            #region StreamlinedPermissionsForJobRequisition
            if (!roleManager.RoleExists("CanCreateCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanCreateCash";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("CanApproveCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanApproveCash";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("CanViewCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanViewCash";
                roleManager.Create(role);
            }
            #endregion
            #region StreamlinedPermissionsForTraining
            if (!roleManager.RoleExists("CanCreateCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanCreateCash";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("CanApproveCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanApproveCash";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("CanViewCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanViewCash";
                roleManager.Create(role);
            }
            #endregion
            #region StreamlinedPermissionsForOnboarding
            if (!roleManager.RoleExists("CanCreateCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanCreateCash";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("CanApproveCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanApproveCash";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("CanViewCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanViewCash";
                roleManager.Create(role);
            }
            #endregion
            #region StreamlinedPermissionsForPermissionsGroups
            if (!roleManager.RoleExists("CanCreateCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanCreateCash";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("CanApproveCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanApproveCash";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("CanViewCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanViewCash";
                roleManager.Create(role);
            }
            #endregion
            #region StreamlinedPermissionsForApprovalFlows
            if (!roleManager.RoleExists("CanCreateCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanCreateCash";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("CanApproveCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanApproveCash";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("CanViewCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanViewCash";
                roleManager.Create(role);
            }
            #endregion
            #region StreamlinedPermissionsForGrievance
            if (!roleManager.RoleExists("CanCreateCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanCreateCash";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("CanApproveCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanApproveCash";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("CanViewCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanViewCash";
                roleManager.Create(role);
            }
            #endregion
            #region StreamlinedPermissionsForLeave
            if (!roleManager.RoleExists("CanCreateCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanCreateCash";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("CanApproveCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanApproveCash";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("CanViewCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanViewCash";
                roleManager.Create(role);
            }
            #endregion
            #region StreamlinedPermissionsForExit
            if (!roleManager.RoleExists("CanCreateCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanCreateCash";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("CanApproveCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanApproveCash";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("CanViewCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanViewCash";
                roleManager.Create(role);
            }
            #endregion
            #region StreamlinedPermissionsForPerformanceMetrics
            if (!roleManager.RoleExists("CanCreateCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanCreateCash";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("CanApproveCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanApproveCash";
                roleManager.Create(role);
            }
            if (!roleManager.RoleExists("CanViewCash"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "CanViewCash";
                roleManager.Create(role);
            }
            #endregion
        }
        private void setupSuperAdminAccount()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            
            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("SuperAdmin"))
            {
                bool admiRoleIsCreated = roleManager.RoleExists("Admin");
                // first we create Admin rool   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "SuperAdmin";
                roleManager.Create(role);
                var role2 = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                if (!admiRoleIsCreated)
                {
                    role2.Name = "Admin";
                    roleManager.Create(role2);
                }
              
                //Here we create a Admin super user who will maintain the website                  

                var user = new ApplicationUser();
                user.UserName = "SystemAdministrator";
                //set System Administrator email for Account
                user.Email = "systemadministrator@mail.com";
                //set System Administrator Password for Account
                string userPWD = "Trixter34!";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var employee = new Employee() { UserId = user.Id, FirstName = user.UserName, LastName = user.Email };
                    TalentContext _context = new TalentContext();
                    _context.Employees.Add(employee);
                    _context.SaveChanges();
                    var result1 = UserManager.AddToRole(user.Id, "SuperAdmin");
                    if (!admiRoleIsCreated)
                    {
                        var result2 = UserManager.AddToRole(user.Id, "Admin");
                    }
                    var userrole = UserManager.GetRoles(user.Id);
                    if (userrole.Count > 0)
                    {
                        //redirect to Employees Page
                    }
                }
            }
        }
    }
}
