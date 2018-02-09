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
        // GET: Application
        public ActionResult Index()
        {
            return View("Error");
        }

        [HttpPost]
        /// [ChildActionOnly]
        [Route("Application/UpdatePage")]
        public JsonResult UpdatePage()
        {
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
            //careful here
            ViewBag.pageid = requisitionid;
            ViewBag.applicationid = applicationid;
            return PartialView();
        }
        [Route("Application/InterviewPage/{requisitionid:int}")]
        public ActionResult InterviewPage(int requisitionid, int applicationid)
        {
            
            ViewBag.applicationid = applicationid;
            ViewBag.requisitionid = requisitionid;
            return PartialView();
        }
        [Route("Application/OfferJobPage/{requisitionid:int}")]
        public ActionResult OfferJobPage(int requisitionid, int applicationid)
        {
            ViewBag.applicationid = applicationid;
            ViewBag.requisitionid = requisitionid;
            return PartialView();
        }
        public ActionResult _GetCandidateAvailabilityForm(int requisitionid, int applicationid)
        {
            var interview = new Interview();
            
            interview.JobApplicationID = applicationid;
            interview.JobRequisitionID = requisitionid;
            using(var db = new TalentContext())
            {
                interview.OfficePositionID = db.JobApplications.Find(requisitionid).JobApplicationID;
            }
            ViewBag.applicationid = applicationid;
            ViewBag.requisitionid = requisitionid;
            return PartialView(interview);
        }
        public JsonResult _SubmitCandidateAvailabilityForm(Interview interview)
        {
            bool action = false;
            if (ModelState.IsValid)
            {
                
                using (var db=new TalentContext())
                 {
                    var InterviewExistingCheck=db.Interviews.Where(o=> o.JobRequisitionID == interview.JobRequisitionID && o.JobApplicationID == interview.JobApplicationID).FirstOrDefault();
                    if (InterviewExistingCheck == null)
                    {
                        db.Interviews.Add(interview);   
                    }
                    else
                    {
                        interview.InterviewID = InterviewExistingCheck.InterviewID;
                        db.Entry(interview).State = System.Data.Entity.EntityState.Modified;
                    }
                    db.SaveChanges();
                    action = true;
                } 
            }
            return Json(action, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _GetChooseInterviewTeamForm(int requisitionid, int applicationid)
        {
            var interview = new Interview();
            var interviewdetail = new InterviewDetail();
            var allEmployees = new List<Employee>();
            var allApplicants = new List<JobSeeker>();
            using (var db = new TalentContext())
            {
                interview = db.Interviews.Where(o=> o.JobRequisitionID == requisitionid && o.JobApplicationID== applicationid).FirstOrDefault();
                interviewdetail.Interview = interview;
                //for the Interview Team we add
                //--One person from the HR Department
                //--The Head of Department for the position
                allEmployees = db.Employees.ToList();
                allApplicants = db.Applicants.ToList();
            }
            ViewBag.applicationid = applicationid;
            ViewBag.requisitionid = requisitionid;
            ViewBag.allEmployees = allEmployees;
            ViewBag.allApplicants = allApplicants;

            return PartialView(interviewdetail);
        }
        public JsonResult _SubmitChooseInterviewTeamForm(InterviewDetail interviewdetail)
        {
            bool action = false;
            if (ModelState.IsValid)
            {
                if (interviewdetail.InterviewDetailID == 0)
                {
                    using (var db = new TalentContext())
                    {
                        db.InterviewDetails.Add(interviewdetail);
                        db.SaveChanges();
                    }
                    action = true;
                }
            }
            return Json(action, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _GetInterviewFeedbackForm(int requisitionid, int applicationid)
        {
            var interview = new Interview();
            var interviewdetail = new InterviewDetail();
            //interview.JobApplicationID = applicationid;
            //interview.JobRequisitionID = requisitionid;
            using (var db = new TalentContext())
            {
                interview = db.Interviews.Where(o=>o.JobApplicationID == applicationid && o.JobRequisitionID == requisitionid).FirstOrDefault();
            }
            ViewBag.applicationid = applicationid;
            ViewBag.requisitionid = requisitionid;
            return PartialView(interview);
        }
        public JsonResult _SubmitInterviewFeedbackForm(InterviewDetail interviewdetail)
        {
            bool action = false;
            if (ModelState.IsValid)
            {
                if (interviewdetail.InterviewDetailID == 0)
                {
                    using (var db = new TalentContext())
                    {
                        db.InterviewDetails.Add(interviewdetail);
                        db.SaveChanges();
                    }
                    action = true;
                }
            }
            return Json(action, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _GetFinalizeInterviewForm(int requisitionid, int applicationid)
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
            return PartialView(interview);
        }
        public JsonResult _SubmitInterviewSchedulingForm(InterviewDetail interviewdetail)
        {
            bool action = false;
            if (ModelState.IsValid)
            {
                if (interviewdetail.InterviewDetailID == 0)
                {
                    using (var db = new TalentContext())
                    {
                        db.InterviewDetails.Add(interviewdetail);
                        db.SaveChanges();
                    }
                    action = true;
                }
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
            return PartialView(interview);
        }
    }
}