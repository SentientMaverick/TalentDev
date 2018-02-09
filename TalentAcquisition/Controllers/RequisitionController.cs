using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TalentAcquisition.Core.Domain;
using TalentAcquisition.DataLayer;

namespace TalentAcquisition.Controllers
{
    public class RequisitionController : Controller
    {
        // GET: Requisition
        [Route("Admin/Requisitions")]
        public ActionResult Index()
        {
            return View();
        }
        [Route("Admin/Requisition/Create")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Route("Admin/Requisition/Create")]
        public ActionResult Create(JobRequisition requisition)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Couldn't Create Requisition";
                return View(requisition);
            }
            try
            {
               requisition.Status = JobRequisition.JobRequisitionStatus.Created;
                requisition.PublishedDate = DateTime.Now;
                using (var db=new TalentContext())
                {
                    db.JobRequisitions.Add(requisition);
                    db.SaveChanges();
                }
                ViewBag.Message = "Successfully Created Requisition";
                return View();
            }
            catch 
            {
                return View("Error");
            }
        }
        [Route("Admin/Requisition/{id:int}/{details}",Name ="RequisitionLink")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TalentContext db = new TalentContext();
            JobRequisition jobRequisition = db.JobRequisitions.Find(id);
            if (jobRequisition == null)
            {
                return HttpNotFound();
            }
            ViewBag.RequisitionID = jobRequisition.JobRequisitionID;
            return View(jobRequisition);
        }
        [Route("Admin/matchskill/{id:int}/{details}/", Name = "MatchSkill")]
        public ActionResult MatchSkill(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                TalentContext db = new TalentContext();
                JobRequisition jobRequisition = db.JobRequisitions.Find(id);
                if (jobRequisition == null)
                {
                    return HttpNotFound();
                }
                var jobSeekers = db.Applicants.ToList();
                return View(jobSeekers);
            }
            catch(Exception ex)
            {
                return View("Error");
            }
        }
        [Route("Admin/publish/{id:int}/{details}/", Name = "PublishLink")]
        public ActionResult Publish(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TalentContext db = new TalentContext();
            JobRequisition jobRequisition = db.JobRequisitions.Find(id);
            if (jobRequisition == null)
            {
                return HttpNotFound();
            }
            ViewBag.RequisitionID = jobRequisition.JobRequisitionID;
            return View(jobRequisition);
        }

        [Route("Admin/publish/{id:int}/{details}")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Publish(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            TalentContext db = new TalentContext();
            JobRequisition jobRequisition = db.JobRequisitions.Find(id);
            if (jobRequisition == null)
            {
                return HttpNotFound();
            }
            jobRequisition.Status = JobRequisition.JobRequisitionStatus.Posted;
            db.Entry(jobRequisition).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            ViewBag.Message = "Job Successfully Published";
            return RedirectToAction("Requisitions","Admin");
        }
        [Route("Admin/Edit/{id:int}/{details}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TalentContext db = new TalentContext();
            JobRequisition jobRequisition = db.JobRequisitions.Find(id);
            if (jobRequisition == null)
            {
                return HttpNotFound();
            }
            ViewBag.RequisitionID = jobRequisition.JobRequisitionID;
                ViewBag.Departments = db.Departments.ToList();
            ViewBag.Industries = db.Departments.ToList();
            return View(jobRequisition);
        }
        [Route("Admin/Edit/{id:int}/{details}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id,JobRequisition requisition)
        {
            ViewBag.Message = "Changes Were Not Saved. Pls Try Again";
            if (!ModelState.IsValid)
            {
                return View(requisition);
            }
            try
            {
                TalentContext db = new TalentContext();
                JobRequisition jobRequisition = db.JobRequisitions.Find(id);

                jobRequisition.PublishedDate = DateTime.Now;
                jobRequisition.JobResponsibilities = requisition.JobResponsibilities;
                jobRequisition.EducationalRequirements = requisition.EducationalRequirements;
                jobRequisition.AgeLimit = requisition.AgeLimit;
                jobRequisition.JobDescription = jobRequisition.JobDescription;
                jobRequisition.Location = requisition.Location;
                jobRequisition.ClosingDate = requisition.ClosingDate;
                jobRequisition.StartDate = requisition.StartDate;
                jobRequisition.YearsOfExperience = requisition.YearsOfExperience;
                jobRequisition.NoOfPositionsAvailable = requisition.NoOfPositionsAvailable;

              db.Entry(jobRequisition).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();
                ViewBag.Message = "Changes Were succesfully Saved";
                return View(jobRequisition);
            }
            catch
            {
                return View("Error");
            }

        }
        [Route("Admin/reject/{id:int}/{details}")]
        public ActionResult Reject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TalentContext db = new TalentContext();
            JobRequisition jobRequisition = db.JobRequisitions.Find(id);
            if (jobRequisition == null)
            {
                return HttpNotFound();
            }
            ViewBag.RequisitionID = jobRequisition.JobRequisitionID;
            return View(jobRequisition);
        }
        [HttpPost]
        [Route("Admin/reject/{id:int}/{details}")]
        public ActionResult Reject(int? id,JobRequisition requisition)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TalentContext db = new TalentContext();
            JobRequisition jobRequisition = db.JobRequisitions.Find(id);
            if (jobRequisition == null)
            {
                return HttpNotFound();
            }
            ViewBag.RequisitionID = jobRequisition.JobRequisitionID;
            return View(jobRequisition);
        }
        [Route("Admin/close/{id:int}/{details}/", Name = "Close")]
        public ActionResult Close(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TalentContext db = new TalentContext();
            JobRequisition jobRequisition = db.JobRequisitions.Find(id);
            if (jobRequisition == null)
            {
                return HttpNotFound();
            }
            ViewBag.RequisitionID = jobRequisition.JobRequisitionID;
            return View(jobRequisition);
        }
        [HttpPost]
        [Route("Admin/close/{id:int}/{details}/")]
        public ActionResult Close(int Jobid)
        {
            try
            {
                using (var db = new TalentContext())
                {
                    var job = db.JobApplications.Find(Jobid);
                    db.JobApplications.Remove(job);
                    db.SaveChanges();
                }
                //return View();
                return RedirectToAction("Requisitions","Admin");
            }
            catch
            {
                return View("Error");
            }

        }

        [ChildActionOnly]
        public ActionResult _GetNewApplications(int id)
        {
            var activeapplications = new List<JobApplication>();
            //int id = (int)ViewBag.RequisitionID;
            var db = new TalentContext();
            //using (var db = new TalentContext())
            //{
                activeapplications = db.JobApplications.Where(job => job.JobRequisitionID == id && job.ApplicationStatus== ApplicationStatus.Applied).ToList();
            //}
            return PartialView(activeapplications);
        }
        [ChildActionOnly]
        public ActionResult _GetClosedApplications(int id)
        {
            var activeapplications = new List<JobApplication>();
            var db = new TalentContext();
            activeapplications = db.JobApplications.Where(job => job.JobRequisitionID == id).
                    Where(job => job.ApplicationStatus == ApplicationStatus.Canceled).ToList();
            return PartialView("_GetNewApplications",activeapplications);
        }
        [ChildActionOnly]
        public ActionResult _GetScreenedApplications(int id)
        {
            var activeapplications = new List<JobApplication>();
            var db = new TalentContext();
            activeapplications = db.JobApplications.Where(job => job.JobRequisitionID == id).
                    Where(job => (job.ApplicationStatus == ApplicationStatus.Screened)
                    || (job.ApplicationStatus == ApplicationStatus.Interview)
                    || (job.ApplicationStatus == ApplicationStatus.JobOffer)).ToList();
            return PartialView("_GetNewApplications",activeapplications);
        }
        [ChildActionOnly]
        [Route("Admin/Requisitions/_GetActiveRequisitions")]
        public ActionResult _GetActiveRequisitions()
        {
            var activerequisitions = new List<JobRequisition>();
            using (var db = new TalentContext())
            {
                activerequisitions = db.JobRequisitions.Where(job =>job.Status == JobRequisition.JobRequisitionStatus.Posted).ToList();
            }
            return PartialView("_GetIncomingRequisitions",activerequisitions);
        }
        [ChildActionOnly]
        public ActionResult _GetIncomingRequisitions()
        {
            var activerequisitions = new List<JobRequisition>();
            using (var db = new TalentContext())
            {
                activerequisitions = db.JobRequisitions.Where(job => job.Status == JobRequisition.JobRequisitionStatus.Created).ToList();
            }
            return PartialView(activerequisitions);
        }
        //[ChildActionOnly]
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
        public ActionResult _GetDetailsFromRole()
        {
            using (var db=new TalentContext())
            {
                ViewBag.Departments = db.Departments.ToList();
                ViewBag.Positions = Enumerable.Empty<SelectListItem>();
            }
            return PartialView();
        }
        [Route("Requisition/RolesInDepartment")]
        public JsonResult RolesInDepartment(int departmentid)
        {
            var db = new TalentContext();
            var positions = db.OfficePositions.Where(O => O.DepartmentID == departmentid)
                     .Select(c => new { Value = c.OfficePositionID, Text = c.Title }).ToList();
            var positionslist = new SelectList(positions, "Value", "Text", "Select Any Item");
            ViewBag.Positions = positionslist;
            return Json(positions, "application/json", JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetInformationForRole(int officeroleid)
        {
            var db = new TalentContext();
            OfficePosition details = db.OfficePositions.Find(officeroleid);
            return Json(new {Title=details.Title,Responsibilities=details.Reqirements }, "application/json",Encoding.UTF8, JsonRequestBehavior.AllowGet);
        }

    }
}