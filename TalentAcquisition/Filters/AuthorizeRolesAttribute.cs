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
        private readonly string[] userAssignedRoles;
        public AuthorizeRolesAttribute(params string[] roles)
        {
            this.userAssignedRoles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            using (TalentContext db = new TalentContext())
            {
                foreach (var roles in userAssignedRoles)
                {
                    authorize = app.IsUserInRole(httpContext.User.Identity.Name, roles);
                    if (authorize)
                        return authorize;
                }
                return authorize;
            }
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
                {
                    filterContext.Result = new RedirectResult("~/Admin/Unauthorized");
                }
        }
}