using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalentAcquisition.Core.Domain;
using TalentAcquisition.DataLayer;

namespace TalentAcquisition.Controllers
{
    public class ApplicationController : Controller
    {
        TalentContext db = new TalentContext();
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
            ViewBag.applicantid = new TalentContext().JobApplications.Where(o=> o.JobRequisitionID == requisitionid && o.JobApplicationID == applicationid).FirstOrDefault().JobSeekerID;
            return PartialView();
        }
        //[ChildActionOnly]
        [Route("Application/ScreenedPage/{requisitionid:int}")]
        public ActionResult ScreenedPage(int requisitionid,int applicationid)
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
        [Route("Application/OfferJobPage/{requisitionid:int}")]
        public ActionResult OfferJobPage(int requisitionid, int applicationid)
        {
            var req = db.JobApplications.Find(applicationid);
            if (req.ApplicationStatus <= ApplicationStatus.JobOffer)
                req.ApplicationStatus = ApplicationStatus.JobOffer;

            db.SaveChanges();
            ViewBag.applicationid = applicationid;
            ViewBag.requisitionid = requisitionid;
            return PartialView();
        }
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
                interview = new Interview() { JobRequisitionID = requisitionid, JobApplicationID = applicationid };
                db.Interviews.Add(interview);
            }
            interview.JobApplicationID = applicationid;
            interview.JobRequisitionID = requisitionid;
            interview.OfficePositionID = db.JobApplications.Where(o => o.JobRequisitionID == interview.JobRequisitionID).FirstOrDefault().JobApplicationID;

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
                    var InterviewExistingCheck=db.Interviews.Where(o=> o.JobRequisitionID == interview.JobRequisitionID && o.JobApplicationID == interview.JobApplicationID);
                    if (InterviewExistingCheck.Any())
                    {
                        interview.InterviewID = InterviewExistingCheck.FirstOrDefault().InterviewID;
                        //InterviewExistingCheck.FirstOrDefault().ProposedDate1 = interview.ProposedDate1;
                        //InterviewExistingCheck.FirstOrDefault().ProposedDate1 = interview.ProposedDate1;
                        db.Entry(interview).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Interviews.Add(interview);
                        db.SaveChanges();
                    }
                    
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
                interview = db.Interviews.Where(o=>o.JobApplicationID == applicationid && o.JobRequisitionID == requisitionid).FirstOrDefault();
            }
            ViewBag.applicationid = applicationid;
            ViewBag.requisitionid = requisitionid;
            ViewBag.interviewid = interview.InterviewID;
            return PartialView();
        }
        public JsonResult _SubmitInterviewFeedbackForm(int interviewid,string Strength,
            string Weakness, bool Recommendation)
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
                teamMembers = db.Employees.Where(o => o.ID == interviewdetail.TeamMember1ID ||
                o.ID == interviewdetail.TeamMember2ID || o.ID == interviewdetail.TeamMember3ID
                || o.ID == interviewdetail.TeamMember4ID).ToList();
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
    }
}