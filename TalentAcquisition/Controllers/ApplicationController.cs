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

namespace TalentAcquisition.Controllers
{
    public class ApplicationController : Controller
    {
        TalentContext db = new TalentContext();
        IEmailMessaging _messaging;
        #region Views
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
            var req = db.JobApplications.Find(applicationid);
            if (req.ApplicationStatus <= ApplicationStatus.JobOffer)
                req.ApplicationStatus = ApplicationStatus.JobOffer;

            db.SaveChanges();
            ViewBag.applicationid = applicationid;
            ViewBag.requisitionid = requisitionid;
            ViewBag.interviewid = requisitionid;
            return PartialView();
        } 
        #endregion
        #region FormsAndPartialViews
        public ActionResult _GetCandidateAvailabilityForm(int requisitionid, int applicationid)
        {
            var interview = new Interview();
            var InterviewExistingCheck = db.Interviews.Where(o => o.JobRequisitionID == requisitionid && o.JobApplicationID == applicationid);
            if (InterviewExistingCheck.Any())
            {
                interview = InterviewExistingCheck.FirstOrDefault();
            }
            else
            {
                interview = new Interview() { JobRequisitionID = requisitionid, JobApplicationID = applicationid, ProposedDate1 = DateTime.Now, ProposedDate2 = DateTime.Now };
                db.Interviews.Add(interview);
            }
            interview.JobApplicationID = applicationid;
            interview.JobRequisitionID = requisitionid;
            interview.OfficePositionID = db.JobApplications.Where(o => o.JobRequisitionID == interview.JobRequisitionID).FirstOrDefault().JobApplicationID;
            db.SaveChanges();
            ViewBag.applicationid = applicationid;
            ViewBag.requisitionid = requisitionid;

            return PartialView(interview);
        }
        public JsonResult _SubmitCandidateAvailabilityForm(Interview interview)
        {
            bool action = false;
            if (ModelState.IsValid)
            {
                using (var db = new TalentContext())
                {
                    var InterviewExistingCheck = db.Interviews.Where(o => o.JobRequisitionID == interview.JobRequisitionID && o.JobApplicationID == interview.JobApplicationID);
                    if (InterviewExistingCheck.Any())
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
            return Json(action, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _GetChooseInterviewTeamForm(int requisitionid, int applicationid)
        {

            var interviewdetail = new InterviewDetail();
            var allEmployees = new List<Employee>();
            using (var db = new TalentContext())
            {
                var interview = new Interview();
                interview = db.Interviews.Where(o => o.JobRequisitionID == requisitionid && o.JobApplicationID == applicationid).FirstOrDefault();
                interviewdetail.InterviewID = interview.InterviewID;
                interviewdetail.Interview = interview;
                //for the Interview Team we add
                //--One person from the HR Department
                //--The Head of Department for the position
                allEmployees = db.Employees.ToList();
            }
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
        public ActionResult _GetFinalizeInterviewForm(int requisitionid, int applicationid)
        {
            var interview = new Interview();
            //var interviewdetail = new InterviewDetail();
            var teamMembers = new List<Employee>();
            using (var db = new TalentContext())
            {
                interview = db.Interviews.Where(o => o.JobRequisitionID == requisitionid && o.JobApplicationID == applicationid).FirstOrDefault();
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
                    db.Entry(interview).State = System.Data.Entity.EntityState.Modified;
                    //db.Interviews.Add(interview);
                    db.SaveChanges();
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
        public ActionResult _GetCandidateEvaluationForm(int interviewid, int employeeid)
        {
            var interviewevaluation = new InterviewEvaluation();
            using (var db = new TalentContext())
            {
                var existinginterviewevaluation = db.InterviewEvaluations.Where(x => x.InterviewID == interviewid && x.EmployeeID == employeeid);
                if (existinginterviewevaluation.Any())
                {
                    interviewevaluation = existinginterviewevaluation.FirstOrDefault();
                }
                else
                {
                    int count = db.InterviewEvaluations.Where(x => x.InterviewID == interviewevaluation.InterviewID).Count();
                    interviewevaluation.EvaluationNo = "TR" + String.Format("{0:D6}", interviewid + count + 6);
                    interviewevaluation.StageID = 1;
                }
            }
            return PartialView(interviewevaluation);
        }
        public ActionResult _SubmitCandidateEvaluationForm(int? interviewid, int? employeeid, InterviewEvaluation interviewevaluation)
        {
            if (ModelState.IsValid)
            {
                using (var db = new TalentContext())
                {
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
                ViewBag.Message = "Successful";
            }
            // return PartialView(interviewevaluation);
            return RedirectToAction("_getcandidateevaluationform", "Application", new { interviewid = interviewid, employeeid = employeeid });
        }
        public ActionResult _GetInterviewEvaluations(int interviewid)
        {
            var interviewevaluations = new List<InterviewEvaluation>();
            using (var db = new TalentContext())
            {
                var allevaluations = db.InterviewEvaluations.Include("Employee").Where(x => x.InterviewID == interviewid).ToList();
                if (allevaluations.Any())
                {
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
        public ActionResult _GetEvaluations(int id)
        {
            var evaluations = new List<Evaluation>();
            evaluations.Add(new Evaluation() { ID = 1, InterviewEvaluationID = 1, EvaluationCode = "D", EvaluationDescription = "Dressing" });
            evaluations.Add(new Evaluation() { ID = 2, InterviewEvaluationID = 1, EvaluationCode = "C", EvaluationDescription = "Composure" });
            using (var db = new TalentContext())
            {

            }
            return PartialView(evaluations);
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