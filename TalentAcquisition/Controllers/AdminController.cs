using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TalentAcquisition.DataLayer;
using TalentAcquisition.Models;

namespace TalentAcquisition.Controllers
{
    public class AdminController : Controller
    {
        private AppManager app = new AppManager();
        // GET: Admin
        [AllowAnonymous]
        // GET: Applicant
        public ActionResult Portal()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                SetInitializers();
                return await app.EmployeeLogin(model, returnUrl);
            }
            catch
            {

                return View(model);
            }
        }
        //[Route("Employee/Dashboard")]
        [Route("Admin/Dashboard")]
        public ActionResult Dashboard()
        {
            SetUserSessionID();
            return View();
        }
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Portal", "Admin");
        }
             
        // GET: Admin/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: Admin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        private void SetInitializers()
        {
            var var1 = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            var var2 = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            app.UserManager = var2; app.SignInManager = var1;
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        private void SetUserSessionID()
        {
            if (TempData["userid"] == null)
            {
                var userid = User.Identity.GetUserId();
                var applicantid = new TalentContext().Applicants.Where(s => s.UserId == userid).FirstOrDefault().ID;
                TempData["userid"] = applicantid;
            }

        }

    }
}
