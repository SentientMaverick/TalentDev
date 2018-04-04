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
    public class AuthorizeEmployeeAttribute:AuthorizeAttribute
    {
        private string returnto;
        public AuthorizeEmployeeAttribute()
        {
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            returnto = httpContext.Request.CurrentExecutionFilePath;
            if (!Equals(httpContext.User.Identity.Name, ""))
            {    
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var id = UserManager.FindByName(httpContext.User.Identity.Name).Id;
                var user = new TalentContext().Employees.Where(x => x.UserId == id);
                if (user.Any())
                 {
                    return true;
                 }
                return false;
            }
            else
            {
                return false;
            }       
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //filterContext.HttpContext.Response.
            filterContext.Result = new RedirectResult("~/Admin/Portal?returnUrl=" + returnto);
        }
    }
}