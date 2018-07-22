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
using System.Net;
using Talent.HRM.Services.FileManger;

namespace TalentAcquisition.Controllers
{
    public class OnboardingController : Controller
    {
        #region Fields
        private TalentContext db = new TalentContext();
        private IEmailMessaging _messaging;
        private IFileHelper _helper;
        const string filesavepath = "~/Uploads/Ckeditor";
        // const string baseUrl = @"http://localhost:54105/Uploads/Ckeditor/";
        const string baseUrl = @"/Uploads/Ckeditor/";
        const string scriptTag = "<script type='text/javascript'>window.parent.CKEDITOR.tools.callFunction({0}, '{1}', '{2}')</script>";

        #endregion
        #region Views
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
                return RedirectToAction("Template/Customize/" + template.ID, "Onboarding");
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
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Onboarding/Template/Customize/{id:int}")]
        public ActionResult EditTemplate(int id, OnboardingTemplate template, string htmlMessage)
        {
            string htmlEncoded = WebUtility.HtmlEncode(htmlMessage);
            if (ModelState.IsValid)
            {
                template.DateEdited = DateTime.Now;
                template.WelcomeMessage = htmlEncoded;
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
        public ActionResult CreateGuide(int? applicantid = 0, string name = "", string position = "")
        {
            WelcomeGuide guide = new WelcomeGuide();
            guide.DateCreated = DateTime.UtcNow;
            guide.Name = name;
            guide.Position = position;
            guide.StartDate = DateTime.UtcNow.AddDays(7);
            guide.JobSeekerID = applicantid;
            ViewBag.Templates = db.OnboardingTemplates.ToList();
            ViewBag.Branches = db.Branches.ToList();
            return View(guide);
        }
        [Route("Onboarding/Guide/Create/applicant")]
        [Route("Onboarding/Guide/Create/applicant/{applicantid:int}")]
        [HttpPost]
        public ActionResult CreateGuide(int? applicantid, WelcomeGuide guide)
        {
            ViewBag.Templates = db.OnboardingTemplates.ToList();
            ViewBag.Branches = db.Branches.ToList();
            if (!ModelState.IsValid)
            {
                guide.DateCreated = DateTime.UtcNow;
                guide.StartDate = DateTime.UtcNow.AddDays(7);
                return View(guide);
            }
            using (var db = new TalentContext())
            {
                guide.Status = Status.Review;
                db.WelcomeGuides.Add(guide);
                db.SaveChanges();

                if (guide.TemplateID != null)
                {
                    guide.WelcomeMessage = db.OnboardingTemplates.Find(guide.TemplateID).WelcomeMessage;
                    var activities = db.CompletedActivities.Where(x => x.OnboardingTemplateID == guide.ID).ToList();
                    var guideactivities = OnboardingUtilityHelper.ConvertToGuideActivities(activities, guide.ID);
                    //guide.CompletedActivities = OnboardingUtilityHelper.ConvertToGuideActivities(db.OnboardingTemplates.Find(guide.TemplateID).CompletedActivities.ToList(), guide.ID);
                    guide.previewurl = Guid.NewGuid().ToString();
                    db.CompletedActivities.AddRange(guideactivities);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Guide/Customize/" + guide.Name + "/" + guide.ID, "Onboarding");
        }
        [Route("Onboarding/Guide/Customize/{applicant}/{id:int}")]
        public ActionResult EditGuide(int id)
        {
            using (var db = new TalentContext())
            {
                var guide = db.WelcomeGuides.Include("CompletedActivities").Where(x => x.ID == id);
                if (guide == null)
                {
                    return HttpNotFound();
                }
                return View(guide.First());
            }
        }
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [Route("Onboarding/Guide/Customize/{applicant}/{id:int}")]
        [HttpPost]
        public ActionResult EditGuide(int id, WelcomeGuide guide, int? newStatus)
        {
            if (!ModelState.IsValid)
            {
                return View(guide);
            }
            using (var db = new TalentContext())
            {
                guide.WelcomeMessage = WebUtility.HtmlEncode(guide.WelcomeMessage);
                db.WelcomeGuides.Add(guide);
                if (newStatus != null)
                {
                    switch (newStatus)
                    {
                        case 1:
                            guide.Status = Status.Published;
                            break;
                        case 2:
                            guide.Status = Status.Submitted;
                            break;
                        case 3:
                            guide.Status = Status.Complete;
                            break;
                        case 4:
                            guide.Status = Status.Closed;
                            break;
                    }
                }
                db.Entry(guide).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                if (newStatus == 3)
                {
                    return RedirectToAction("Personnel/Create", "Admin", new { guideid = guide.ID });
                }
            }
            return RedirectToAction("Onboarding", "Admin");
        }

        [Route("Onboarding/progress/{guideurl}")]
        [Route("Onboarding/preview/{guideurl}")]
        public ActionResult PreviewGuide(string guideurl)
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
        #endregion
        #region PartialView
        // [Route("Onboarding/Applicant/{guideurl}")]
        public ActionResult ValidateEmployee(int id)
        {
            var guide = new WelcomeGuide();
            using (var db = new TalentContext())
            {
                var _guide = db.WelcomeGuides.Where(x => x.ID == id).First();
                if (_guide == null)
                {
                    return HttpNotFound();
                }
                guide = _guide;
            }
            var url = Url.Action("Personnel/Create", "Admin");
            return View(url, guide);
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
                foreach (var applicant in applicationsinonboarding)
                {
                    if (!db.WelcomeGuides.Where(x => x.JobSeekerID == applicant.JobSeekerID && x.Position == applicant.JobRequisition.JobTitle).Any())
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
                WelcomeGuides = db.WelcomeGuides.Where(x => x.Status < Status.Closed).ToList();
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
        public ActionResult _CreateActivityViewModel(int id, int templateid)
        {
            var onboardActivity = db.OnboardActivities.Find(id);
            var activitymodel = OnboardingUtilityHelper.ConvertToActivityModel(onboardActivity);
            activitymodel.OnboardingTemplateID = templateid;
            return PartialView(activitymodel);
        }
        public ActionResult _CreateGuideActivityViewModel(int id, int templateid, int guideid)
        {
            var onboardActivity = db.OnboardActivities.Find(id);
            var activitymodel = OnboardingUtilityHelper.ConvertToActivityModel(onboardActivity);
            activitymodel.OnboardingTemplateID = templateid;
            activitymodel.WelcomeGuideID = guideid;
            activitymodel.Body = activitymodel.Title;
            return PartialView("_CreateActivityViewModel", activitymodel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult _CreateActivityViewModel(ActivityViewModel activitymodel)
        {
            string htmlEncoded = WebUtility.HtmlEncode(activitymodel.Body);
            activitymodel.Body = htmlEncoded;
            if (ModelState.IsValid)
            {
                CompletedActivity activity = OnboardingUtilityHelper.ConvertToCompletedActivity(activitymodel);
                activity.OnboardActivityID = activitymodel.OnboardActivityID;
                if (activity.WelcomeGuideID == 0)
                {
                    activity.WelcomeGuideID = null;
                }
                db.CompletedActivities.Add(activity);
                db.SaveChanges();
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region CKEditorUpload
        public ActionResult CKEditorUpload()
        {
            var funcNum = 0;
            int.TryParse(Request["CKEditorFuncNum"], out funcNum);

            if (Request.Files == null || Request.Files.Count < 1)
                return BuildReturnScript(funcNum, null, "No file has been sent");

            string fileName = string.Empty;
            SaveAttatchedFile(filesavepath, Request, ref fileName);
            var url = baseUrl + fileName;
            // return BuildReturnScript(funcNum, url, null);
            Dictionary<string, dynamic> response = new Dictionary<string, dynamic>();
            //        "uploaded": 1,
            //"fileName": "foo.jpg",
            //"url": "/files/foo.jpg"
            response["uploaded"] = (int)1;
            response["fileName"] = fileName;
            response["url"] = HttpUtility.JavaScriptStringEncode(url ?? "");
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        private ContentResult BuildReturnScript(int functionNumber, string url, string errorMessage)
        {
            return Content(
                string.Format(scriptTag, functionNumber, HttpUtility.JavaScriptStringEncode(url ?? ""), HttpUtility.JavaScriptStringEncode(errorMessage ?? "")),
                "text/html"
                );
        }
        private void SaveAttatchedFile(string filepath, HttpRequestBase Request, ref string fileName)
        {
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                if (file != null && file.ContentLength > 0)
                {
                    fileName = Path.GetFileName(file.FileName);
                    _helper = new AzureFileHelper();

                    string targetPath = Server.MapPath(filepath);
                    if (!Directory.Exists(targetPath))
                    {
                        Directory.CreateDirectory(targetPath);
                    }
                    fileName = Guid.NewGuid() + fileName;
                    _helper.UploadSingleFileAsync(file, fileName, targetPath);
                    //string fileSavePath = Path.Combine(targetPath, fileName);
                    //file.SaveAs(fileSavePath);
                }
            }
        }
        [HttpPost]
        public async Task<JsonResult> UploadFile(int id)
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    var activity = db.CompletedActivities.Where(x => x.ID == id);
                    if (activity != null)
                    {
                        //activity.First().HasTaskBeenCompleted = true;
                        // await db.SaveChangesAsync();
                        //db.SaveChanges();
                        // action = true;
                    }
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            string extension = Path.GetExtension(file.FileName);
                            fname = activity.First().Name + activity.First().ID + extension;
                        }

                        // Get the complete folder path and store the file inside it.  
                        fname = Path.Combine(Server.MapPath("~/Uploads/"), fname);
                        file.SaveAs(fname);
                    }
                    // Returns message that successfully uploaded 
                    await db.SaveChangesAsync();
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        } 
        #endregion
        #region PartialJsonView
        public ActionResult _GetAllActivitiesForTemplate(int id)
        {
            var activities = new SelectedActivityViewModel();
            var activitylist = new List<CompletedActivity>();
            activitylist = db.CompletedActivities.Where(x => x.OnboardingTemplateID == id).ToList();
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
        public async Task<JsonResult> PushCompletedOnboardingToFullEmployee(int id)
        {
            bool action = false;
            var guide = db.WelcomeGuides.Where(x => x.ID == id);
            if (guide != null)
            {
                guide.First().Status = Status.Complete;
                await db.SaveChangesAsync();
                //db.SaveChanges();
                action = true;
            }
            return Json(action, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> _MarkActivityAsCompleted(int id)

        {
            bool action = false;
            var activity = db.CompletedActivities.Where(x => x.ID == id);
            if (activity != null)
            {
                activity.First().HasTaskBeenCompleted = true;
                await db.SaveChangesAsync();
                //db.SaveChanges();
                action = true;
            }
            return Json(action, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> _SubmitAsCompletedOnboarding(int id)
        {
            bool action = false;
            var guide = db.WelcomeGuides.Where(x => x.ID == id);
            if (guide != null)
            {
                guide.First().Status = Status.Submitted;
                await db.SaveChangesAsync();
                //db.SaveChanges();
                action = true;
            }
            return Json(action, JsonRequestBehavior.AllowGet);
        }
        public JsonResult _NotifyApplicant(int id)
        {
            var guide = db.WelcomeGuides.Find(id);
            _messaging = new NotifyOnboardingEmail("ayandaoluwatosin@gmail.com", guide.Name, guide.Position, "http://localhost:54105" + "/Onboarding/Applicant/" + guide.previewurl);
            _messaging.SendEmailToApplicant();
            return Json(true, JsonRequestBehavior.AllowGet);
        } 
        #endregion

    }
}
