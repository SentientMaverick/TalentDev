using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalentAcquisition.DataLayer;

namespace TalentAcquisition.Filters
{
    public class AuthorizeApplicantAttribute:AuthorizeAttribute
    {
        private AppManager app = new AppManager();
        private string returnto,returnurl;
        public AuthorizeApplicantAttribute()
        {
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (Equals(httpContext.User.Identity.Name, ""))
            {
                returnto = httpContext.Request.CurrentExecutionFilePath;
                returnurl = httpContext.Request.Url.ToString();
                return false;
            }
            else
                return true;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
                {
            //filterContext.HttpContext.Response.
            filterContext.Result = new RedirectResult("~/Applicant/Portal?returnUrl="+returnto);
          
                }
        }
}