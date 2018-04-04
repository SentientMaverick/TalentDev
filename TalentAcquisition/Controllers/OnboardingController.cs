using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalentAcquisition.BusinessLogic.UpdatedDomain;
using TalentAcquisition.DataLayer;
using TalentAcquisition.Models.ViewModel;
using Talent.HRM.Services.Email;
using Talent.HRM.Services.Interfaces;

namespace TalentAcquisition.Controllers
{
    public class OnboardingController : Controller
    {
        private TalentContext db = new TalentContext();
        private IEmailMessaging _messaging;
        // GET: Onboarding
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Checklist()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Logout()
        {
            return View();
        }
        [Route("Onboarding/Template/Create")]
        public ActionResult CreateTemplate()
        {
            OnboardingTemplate template = new OnboardingTemplate();
            template.DateCreated = DateTime.Now;
            template.DateEdited = DateTime.Now;
            return View(template);
        }
        [HttpPost]
        [Route("Onboarding/Template/Create")]
        public ActionResult CreateTemplate(OnboardingTemplate template)
        {
            if (ModelState.IsValid)
            {
                template.DateCreated = DateTime.Now;
                template.DateEdited = DateTime.Now;
                db.OnboardingTemplates.Add(template);
                db.SaveChanges();
                return RedirectToAction("Onboarding", "Admin");
            }
            return View(template);
        }
        [Route("Onboarding/Template/Customize/{id:int}")]
        public ActionResult EditTemplate(int id)
        {
            OnboardingTemplate template = new OnboardingTemplate();
            template = db.OnboardingTemplates.Find(id);
            return View(template);
        }
        [HttpPost]
        [Route("Onboarding/Template/Customize/{id:int}")]
        public ActionResult EditTemplate(int id,OnboardingTemplate template)
        {
            if (ModelState.IsValid)
            {
                template.DateEdited = DateTime.Now;
                db.OnboardingTemplates.Add(template);
                db.Entry(template).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Onboarding", "Admin");
            }
            return View(template);
        }
        //[Route("Onboarding/Guide/Create/applicant/{applicantid:int}")]
        [Route("Onboarding/Guide/Create/applicant")]
        [Route("Onboarding/Guide/Create/applicant/{applicantid:int}")]
        public ActionResult CreateGuide(int? applicantid=0)
        {
            WelcomeGuide guide = new WelcomeGuide();
            guide.DateCreated = DateTime.Now;
            guide.StartDate = DateTime.Now.AddDays(7);
            guide.JobSeekerID = applicantid;
            ViewBag.Templates = db.OnboardingTemplates.ToList();
            ViewBag.Branches = db.Branches.ToList();
            return View(guide);
        }
        [Route("Onboarding/Guide/Create/applicant")]
        [Route("Onboarding/Guide/Create/applicant/{applicantid:int}")]
        [HttpPost]
        public ActionResult CreateGuide(int? applicantid,WelcomeGuide guide)
        {
            ViewBag.Templates = db.OnboardingTemplates.ToList();
            ViewBag.Branches = db.Branches.ToList();
            if (!ModelState.IsValid)
            {
                guide.DateCreated = DateTime.Now;
                guide.StartDate = DateTime.Now.AddDays(7);
                return View(guide);
            }
            using (var db = new TalentContext())
            {
                guide.Status = Status.Review;
                db.WelcomeGuides.Add(guide);
                db.SaveChanges();
            }
            return View();
        }
        [Route("Onboarding/Guide/Customize/{applicant}/{id:int}")]
        public ActionResult EditGuide(int id)
        {
            using (var db = new TalentContext())
             {
              var guide = db.WelcomeGuides.Include("OnboardActivities").Where(x => x.ID == id);
                if(guide.First().WelcomeMessage==null && guide.First().TemplateID != null)
                {
                    guide.First().WelcomeMessage = db.OnboardingTemplates.Find(guide.First().TemplateID).WelcomeMessage;
                }
                if (guide.First().previewurl == null)
                {
                    guide.First().previewurl = Guid.NewGuid().ToString();
                    db.SaveChanges();
                }
                if (guide == null)
                {
                    return HttpNotFound();
                }
                return View(guide.First());
            }
        }
        [Route("Onboarding/Guide/Customize/{applicant}/{id:int}")]
        [HttpPost]
        public ActionResult EditGuide(int id,WelcomeGuide guide)
        {
            if (!ModelState.IsValid)
            {
                return View(guide);
            }
            using (var db=new TalentContext())
            {
                db.WelcomeGuides.Add(guide);
                db.Entry(guide).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Onboarding","Admin");
        }
        [Route("Onboarding/preview/{guideurl}")]
        [Route("Onboarding/Applicant/{guideurl}")]
        public ActionResult PreviewGuide(string guideurl)
        {
            var guide = new WelcomeGuide();
            using (var db = new TalentContext())
            {
              var  _guide = db.WelcomeGuides.Where(x => x.previewurl == guideurl).First();
                if (_guide == null)
                {
                    return HttpNotFound();
                }
                guide = _guide;
            }
            return View(guide);
        }
        public ActionResult _GetAllTemplates()
        {
            var templates = new List<OnboardingTemplate>();
            using (var db = new TalentContext())
            {
                templates = db.OnboardingTemplates.Take(5).ToList();
            }
            return PartialView(templates);
        }
        public ActionResult _GetAllncomingOnboardings()
        {
            var WelcomeGuides = new List<WelcomeGuide>();
            var JobApplications = new List<Core.Domain.JobApplication>();
            using (var db = new TalentContext())
            {
              var applicationsinonboarding = db.JobApplications.Include("JobSeeker").Include("JobRequisition").Where(x => x.ApplicationStatus == Core.Domain.ApplicationStatus.Onboarding).ToList();
                foreach ( var applicant in applicationsinonboarding)
                {
                    if (!db.WelcomeGuides.Where(x=>x.JobSeekerID == applicant.JobSeekerID).Any())
                    {
                        JobApplications.Add(applicant);
                    }     
                }
            }
            return PartialView(JobApplications);
        }
        public ActionResult _GetAllActiveOnboardings()
        {
            var WelcomeGuides = new List<WelcomeGuide>();
            using (var db = new TalentContext())
            {
                WelcomeGuides = db.WelcomeGuides.Where(x => x.Status <= Status.Complete).ToList();
            }
            return PartialView(WelcomeGuides);
        }
        public ActionResult _CreateActivity()
        {
            var activity = new OnboardActivity();
            return View();
        }
        public ActionResult _GetAllActivities(int id)
        {
            var activitylist = new List<OnboardActivity>();
            return PartialView(activitylist);
        }
        public JsonResult _NotifyApplicant(int id)
        {
            var guide = db.WelcomeGuides.Find(id);
            _messaging = new NotifyOnboardingEmail("ayandaoluwatosin@gmail.com", guide.Name, guide.Position,Url.Action("Applicant","Onboarding",new {guideurl=guide.previewurl }));
            _messaging.SendEmailToApplicant();
            return Json(true,JsonRequestBehavior.AllowGet);
        }
    }
}
