using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using TalentAcquisition.BusinessLogic.UpdatedDomain;
using TalentAcquisition.Core.Domain;
using TalentAcquisition.DataLayer;
using Talent.HRM.Services.Interfaces;
using Talent.HRM.Services.Email;
using TalentAcquisition.Models.ViewModel;
using TalentAcquisition.Repositories.Interfaces;
using TalentAcquisition.Repositories;

namespace TalentAcquisition.Controllers
{
    public class ApplicationController : Controller
    {
        #region Fields
        TalentContext db = new TalentContext();
        IEmailMessaging _messaging;
        private OfferLetterViewModel letter;
        private IEmployeeRepository _repo;
        #endregion
        #region Views
        public ApplicationController()
        {
            _repo = new EmployeeRepository(db);
        }
        // GET: Application
        public ActionResult Index()
        {
            return View("Error");
        }
        [HttpPost]
        /// [ChildActionOnly]
        [Route("Application/UpdatePage/{applicationid:int}")]
        public JsonResult UpdatePage(int applicationid)
        {
            ViewBag.applicationid = applicationid;
            var req = db.JobApplications.Find(applicationid);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [ChildActionOnly]
        [Route("Application/AppliedPage/{requisitionid:int}")]
        public ActionResult AppliedPage(int requisitionid, int applicationid)
        {
            ViewBag.applicantid = new TalentContext().JobApplications.Where(o => o.JobRequisitionID == requisitionid && o.JobApplicationID == applicationid).FirstOrDefault().JobSeekerID;
            return PartialView();
        }
        //[ChildActionOnly]
        [Route("Application/ScreenedPage/{requisitionid:int}")]
        public ActionResult ScreenedPage(int requisitionid, int applicationid)
        {
            var req = db.JobApplications.Find(applicationid);
            if (req.ApplicationStatus <= ApplicationStatus.Screened)
                req.ApplicationStatus = ApplicationStatus.Screened;
            db.SaveChanges();
            //careful here
            ViewBag.pageid = requisitionid;
            ViewBag.applicationid = applicationid;
            return PartialView();
        }
        [Route("Application/InterviewPage/{requisitionid:int}")]
        public ActionResult InterviewPage(int requisitionid, int applicationid)
        {
            var req = db.JobApplications.Find(applicationid);
            if (req.ApplicationStatus <= ApplicationStatus.Interview)
                req.ApplicationStatus = ApplicationStatus.Interview;
            db.SaveChanges();

            var interview = new Interview();
            var Interviews = db.Interviews.Where(o => o.JobRequisitionID == requisitionid 
                    && o.JobApplicationID == applicationid && o.HasInterviewBeenCompleted==false);

            ViewBag.interviewid = 0;
            ViewBag.Status = false;
            ViewBag.Count = false;
            if (Interviews.Count() > 0)
            {
              
                if (Interviews.Count() == 1)
                {
                   interview = Interviews.FirstOrDefault();
                }
                else
                {
                    interview = Interviews.OrderByDescending(o => o.InterviewID).First();
                }
                ViewBag.interviewid = interview.InterviewID;
                ViewBag.Count = Equals(interview.InterviewDetails.TeamMember1ID, null);
               // ViewBag.Status = interview.HasInterviewBeenCompleted;
                if (!String.IsNullOrEmpty(interview.Venue))
                {
                    ViewBag.Status = true;
                }
            }

            ViewBag.applicationid = applicationid;
            ViewBag.requisitionid = requisitionid; 
            return PartialView();
        }
        [Route("Application/EvaluationPage/{requisitionid:int}")]
        public ActionResult EvaluationPage(int requisitionid, int applicationid)
        {
            var req = db.JobApplications.Find(applicationid);
            var userid = User.Identity.GetUserId();
            var Interview = db.Interviews
                .Where(o => o.JobRequisitionID == requisitionid && o.JobApplicationID == applicationid);
            var applicant = db.Employees.Where(s => s.UserId == userid).First();
            if (req.ApplicationStatus <= ApplicationStatus.Interview)
                req.ApplicationStatus = ApplicationStatus.Interview;
            db.SaveChanges();
            ViewBag.applicationid = applicationid;
            ViewBag.requisitionid = requisitionid;
            ViewBag.interviewid = Interview.First().InterviewID;
            ViewBag.jobseekerid = applicant.ID;
            return PartialView();
        }
        [Route("Application/OfferJobPage/{requisitionid:int}")]
        public ActionResult OfferJobPage(int requisitionid, int applicationid)
        {
            var model = new OfferJobViewModel();
            var req = db.JobApplications.Find(applicationid);
            
            if (req.ApplicationStatus <= ApplicationStatus.JobOffer)
                req.ApplicationStatus = ApplicationStatus.JobOffer;
            if (req.ApplicationStatus == ApplicationStatus.JobOfferAccepted)
                model.OfferAccepted = true;
            db.SaveChanges();
            var Interview = db.Interviews
                .Where(o => o.JobRequisitionID == requisitionid && o.JobApplicationID == applicationid);
           // if(req.ApplicationStatus==ApplicationStatus.)
            ViewBag.applicationid = applicationid;
            ViewBag.requisitionid = requisitionid;
            ViewBag.interviewid = Interview.First().InterviewID;
            return PartialView(model);
        } 
        #endregion
        #region FormsAndPartialViews
        public ActionResult _GetCandidateAvailabilityForm(int requisitionid, int applicationid,int interviewid)
        {
            var interview = new Interview();
            var LastInterview = new Interview();

            var Interviews = db.Interviews.Where(o => o.JobRequisitionID == requisitionid && o.JobApplicationID == applicationid);
            if (Interviews.Count() > 0)
            {
                if (Interviews.Count() == 1)
                {
                    LastInterview = Interviews.FirstOrDefault();
                }
                else
                {
                    LastInterview = Interviews.OrderByDescending(o=>o.InterviewID).First();
                }
            }
            if (LastInterview.InterviewEvaluations.Count() > 0)
            {
                interview = new Interview() { JobRequisitionID = requisitionid, JobApplicationID = applicationid, ProposedDate1 = DateTime.Now.AddDays(2), ProposedDate2 = DateTime.Now.AddDays(2) };
                db.Interviews.Add(interview);
            }
            else
            {
                if (interviewid == 0)
                {
                    interview = new Interview() { JobRequisitionID = requisitionid, JobApplicationID = applicationid, ProposedDate1 = DateTime.Now.AddDays(2), ProposedDate2 = DateTime.Now.AddDays(2) };
                    db.Interviews.Add(interview);
                }
                else
                {
                    interview = LastInterview;
                }
            }
            
            interview.JobApplicationID = applicationid;
            interview.JobRequisitionID = requisitionid;
            //interview.OfficePositionID = db.JobApplications.Where(o => o.JobRequisitionID == interview.JobRequisitionID).FirstOrDefault().JobApplicationID;
            interview.OfficePositionID = db.JobRequisitions.Where(o => o.JobRequisitionID == interview.JobRequisitionID).FirstOrDefault().OfficePositionID;
           // db.SaveChanges();
            ViewBag.applicationid = applicationid;
            ViewBag.requisitionid = requisitionid;
            //ViewBag.interviewcount = InterviewExistingCheck.Count();

            return PartialView(interview);
        }
        public JsonResult _SubmitCandidateAvailabilityForm(Interview interview)
        {
            bool action = false;
            if (ModelState.IsValid)
            {
                using (var db = new TalentContext())
                {
                    //var InterviewExistingCheck = db.Interviews.Where(o => o.JobRequisitionID == interview.JobRequisitionID && o.JobApplicationID == interview.JobApplicationID);
                    if (interview.InterviewID!=0)
                    {
                        db.Interviews.Add(interview);
                        db.Entry(interview).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Interviews.Add(interview);
                        db.SaveChanges();
                    }
                    var applicant = db.JobApplications.Include("JobSeeker").Where(x => x.JobApplicationID == interview.JobApplicationID).First().JobSeeker;
                    ApplicationDbContext context = new ApplicationDbContext();
                    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    var applicantemail = UserManager.FindById(applicant.UserId).Email;
                    SendEmailToApplicant();
                    action = true;
                }
            }
            var response = new Dictionary<string, dynamic>();
            response.Add("action", (bool)action);
            response.Add("interviewid", (int)interview.InterviewID);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _GetChooseInterviewTeamForm(int requisitionid, int applicationid,int interviewid)
        {

            var interviewdetail = new InterviewDetail();
            var allEmployees = _repo.GetAll().ToList();
            interviewdetail.InterviewID = interviewid;
            //using (var db = new TalentContext())
            //{
            //   // var interview = new Interview();
            //  //  interview = db.Interviews.Where(o => o.JobRequisitionID == requisitionid && o.JobApplicationID == applicationid).FirstOrDefault();
            //   // interview = db.Interviews.Where(o => o.InterviewID==interviewid).FirstOrDefault();
            //    interviewdetail.InterviewID = interviewid;
            //    //interviewdetail.Interview = interview;
            //    //for the Interview Team we add
            //    //--One person from the HR Department
            //    //--The Head of Department for the position
            //    allEmployees = db.Employees.ToList();
            //}
            ViewBag.applicationid = applicationid;
            ViewBag.requisitionid = requisitionid;
            ViewBag.allEmployees = allEmployees;

            return PartialView(interviewdetail);
        }
        public JsonResult _SubmitChooseInterviewTeamForm(InterviewDetail interviewdetail)
        {
            bool action = false;
            //if (ModelState.IsValid)
            //{
            if (interviewdetail.InterviewDetailID == 0)
            {
                using (var db = new TalentContext())
                {
                    //var interview = new Interview();
                    interviewdetail.Interview = db.Interviews.Find(interviewdetail.InterviewID);
                    db.InterviewDetails.Add(interviewdetail);
                    db.SaveChanges();
                }
                action = true;
            }

            //}
            return Json(action, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _GetInterviewFeedbackForm(int requisitionid, int applicationid)
        {
            var interview = new Interview();
            using (var db = new TalentContext())
            {
                interview = db.Interviews.Where(o => o.JobApplicationID == applicationid && o.JobRequisitionID == requisitionid).FirstOrDefault();
            }
            ViewBag.applicationid = applicationid;
            ViewBag.requisitionid = requisitionid;
            ViewBag.interviewid = interview.InterviewID;
            return PartialView();
        }
        public JsonResult _SubmitInterviewFeedbackForm(int interviewid, string Strength, string Weakness, bool Recommendation)
        {
            bool action = false;
            var interviewdetail = new InterviewDetail();
            var user = db.Employees.Where(o => o.UserId == User.Identity.GetUserId()).FirstOrDefault();
            interviewdetail = db.InterviewDetails.Where(o => o.InterviewID == interviewid).FirstOrDefault();
            //if (ModelState.IsValid)
            //{
            if (interviewdetail.InterviewDetailID == 0)
            {
                using (var db = new TalentContext())
                {
                    db.InterviewDetails.Add(interviewdetail);
                    db.SaveChanges();
                }
                action = true;
            }
            //}
            return Json(action, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _GetFinalizeInterviewForm(int requisitionid, int applicationid, int interviewid)
        {
            var interview = new Interview();
            //var interviewdetail = new InterviewDetail();
            var teamMembers = new List<Employee>();
            using (var db = new TalentContext())
            {
                interview = db.Interviews.Where(o=>o.InterviewID==interviewid).FirstOrDefault();
                var interviewdetail = db.InterviewDetails.Where(o => o.Interview.InterviewID == interview.InterviewID).FirstOrDefault();
                if (interviewdetail != null)
                {
                    var team = db.Employees.Where(o => o.ID == interviewdetail.TeamMember1ID ||
                    o.ID == interviewdetail.TeamMember2ID || o.ID == interviewdetail.TeamMember3ID
                    || o.ID == interviewdetail.TeamMember4ID);
                    teamMembers = teamMembers.Union(team).ToList();
                }
            }
            ViewBag.applicationid = applicationid;
            ViewBag.requisitionid = requisitionid;
            ViewBag.teamMembers = teamMembers;
            //interview.TeamMembers= teamMembers;
            return PartialView(interview);
        }
        public JsonResult _SubmitInterviewSchedulingForm(Interview data)
        {
            bool action = false;
            if (ModelState.IsValid)
            {
                using (var db = new TalentContext())
                {
                    var interview = db.Interviews.Find(data.InterviewID);
                    interview.SchedulingFinalNote = data.SchedulingFinalNote;
                    interview.ScheduledDate = data.ScheduledDate;
                    interview.Venue = data.Venue;
                    interview.Time = data.Time;
                    db.Entry(interview).State = System.Data.Entity.EntityState.Modified;
                    //db.Interviews.Add(interview);
                    db.SaveChanges();
                    var interviewdetail = db.InterviewDetails.Where(o => o.Interview.InterviewID == interview.InterviewID).FirstOrDefault();
                    var interviewevaluations = new List<InterviewEvaluation>();
                    int[] employeeids = new int[4] { interviewdetail.TeamMember1ID, interviewdetail.TeamMember2ID, interviewdetail.TeamMember3ID, interviewdetail.TeamMember4ID };
                    int count = 5;
                    foreach (var item in employeeids)
                    {
                        var tempeval = new InterviewEvaluation
                        {
                         EvaluationNo = "TR" + String.Format("{0:D6}", data.InterviewID + count),
                         InterviewID = data.InterviewID,
                         EmployeeID = item
                        };
                        interviewevaluations.Add(tempeval);
                        count++;
                    }
                    db.InterviewEvaluations.AddRange(interviewevaluations);
                    db.SaveChanges();
                    //message interview Team Members
                }
                action = true;
            }
            return Json(action, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _GetTeamFeedback(int requisitionid, int applicationid)
        {
            var interview = new Interview();
            var interviewdetail = new InterviewDetail();
            var allApplicants = new List<JobSeeker>();
            var teamMembers = new List<Employee>();
            using (var db = new TalentContext())
            {
                interview = db.Interviews.Where(o => o.JobRequisitionID == requisitionid && o.JobApplicationID == applicationid).FirstOrDefault();
                //interviewdetail = db.InterviewDetails.Where(o => o.Interview.InterviewID == interview.InterviewID).FirstOrDefault();
                interviewdetail.Interview = interview;
            }

            ViewBag.applicationid = applicationid;
            ViewBag.requisitionid = requisitionid;
            ViewBag.teamMembers = teamMembers;
            return PartialView(interviewdetail);
        }
        [Route("Application/Interview/{interviewid}/Evaluation/Employee/{employeeid}")]
        public ActionResult _GetCandidateEvaluationForm(int interviewid, int employeeid,string status)
        {
            var interviewevaluation = new InterviewEvaluation();
            var dictionary = new Dictionary<string, int>();
            using (var db = new TalentContext())
            {
                var categories = db.EvaluationCategories.Where(x => x.InterviewID == interviewid).ToList();
                var interview = db.Interviews.Find(interviewid);
                var officeid = interview.OfficePositionID;
                ViewBag.OfficeID = officeid;
                var evaluationss = db.ApplicantEvaluationMetrics.Where(x => x.OfficePositionID == officeid);
                ViewBag.JobDetails = evaluationss.FirstOrDefault().OfficePosition.Title;
                foreach (var item in evaluationss)
                {
                    dictionary[item.EvaluationCode] = item.MaximumScore;
                }
                var existinginterviewevaluation = db.InterviewEvaluations.Where(x => x.InterviewID == interviewid && x.EmployeeID == employeeid);
                if (existinginterviewevaluation.Any())
                {
                    interviewevaluation = existinginterviewevaluation.FirstOrDefault();
                }
                else
                {
                    int count = db.InterviewEvaluations.Where(x => x.InterviewID == interviewevaluation.InterviewID).Count();
                    interviewevaluation.EvaluationNo = "TR" + String.Format("{0:D6}", interviewid + count + 6);
                    interviewevaluation.EmployeeID = employeeid;
                    interviewevaluation.InterviewID = interviewid;
                    interviewevaluation.StageID = 1;
                }
            }
            ViewBag.Status = false;
            if (status == "success")
            {
                ViewBag.Message = "Successfully Saved Evaluation";
                ViewBag.Status = true;
            }
            ViewBag.Dictionary = dictionary;
            return PartialView(interviewevaluation);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _SubmitCandidateEvaluationForm(InterviewEvaluation interviewevaluation, List<Evaluation> evaluations)
        {
            if (ModelState.IsValid)
            {
                using (var db = new TalentContext())
                {

                    foreach (var evaluation in evaluations)
                    {
                        if (evaluation.ID == 0)
                        {
                            db.Evaluations.Add(evaluation);
                        }
                        else
                        {
                            db.Evaluations.Add(evaluation);
                            db.Entry(evaluation).State = System.Data.Entity.EntityState.Modified;
                        }
                        db.SaveChanges();
                    }

                    // interviewevaluation.EvaluationNo = "TR" + String.Format("{0:D6}", interviewid + db.InterviewEvaluations.Where(x => x.InterviewID == interviewevaluation.InterviewID).Count());
                    if (interviewevaluation.ID == 0)
                    {
                        db.InterviewEvaluations.Add(interviewevaluation);
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Entry(interviewevaluation).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                
               // return PartialView("_getcandidateevaluationform",interviewevaluation);
                return RedirectToAction("_getcandidateevaluationform", "Application", new { interviewid = interviewevaluation.InterviewID, employeeid = interviewevaluation.EmployeeID,status="successful" });
                //              return RedirectToAction("Dashboard", "Admin");
            }
            // return PartialView(interviewevaluation);
            return RedirectToAction("_getcandidateevaluationform", "Application", new { interviewid = interviewevaluation.InterviewID, employeeid = interviewevaluation.EmployeeID });
        }
        public ActionResult _GetInterviewEvaluations(int interviewid)
        {
            var interviewevaluations = new List<InterviewEvaluation>();
            using (var db = new TalentContext())
            {
                var allevaluations = db.InterviewEvaluations.Include("Employee").Where(x => x.InterviewID == interviewid).ToList();
                var evalscores= db.Evaluations.Include("InterviewEvaluation").Where(x => x.InterviewEvaluation.InterviewID == interviewid);
                if (allevaluations.Any())
                {
                    foreach (var score in allevaluations)
                    {
                        var ev = evalscores.Where(x => x.InterviewEvaluationID == score.ID);
                        if(ev.Count()>0)
                        {
                            score.Score1 = (decimal)ev.Average(x => x.Score1);
                        }
                        
                    }
                    interviewevaluations.AddRange(allevaluations);
                    
                }
                //interviewevaluations.Add(new InterviewEvaluation()
                //{
                //    ID = 1,
                //    EmployeeID = 10,
                //    InterviewID = interviewid,
                //    EvaluationNo = "TR" + String.Format("{0:D6}", interviewid + allevaluations.Count),
                //    RecommendForHire = false
                //});
            }
            return PartialView(interviewevaluations);
        }
        public ActionResult _GetEvaluations(int id,int stageid,int officeid, Dictionary<string, int> maxscores)
        {
            ViewBag.stageid = stageid;
            ViewBag.maxscores = maxscores;
            var applicantmetrics = db.ApplicantEvaluationMetrics.Where(x => x.OfficePositionID == officeid);
           // ViewBag.ApplicantMetrics = applicantmetrics;
            var evaluations = new List<Evaluation>();
            using (var db = new TalentContext())
            {
               var evaluationss = db.Evaluations.Where(x => x.InterviewEvaluationID == id).ToList();
                if (evaluationss.Count() != applicantmetrics.Count())
                {
                    foreach (var metric in applicantmetrics)
                    {
                        evaluations.Add(new Evaluation
                        {
                            EvaluationCode = metric.EvaluationCode,
                            EvaluationDescription = metric.EvaluationDescription,
                            InterviewEvaluationID = id
                        });
                    }
                    db.Evaluations.AddRange(evaluations);
                    db.SaveChanges();
                }
                else
                {
                    evaluations = evaluationss;
                }
                
            }
            

            return PartialView(evaluations);
        }
        public ActionResult _GetApplicantInterviewView(int id, int officeid)
        {
            var model = new ApplicantEvalFormViewModel();

            var applicationid = db.Interviews.Where(x => x.InterviewID == id).First().JobApplicationID;
            var applicantid = db.JobApplications.Where(x => x.JobApplicationID == applicationid).First().JobSeekerID;
            var applicant = db.Applicants.Include("Skills").Where(x => x.ID == applicantid).First();
            model.Name = applicant.FullName;
            model.Age = applicant.Age;
            model.HighestQualification = applicant.HighestQualification.ToString();
            model.skills = applicant.Skills.Select(x => x.Name).ToList();
            model.PositionAppliedFor = db.JobRequisitions.Where(x => x.OfficePositionID == officeid).First().JobTitle;
            return PartialView(model);
        }
        public ActionResult _NewEvaluation(int aid)
        {
            var evaluation = new Evaluation();
            //ViewBag.Categories = new SelectList(db.Interviews, "EvaluationCode", "EvaluationCode");
            var categories = db.EvaluationCategories.Where(x=>x.InterviewID == aid).ToList();
            var dictionary = new Dictionary<string,string>();
            foreach(var item in categories)
            {
                dictionary[item.EvaluationCode] = item.EvaluationDescription;
            }
            ViewBag.Categories = categories;
            ViewBag.Dictionary = dictionary;
            return PartialView("EvaluationView", evaluation);
        }
        public JsonResult _AddorUpdateEvaluation(Evaluation evaluation)
        {
            var action = false;
            if (ModelState.IsValid)
            {
                using (var db = new TalentContext())
                {
                    if (evaluation.ID == 0)
                    {
                        db.Evaluations.Add(evaluation);
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Evaluations.Add(evaluation);
                        db.Entry(evaluation).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    action = true;
                }
            }     
            return Json(action, JsonRequestBehavior.AllowGet);
        }
        public JsonResult _DeleteEvaluation(int id)
        {
            var action = false;
            //var evaluation = new Evaluation();
            using (var db = new TalentContext())
            {
                var evaluation = db.Evaluations.Find(id);
                if(evaluation != null)
                {
                    db.Evaluations.Remove(evaluation);
                    db.SaveChanges();
                }
                action = true;
            }
            return  Json(action,JsonRequestBehavior.AllowGet);
        }
        public ActionResult _NewEvaluationCategory(int id,int interviewid)
        {
            var evaluation = new List<EvaluationCategory>();
            ViewBag.ID = id;
            for (int i = 0; i <= id; i++)
            {
                evaluation.Add(new EvaluationCategory() { InterviewID=interviewid});
            }
            return PartialView(evaluation);
        }
        public ActionResult _GetEvaluationCategories(int interviewid)
        {
            var evaluation = new List<EvaluationCategory>();
            evaluation = db.EvaluationCategories.Where(x => x.InterviewID == interviewid).ToList();
            return PartialView(evaluation);
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult _SubmitCategoriesForm(IEnumerable<EvaluationCategory> submittedcategories)
        {
            bool action = false;
            if (submittedcategories !=null && submittedcategories.Count() >= 0)
            {
                var categories = submittedcategories.ToList();
                foreach (EvaluationCategory category in categories)
                {
                    if (ModelState.IsValid)
                    {
                        if (category.ID == 0)
                        {
                            db.EvaluationCategories.Add(category);
                        }
                        else
                        {
                            db.Entry(category).State = System.Data.Entity.EntityState.Modified;
                        }
                        db.SaveChanges();
                    }
                    action = true;
                }
            }
            
            return Json(action, JsonRequestBehavior.AllowGet);
        }
        public JsonResult _AddorUpdateEvaluationCategory(Evaluation evaluation)
        {
            var action = false;
            if (ModelState.IsValid)
            {
                using (var db = new TalentContext())
                {
                    if (evaluation.ID == 0)
                    {
                        db.Evaluations.Add(evaluation);
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Evaluations.Add(evaluation);
                        db.Entry(evaluation).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    action = true;
                }
            }
            return Json(action, JsonRequestBehavior.AllowGet);
        }
        public JsonResult _DeleteEvaluationCategory(int id)
        {
            var action = false;
            //var evaluation = new Evaluation();
            using (var db = new TalentContext())
            {
                var evaluation = db.Evaluations.Find(id);
                if (evaluation != null)
                {
                    db.Evaluations.Remove(evaluation);
                    db.SaveChanges();
                }
                action = true;
            }
            return Json(action, JsonRequestBehavior.AllowGet);
        }
        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult _SubmitOfferMessage(int applicationid, int requisitionid,string finalmessage, OfferLetterViewModel offerdetails)
        {
            bool action = false;
            try
            {
                var interview = db.Interviews.Where(x => x.JobApplicationID == applicationid);
                interview.First().JobOfferMessage = finalmessage;
                db.SaveChanges();
                var applicant = db.JobApplications.Include("JobSeeker").Where(x => x.JobApplicationID == applicationid).First().JobSeeker;
                string applicantemail, jobtitle;
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    applicantemail = UserManager.FindById(applicant.UserId).Email;
                }
                jobtitle = db.JobRequisitions.Find(requisitionid).JobTitle;               
               // _messaging = new SendJobOfferEmail(applicantemail, applicant.FullName, jobtitle);
                _messaging = new SendJobOfferEmail("ayandaoluwatosin@gmail.com", applicant.FullName, jobtitle,finalmessage);
                _messaging.SendEmailToApplicant();
                action = true;
            }
            catch
            {

            }
            return Json(action, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _ConfirmApplicantOnboarding(int requisitionid, int applicationid)
        {
            using (var db = new TalentContext())
            {
                var application = db.JobApplications.Where(x => x.JobApplicationID == applicationid).First();
                application.ApplicationStatus = ApplicationStatus.Onboarding;
                db.SaveChanges();
            }
            return RedirectToAction("onboarding", "Admin", new { requisitionid = requisitionid, applicationid = applicationid });
        }
        public ActionResult _GetAllInterviewsForApplication(int requisitionid, int applicationid)
        {
            var req = db.JobApplications.Find(applicationid);
            var userid = User.Identity.GetUserId();
            var Interviews = db.Interviews
                .Where(o => o.JobRequisitionID == requisitionid && o.JobApplicationID == applicationid).ToList();
            ViewBag.applicationid = applicationid;
            ViewBag.requisitionid = requisitionid;
            ViewBag.interviewid = Interviews.First().InterviewID;
            return PartialView(Interviews);
        }
        [HttpPost]
        public ActionResult _EmployeeOfferLetter(OfferLetterViewModel offerdetails)
        {
            letter=offerdetails;
            string message = _getletterhtml(offerdetails).ToString();
            return View(offerdetails);
        }
        //[ChildActionOnly]
        public JsonResult _MarkInterviewAsComplete(int id)
        {
            var action = false;
            var interview = db.Interviews.Find(id);
            if (interview != null)
            {
                interview.HasInterviewBeenCompleted = true;
                db.Entry(interview).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                action = true;
            }
             return Json(action,JsonRequestBehavior.AllowGet);
        }
        public JsonResult _AcceptApplicationJobOffer(int id)
        {
            var action = false;
            var req = db.JobApplications.Find(id);
            if (req != null)
            {
                req.ApplicationStatus = ApplicationStatus.JobOfferAccepted;
                db.Entry(req).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                action = true;
            }
            return Json(action, JsonRequestBehavior.AllowGet);
        }
        public JsonResult _getletterhtml(OfferLetterViewModel offerdetails)
        {
            var letter = PartialView("_EmployeeOfferLetter", offerdetails);
            return Json(letter,JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Helper Methods
        private void SendEmailToApplicant(string applicantemail="ayandaoluwatosin@gmail.com",string applicantname="",string emailtype="")
        {
            _messaging = new ConfirmInterviewEmail(applicantemail, applicantname);
            _messaging.SendEmailToApplicant();
        } 
        #endregion
    }
}