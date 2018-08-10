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
    public class AuthorizeApplicantAttribute : AuthorizeAttribute
    {
        private AppManager app = new AppManager();
        ApplicationDbContext context = new ApplicationDbContext();


        private string returnto, returnurl;
        public AuthorizeApplicantAttribute()
        {
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            try
            {
                if (Equals(httpContext.User.Identity.Name, ""))
                {
                    returnto = httpContext.Request.CurrentExecutionFilePath;
                    returnurl = httpContext.Request.Url.ToString();
                    return false;
                }
                var user = UserManager.FindByName(httpContext.User.Identity.Name);
                if (user.Roles.Count != 0)
                {
                    returnto = httpContext.Request.CurrentExecutionFilePath;
                    returnurl = httpContext.Request.Url.ToString();
                    return false;
                }
                else
                    return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext ctx)
        {
            //filterContext.HttpContext.Response.
            //ctx.Result = new RedirectResult("~/Applicant/Portal?returnUrl="+returnto);
            if (!ctx.HttpContext.User.Identity.IsAuthenticated)
                ctx.Result = new RedirectResult("~/Applicant/Portal?returnUrl="+returnto);
                //base.HandleUnauthorizedRequest(ctx);
            else
            {
                if (true)
                {
                    // handle controller access
                    ctx.Result = new ViewResult { ViewName = "Unauthorized" };
                    ctx.HttpContext.Response.StatusCode = 403;
                }
                else
                {
                    // handle menu links
                    ctx.Result = new HttpUnauthorizedResult();
                    ctx.HttpContext.Response.StatusCode = 403;
                }

            }
        }
    }
}