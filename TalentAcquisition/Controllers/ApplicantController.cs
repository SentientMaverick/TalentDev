using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TalentAcquisition.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using TalentAcquisition.Core.Domain;
using TalentAcquisition.DataLayer;
using TalentAcquisition.Filters;

namespace TalentAcquisition.Controllers
{
    [AuthorizeApplicant]
    public class ApplicantController : Controller
    {
       private AppManager app = new AppManager();
        
        [AllowAnonymous]
        // GET: Applicant
        public ActionResult Portal()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Applicant");
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model,string returnUrl)
        {
            try
            {
                SetInitializers();
                return await app.ApplicantLogin(model, returnUrl);
            }
            catch
            {
               
                return View(model);
            }
        }

        private void SetInitializers()
        {
            var var1 = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            var var2 = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            app.UserManager = var2; app.SignInManager = var1;
        }

        [AllowAnonymous]
        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Register(RegisterViewModel model, string returnUrl)
        {
            SetInitializers();
            return await app.ApplicantRegistration(model, returnUrl);
        }
        public ActionResult Dashboard()
        {
            SetUserSessionID();
            return View();
            //return View("Home/Index");
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
        public ActionResult ManageApplications()
        {
            return View();
        }

        [Route("Applicant/Profile")]
        public ActionResult ManageProfile()
        {
            SetUserSessionID();
            var user = getUserProfile();
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Applicant/Profile")]
        public async Task<ActionResult> ManageProfile(JobSeeker applicant)
        {
            applicant.ID = (int)TempData["userid"];
            return await app.SaveBioDetails(applicant);
        }
        [Route("Applicant/Profile/Uploadcv")]
        public ActionResult UploadCv()
        {
            return PartialView();
        }
        [Route("Applicant/Profile/Passport")]
        public ActionResult Passport()
        {
            return PartialView();
        }
        [Route("Applicant/Profile/Education")]
        public ActionResult Education()
        {
            return PartialView();
        }
        [Route("Applicant/Profile/Employment")]
        public ActionResult Employment()
        {
            SetUserSessionID();
            var id = (int)TempData["userid"];
            ViewBag.Userid= id;
            var employment = new WorkExperience {StartingDate=DateTime.Now,EndingDate=DateTime.Now,JobSeekerID=id };
            return PartialView(employment);
        }
        [Route("Applicant/Profile/NewEmployment")]
       // [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult NewEmployment(WorkExperience work)
        {
            if (ModelState.IsValid)
            {
                using (var db=new TalentContext())
                {
                    db.WorkExperiences.Add(work);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Profile/Employment","Applicant");
        }

        [Route("Applicant/Profile/Certification")]
        public ActionResult Certification()
        {
            SetUserSessionID();
            var applicantid = (int)TempData["userid"];
            var certifications = new List<Certification>();
            using (var db = new TalentContext())
            {
                certifications = db.Certifications.Where(x => x.JobSeekerID == applicantid).ToList();
            }
            return PartialView(certifications);
        }

        [ChildActionOnly]
        public PartialViewResult _GetCertifications()
        {
            SetUserSessionID();
            var applicantid = (int)TempData["userid"];
            var certifications = new List<Certification>();
            using (var db = new TalentContext())
            {
                certifications = db.Certifications.Where(x => x.JobSeekerID == applicantid).ToList();
            }
            return PartialView();
        }

        [ChildActionOnly]
        public PartialViewResult _GetSchools()
        {
            var applicantid = (int)TempData["userid"];
            var schools = new List<School>();
            using (var db = new TalentContext())
            {
                schools = db.Schools.Where(x => x.JobSeekerID == applicantid).ToList();
            }
            return PartialView();
        }

        [ChildActionOnly]
        public PartialViewResult _GetEmploymentList()
        {
            var applicantid = (int)TempData["userid"];
            var employments = new List<WorkExperience>();
            using (var db = new TalentContext())
            {
                employments = db.WorkExperiences.Where(x => x.JobSeekerID == applicantid).ToList();
            }
            return PartialView(employments);
        }
        [Route("Applicant/Profile/Overview")]
        public ActionResult Overview()
        {
            SetUserSessionID();
            var applicantid = (int)TempData["userid"];
            var applicant = new JobSeeker();
            using (var db = new TalentContext())
            {
                //applicant= db.Applicants.Include("WorkExperiences").FirstOrDefault(x => x.ID == applicantid);
                ViewBag.Links = "Hide";
               applicant = db.Applicants.Include("Schools").Include("Certifications").Include("WorkExperiences").FirstOrDefault(x => x.ID == applicantid);
            }
            return View(applicant);
        }

        private JobSeeker getUserProfile()
        {
            var currentuser = new JobSeeker();
            using (var db = new TalentContext())
            {
                var id = User.Identity.GetUserId();
                var userid = db.Applicants.Where(s => s.UserId == id).FirstOrDefault().ID;
                currentuser = db.Applicants.Where(s => s.ID == userid).FirstOrDefault();
            }
            return currentuser;
        }
        public ActionResult RegistrationSuccess()
        {
            if (User.Identity.IsAuthenticated)
            {
                var var3 = HttpContext.GetOwinContext().Authentication;
                var3.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            }
            return View();
            //return View("Home/Index");
        }
        
    }
}