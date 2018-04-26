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
using System.Net;
using TalentAcquisition.Models.Core;
using TalentAcquisition.BusinessLogic.Domain;

namespace TalentAcquisition.Controllers
{
    [AuthorizeApplicant]
    public class ApplicantController : Controller
    {
       private AppManager app = new AppManager();
        
        [AllowAnonymous]
        // GET: Applicant
        public ActionResult Portal(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Applicant");
            }
            TempData["Url"] = returnUrl;
            ViewBag.Message = TempData["ErrorMessage"];
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Portal(LoginViewModel model)
        {
          string returnUrl =(string)TempData["Url"];
            try
            {
                SetInitializers();
                return await app.ApplicantLogin(model, returnUrl);
            }
            catch
            {
                TempData["ErrorMessage"] = "Invalid Email and Password Combination; Check and Try again";
                return RedirectToAction("Portal");
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
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Register(RegisterViewModel model, string returnUrl)
        {
            SetInitializers();
            return await app.ApplicantRegistration(model, returnUrl);
        }
        public ActionResult Dashboard()
        {
            SetUserSessionID();
            if (TempData["userid"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
                return View();
            //return View("Home/Index");
        }
       [OutputCache(Duration =20,VaryByParam ="None")]
        private void SetUserSessionID()
        {
            if (TempData["userid"] == null)
            {
                var userid = User.Identity.GetUserId();
                var applicant = new TalentContext().Applicants.Where(s => s.UserId == userid);
                if (applicant.Any())
                {
                    var applicantid = applicant.FirstOrDefault().ID;
                    TempData["userid"] = applicantid;
                }
                else {
                   // throw new HttpException(HttpStatusCode.BadRequest,"BadRequest");
                   // throw new HttpException();
                }       
            }
        }
        [Route("Applicant/manage_application/{id:int}")]
        public ActionResult ManageApplication(int? id)
        {
            var db = new TalentContext();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobApplication application = db.JobApplications.Find(id);
           // JobApplication application = db.JobApplications.Include("JobRequisition").Include("JobSeeker").WhereFind(id);

            if (application == null)
            {
                return HttpNotFound();
            }
            try
            {
                ViewBag.requisitionid = application.JobRequisitionID;
                ViewBag.applicationid = application.JobApplicationID;
                return View(application);
            }
            catch(Exception ex)
            {
                ViewBag.Message = "Sorry , Unable to retrieve Details for Job Application";
                return View("Error");   
            }
            
        }
        [ChildActionOnly]
        public ActionResult _GetRequisition(int? id)
        {
            var requisition = new JobRequisition();
            using (var db = new TalentContext())
            {
                requisition = db.JobRequisitions.Find(id);
            }
            return PartialView(requisition);
        }
        [ChildActionOnly]
        public ActionResult _GetApplication(int? id)
        {
            var application = new JobApplication();
            var db = new TalentContext();
                application = db.JobApplications.Find(id);
            return PartialView(application);
        }
        [HttpPost]
        [Route("Applicant/manage_application/{id:int}")]
        public ActionResult ManageApplication(JobApplication application)
        {
            return View();
        }
        [Route("Applicant/Profile")]
        public ActionResult ManageProfile()
        {
            SetUserSessionID();
            if (TempData["userid"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            var user = getUserAndSkill();
            using (var db = new TalentContext())
            {
                ViewBag.Industries = db.Industries.ToList();
                ViewBag.Skills = db.Skills.ToList();
                //var list = new List<CheckModel>();
                //foreach (var item in (List<Skill>)ViewBag.Skills)
                //{
                //    if (user.Skills.Contains(item))
                //    {
                //        list.Add(new CheckModel { Id = item.ID, Name = item.Name, Checked = true});
                //    }
                //    else
                //    {
                //        list.Add(new CheckModel { Id = item.ID, Name = item.Name, Checked = false });
                //    }  
                //}

                //ViewBag.SelectedSkills = list;
                ViewBag.userid = user.ID;
            }
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Applicant/Profile")]
        public async Task<ActionResult> ManageProfile(JobSeeker applicant, List<CheckModel> checks)
        {
            //var selectedskills = checks.Where(x=> x.Checked == true)
            //                             .Select(o=> new Skill { ID=o.Id,Name=o.Name });
            List<Skill> selectedskills = checks.Where(x => x.Checked == true)
                                        .Select(o => new Skill { ID = o.Id, Name = o.Name }).ToList();
            applicant.Skills =selectedskills;
            // List<Skill> r = (List<Skill>)selectedskills;
            //var selectedskills = from x in checks
            //                     where x.Checked == true
            //                     select new Skill { ID = x.Id, Name = x.Name };
            //List<Skill> r = (List<Skill>)selectedskills;
            // checks.Where(x=>x.Checked==true).Select(new {}

            if (!ModelState.IsValid || Equals(applicant.ID,null) || Equals(applicant.FirstName, null))
            {
                ViewBag.userid = applicant.ID;
                ModelState.AddModelError("", "Please Complete all Mandatory Fields");
                ModelState.AddModelError("IndustryID", "Select an Industry of Specialization");
                ModelState.AddModelError("IndustryID", "Select an Industry of Specialization");
                ViewBag.Industries = new TalentContext().Industries.ToList();
                return View(applicant);
            }
            //applicant.ID = (int)TempData["userid"];
            //ViewBag.userid = applicant.ID;
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

        public ActionResult _GetSkills(int id)
        {
            var user = new JobSeeker();
            var sk = new List<CheckModel>();
            using (var db = new TalentContext())
            {
                user = db.Applicants.Include("Skills").Where(s => s.ID == id).FirstOrDefault();
                ViewBag.Skills = db.Skills.ToList();
                var list = new List<CheckModel>();
                foreach (var item in (List<Skill>)ViewBag.Skills)
                {
                    if (user.Skills.Contains(item))
                    {
                        list.Add(new CheckModel { Id = item.ID, Name = item.Name, Checked = true });
                    }
                    else
                    {
                        list.Add(new CheckModel { Id = item.ID, Name = item.Name, Checked = false });
                    }
                }
                ViewBag.SelectedSkills = list;
                sk = list;
            }
            return PartialView(sk);
        }
        [Route("Applicant/Profile/Skills")]
        public ActionResult Skills()
        {
            SetUserSessionID();
            var user = getUserAndSkill();
            ViewBag.userid =user.ID;
           var sk = new List<CheckModel>();
            //using (var db = new TalentContext())
            //{
            //    ViewBag.Industries = db.Industries.ToList();
            //    ViewBag.Skills = db.Skills.ToList();
            //    var list = new List<CheckModel>();
            //    foreach (var item in (List<Skill>)ViewBag.Skills)
            //    {
            //        if (user.Skills.Contains(item))
            //        {
            //            list.Add(new CheckModel { Id = item.ID, Name = item.Name, Checked = true });
            //        }
            //        else
            //        {
            //            list.Add(new CheckModel { Id = item.ID, Name = item.Name, Checked = false });
            //        }
            //    }
            //    ViewBag.SelectedSkills = list;
            //    sk = list;
            //}

            return PartialView(sk);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Applicant/Profile/Skills")]
        public ActionResult Skills(List<CheckModel> checks, ICollection<string> ret, ICollection<Product> products)
        {
            SetUserSessionID();
            var user = getUserAndSkill();
            using (var db = new TalentContext())
            {
                ViewBag.Industries = db.Industries.ToList();
                ViewBag.Skills = db.Skills.ToList();
                var list = new List<CheckModel>();
                foreach (var item in (List<Skill>)ViewBag.Skills)
                {
                    if (user.Skills.Contains(item))
                    {
                        list.Add(new CheckModel { Id = item.ID, Name = item.Name, Checked = true });
                    }
                    else
                    {
                        list.Add(new CheckModel { Id = item.ID, Name = item.Name, Checked = false });
                    }
                }
                ViewBag.SelectedSkills = list;
            }
            return PartialView(user);
        }
        [Route("Applicant/Profile/Employment")]
        public ActionResult Employment()
        {
            SetUserSessionID();
            var id = (int)TempData["userid"];
            ViewBag.Userid= id;
            var employment = new WorkExperience {StartingDate=DateTime.Now,
                EndingDate =DateTime.Now,JobSeekerID=id };
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
            SetUserSessionID();
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
        [ChildActionOnly]
        public ActionResult _GetOverview()
        {
            SetUserSessionID();
            var applicantid = (int)TempData["userid"];
            var applicant = new JobSeeker();
            using (var db = new TalentContext())
            {
               ViewBag.Links = "Hide";
                applicant = db.Applicants.Include("Schools").Include("Certifications").Include("WorkExperiences").FirstOrDefault(x => x.ID == applicantid);
            }
            return PartialView(applicant);
        }
        public ActionResult _GetApplicantOverview(int applicantid)
        {
            var applicant = new JobSeeker();
            using (var db = new TalentContext())
            {
                ViewBag.Links = "Hide";
                applicant = db.Applicants.Include("Schools").Include("Certifications").Include("WorkExperiences").FirstOrDefault(x => x.ID == applicantid);
            }
            return PartialView("_GetOverview",applicant);
        }
        [Route("Applicant/JobApplications")]
        public ActionResult JobApplications()
        {
            SetUserSessionID();
            var id = (int)TempData["userid"];
            ViewBag.Userid = id;
            var applications = new List<JobApplication>();
            applications = new TalentContext().JobApplications.Where(application=>application.JobSeekerID== id).ToList() ;
            return PartialView(applications);
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
        private JobSeeker getUserAndSkill()
        {
            var currentuser = new JobSeeker();
            using (var db = new TalentContext())
            {
                var id = User.Identity.GetUserId();
                var userid = db.Applicants.Where(s => s.UserId == id).FirstOrDefault().ID;
                currentuser = db.Applicants.Include("Skills").Where(s => s.ID == userid).FirstOrDefault();
            }
            return currentuser;
        }
        [AllowAnonymous]
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

        public ActionResult _GetInterview(int requisitionid, int applicationid)
        {
            var interview = new Interview();
            using (var db = new TalentContext())
            {
                interview = db.Interviews.Where(o => o.JobRequisitionID == requisitionid && o.JobApplicationID == applicationid).FirstOrDefault();
            }
            return PartialView(interview);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Applicant/_SubmitInterview")]
        public ActionResult _SubmitInterview(Interview data)
        {
            using (var db = new TalentContext())
            {
                var interview = db.Interviews.Find(data.InterviewID);
                interview.ScheduledDate = data.ScheduledDate;
                db.SaveChanges();
            }
            return RedirectToAction("manage_application/"+data.JobApplicationID);
        }
        public ActionResult _GetJobOffer()
        {
            SetUserSessionID();
            var applicantid = (int)TempData["userid"];
            var applicant = new JobSeeker();
            using (var db = new TalentContext())
            {
                ViewBag.Links = "Hide";
                //applicant = db.Applicants.Include("Schools").Include("Certifications").Include("WorkExperiences").FirstOrDefault(x => x.ID == applicantid);
            }
            return PartialView(applicant);
        }

    }
}