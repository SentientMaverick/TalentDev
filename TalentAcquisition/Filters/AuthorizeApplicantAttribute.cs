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
        public AuthorizeApplicantAttribute()
        {
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
                {
                    filterContext.Result = new RedirectResult("~/Applicant/Portal");
                }
        }
}