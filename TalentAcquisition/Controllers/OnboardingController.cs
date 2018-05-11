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
using TalentAcquisition.Helper;
using System.Threading.Tasks;
using System.IO;

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
                return RedirectToAction("Template/Customize/"+template.ID, "Onboarding");
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
                var activities = db.CompletedActivities.Where(x => x.OnboardingTemplateID == guide.ID).ToList();
                var guideactivities = OnboardingUtilityHelper.ConvertToGuideActivities(activities,guide.ID);
                db.CompletedActivities.AddRange(guideactivities);
                db.SaveChanges();
            }
            return RedirectToAction("Guide/Customize/" + guide.Name + "/" + guide.ID, "Onboarding");
        }
        [Route("Onboarding/Guide/Customize/{applicant}/{id:int}")]
        public ActionResult EditGuide(int id)
        {
            using (var db = new TalentContext())
             {
              var guide = db.WelcomeGuides.Include("CompletedActivities").Where(x => x.ID == id);
                if(guide.First().WelcomeMessage==null && guide.First().TemplateID != null)
                {
                    guide.First().WelcomeMessage = db.OnboardingTemplates.Find(guide.First().TemplateID).WelcomeMessage;
                   
                }
                int activitycount = guide.First().CompletedActivities.Count();
                if (activitycount <= 0)
                {
                    guide.First().CompletedActivities = OnboardingUtilityHelper.ConvertToGuideActivities(db.OnboardingTemplates.Find(guide.First().TemplateID).CompletedActivities.ToList(), guide.First().ID);
                    db.SaveChanges();
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

        [Authorize]
        [Route("Onboarding/Applicant/{guideurl}")]
        public ActionResult SecuredGuide(string guideurl)
        {
            var guide = new WelcomeGuide();
            using (var db = new TalentContext())
            {
                var _guide = db.WelcomeGuides.Where(x => x.previewurl == guideurl).First();
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _CreateActivity(OnboardActivity activity)
        {
            if (ModelState.IsValid)
            {
                db.OnboardActivities.Add(activity);
                db.SaveChanges();
            }
            return View();
        }
        public ActionResult _CreateActivityViewModel(int id,int templateid)
        {
            var onboardActivity = db.OnboardActivities.Find(id);
            var activitymodel = OnboardingUtilityHelper.ConvertToActivityModel(onboardActivity);
            activitymodel.OnboardingTemplateID = templateid;
            return PartialView(activitymodel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult _CreateActivityViewModel(ActivityViewModel activitymodel)
        {
            if (ModelState.IsValid)
            {
                CompletedActivity activity = OnboardingUtilityHelper.ConvertToCompletedActivity(activitymodel);
                db.CompletedActivities.Add(activity);
                db.SaveChanges();
            }
            return Json(true,JsonRequestBehavior.AllowGet);
        }
        public ActionResult _GetAllActivitiesForTemplate(int id)
        {
            var activities = new SelectedActivityViewModel();
            var activitylist = new List<CompletedActivity>();
            activitylist = db.CompletedActivities.Where(x=>x.OnboardingTemplateID==id).ToList();
            activities.Activities.AddRange(OnboardingUtilityHelper.ConvertToActivityModelList(activitylist));
            return PartialView(activities);
        }
        public JsonResult _DeleteActivity(int id)
        {
            if (ModelState.IsValid)
            {
                CompletedActivity activity = db.CompletedActivities.Find(id);
                db.CompletedActivities.Remove(activity);
                db.SaveChanges();
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _GetAllActivities()
        {
            var activitylist = new List<OnboardActivity>();
            activitylist = db.OnboardActivities.ToList();
            return PartialView(activitylist);
        }
        public ActionResult _GetAllActivitiesForGuide(int id)
        {
            var activities = new SelectedActivityViewModel();
            var activitylist = new List<CompletedActivity>();
            activitylist = db.CompletedActivities.Where(x => x.WelcomeGuideID == id).ToList();
            activities.Activities.AddRange(OnboardingUtilityHelper.ConvertToActivityModelList(activitylist));
            return PartialView(activities);
        }
        public ActionResult _GetAllActivitiesForGuideEditable(int id)
        {
            var activities = new SelectedActivityViewModel();
            var activitylist = new List<CompletedActivity>();
            activitylist = db.CompletedActivities.Where(x => x.WelcomeGuideID == id).ToList();
            activities.Activities.AddRange(OnboardingUtilityHelper.ConvertToActivityModelList(activitylist));
            return PartialView(activities);
        }
        [HttpPost]
        public async Task<JsonResult> _MarkActivityAsCompleted(int id)
       // public JsonResult _MarkActivityAsCompleted(int id)
        {
            bool action = false;
            var activity = db.CompletedActivities.Where(x => x.ID == id);
            if (activity != null)
            {
                //activity.First().HasTaskBeenCompleted = true;
                await db.SaveChangesAsync();
                //db.SaveChanges();
                action = true;
            }
            return Json(action,JsonRequestBehavior.AllowGet);
        }
        public JsonResult _NotifyApplicant(int id)
        {
            var guide = db.WelcomeGuides.Find(id);
            _messaging = new NotifyOnboardingEmail("ayandaoluwatosin@gmail.com", guide.Name, guide.Position,"http://localhost:54105"+"/Onboarding/Applicant/"+guide.previewurl);
            _messaging.SendEmailToApplicant();
            return Json(true,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> UploadFile(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/Uploads/documents"), _FileName);
                    file.SaveAs(_path);
                }
            }
            catch
            {
                
            }
            await db.SaveChangesAsync();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

    }
}
