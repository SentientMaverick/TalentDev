using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalentAcquisition.DataLayer;

namespace TalentAcquisition.Filters
{
    public class AuthorizeRolesAttribute:AuthorizeAttribute
    {
        private AppManager app = new AppManager();
        ApplicationDbContext context = new ApplicationDbContext();

        private readonly string[] userAssignedRoles;
        public AuthorizeRolesAttribute(params string[] roles)
        {
            this.userAssignedRoles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            try
            {
                if (Equals(httpContext.User.Identity.Name, ""))
                {
                    return false;
                }
                foreach (var roles in userAssignedRoles)
                {
                    var id = UserManager.FindByName(httpContext.User.Identity.Name).Id;
                    authorize = UserManager.IsInRole(id, roles);
                    if (authorize)
                        return authorize;
                }
                return authorize;
            }
            catch (Exception ex)
            {
                 return authorize;
            }  
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
                {
                    filterContext.Result = new RedirectResult("~/Admin/Unauthorized");
                }
        }
}