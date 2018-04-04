using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TalentAcquisition.Core.Domain;
using TalentAcquisition.DataLayer;
using TalentAcquisition.Filters;
using System.Web.UI;

namespace TalentAcquisition.Controllers
{
  
    public class JobController : Controller
    {
        private TalentContext db = new TalentContext();

        // GET: Job
        //[Route("Job")]
        //[Route("Home/Jobs")]
        //public ActionResult Index()
        //{
        //    return View(getJobs());
        //}
        [AllowAnonymous]
        [OutputCache(Duration = 120, VaryByParam = "none")]
        public PartialViewResult _GetRecentJobs()
        {
            return PartialView(getJobs());
        }

        [AllowAnonymous]
        [OutputCache(Duration = 3600, VaryByParam = "none")]
        public PartialViewResult _GetSearchForm()
        {
            ViewBag.Departments=db.Departments.ToList();
            ViewBag.Industries = db.Industries.ToList();
            return PartialView("SearchForm");
        }

        [AllowAnonymous]
        [Route("Job")]
        [Route("Job/Search")]
        public ViewResult Search(string skillOrProfession,string specialization,string department)
        {
            return View(getJobsBySearch(skillOrProfession,specialization,department));
        }
        //Cannot be called Directly
        [ChildActionOnly]
        public PartialViewResult _GetJobPost (JobRequisition job)
            {
            //JobRequisition job = new JobRequisition();
            //using(var db=new TalentContext())
            //    {
            //        int id = job.JobRequisitionID;
            //        //ViewBag.Location = "Lagos";
            //        //ViewBag.Industry = from s in db.Industries
            //        //                   join ka in db.OfficePositions
            //        //                   on s.IndustryId equals ka.IndustryID
            //        //                   join b in db.JobRequisitions on
            //        //                   ka.Title equals b.JobTitle
            //        //                   select s.Name;
            //    }
                return PartialView("JobPost", job);
            }
        private List<JobRequisition> getJobsBySearch(string skillOrProfession, string specialization, string department)
        {
            var jobs = new List<JobRequisition>();
            using (db)
            {
                //var alljobs= from s in db.JobRequisitions
                //             select s;
                var alljobs = db.JobRequisitions.Include("OfficePosition.Department");
                jobs = alljobs.ToList();
                var publishedjobs = alljobs.Where(o => o.Status.Value == JobRequisition.JobRequisitionStatus.Posted);
                var filteredjobs = new List<JobRequisition>();

                if((skillOrProfession == null) && (specialization == null))
                {
                    return jobs;
                }
                if (skillOrProfession != "")
                {
                    filteredjobs = publishedjobs.Where(s => s.JobDescription.Contains(skillOrProfession)).ToList();
                }
                if (department != "-1" && department != null)
                {
                    var deptid = db.Departments.Where(s => s.DepartmentName == department).FirstOrDefault().DepartmentID;
                    var deptjobs = db.JobRequisitions.Where(s => s.OfficePosition.DepartmentID == deptid && s.Status.Value == JobRequisition.JobRequisitionStatus.Posted).ToList();
                    filteredjobs= filteredjobs.Union(deptjobs).ToList();
                }
                if (specialization != "-1" && specialization != null)
                {
                    var industryid=db.Industries.Where(s => s.Name == specialization).FirstOrDefault().IndustryId;
                    filteredjobs = filteredjobs.Union(db.JobRequisitions.Where(s => s.OfficePosition.IndustryID == industryid && s.Status.Value == JobRequisition.JobRequisitionStatus.Posted)).ToList();
                }
                //var requiredjobs = from pj in publishedjobs
                //                   join oj in db.OfficePositions
                //                   on pj.OfficePositionID equals oj.OfficePositionID
                //                   join dp in db.Departments on oj.DepartmentID equals dp.DepartmentID
                //                   join ind in db.Industries on oj.IndustryID equals ind.IndustryId
                //                   where (oj.Title.Contains(skillOrProfession)||
                //                   ind.Name.Contains(specialization) ||
                //                   dp.DepartmentName.Contains(department))
                //                   select pj;
                //jobs = requiredjobs.ToList();
                return filteredjobs.OrderBy(x=>x.PublishedDate).ToList();
            }
        }

        private bool checkalluserfields()
        {
            var userid = User.Identity.GetUserId();
            var applicant = new TalentContext().Applicants.Where(s => s.UserId == userid).FirstOrDefault();
            if (applicant == null)
            {
                return false;
            }
            if (
                !Equals(applicant.Address,null)  &&
                !Equals(applicant.FirstName, null) && !Equals(applicant.LastName, null) &&
                !Equals(applicant.DateOfBirth, null)
                //&& Equals(applicant.Age, null)
                //&& !Equals(applicant.UploadedCVAddress, null) && 
                //!Equals(applicant.UploadedPassportAddress, null) &&
                //!Equals(applicant.ApplicantNumber, null) && (applicant.WorkExperiences.Count>0) &&
                //(applicant.Schools.Count > 0) && (applicant.Certifications.Count > 0)
                )
            {
                return true;
            }
            return false;
        }

        private List<JobRequisition> getJobs()
        {
            var jobs = new List<JobRequisition>();
            using (db)
            {
                // from s in db.Students select s;
                var anyjobs = db.JobRequisitions.Where(o => o.Status.Value 
                == JobRequisition.JobRequisitionStatus.Posted).Take(10).OrderByDescending(o=>o.PublishedDate);
                jobs = anyjobs.ToList();
                    return jobs;
            }
        }

        // GET: Job/Details/5
        
        [Route("Job/{id:int}/{name}", Name = "JobDetails")]
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                JobRequisition jobRequisition = db.JobRequisitions.Find(id);
                if (jobRequisition == null)
                {
                    return HttpNotFound();
                }
                return View(jobRequisition);
            }
            catch
            {
                return View("Error");
            }
            
        }

        [AuthorizeApplicant]
        [Route("Apply/{id:int}", Name = "ApplyLink")]
        public ActionResult Apply(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobRequisition job = db.JobRequisitions.Find(id);
            ViewBag.jobRequisition = job;
            ViewBag.Industries = db.Industries.ToList();

            if (job == null)
            {
                return HttpNotFound();
            }
            var jobseekerguid = User.Identity.GetUserId();
            JobSeeker applicant = db.Applicants.Where(o => o.UserId == jobseekerguid)
                .Include("JobApplications")
                .Include("Schools")
                .Include("WorkExperiences")
                .Include("Certifications").FirstOrDefault();
            return View(applicant);
        }
        [HttpPost]
        [AuthorizeApplicant]
        [Route("Apply/{id:int}")]
        public ActionResult Apply(int id,JobSeeker jobseeker)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobRequisition job = db.JobRequisitions.Find(id);
            ViewBag.Industries = db.Industries.ToList();
            ViewBag.jobRequisition = job;
            if (job == null)
            {
                return HttpNotFound();
            }
            try
            {   
                //get the details of the applicant
                var IsProfileCompleted = checkalluserfields();
                var IsApplicationExisting = checkIfApplicationExists(User.Identity.GetUserId(),id);
                if (IsApplicationExisting)
                {
                    ViewBag.Message = "FoundApplication";
                    return View(jobseeker);
                }
                if (IsProfileCompleted)
                {
                    //run check to see if there is an existing application with the clientid and job requisitionid
                    //Display Message to indicate Success
                    //Create new Job Application
                    
                    var application = new JobApplication();
                    application.ApplicationStatus = ApplicationStatus.Applied;
                    application.JobSeekerID = getuserid();
                    application.RegistrationDate = DateTime.Now;
                    application.JobRequisitionID = id;

                    using(var db=new TalentContext())
                    {
                        db.JobApplications.Add(application);
                        db.SaveChanges();
                    }
                    ViewBag.Message = "True";

                }
                else
                {
                    ViewBag.Message = "False";
                }
                //Obtain List of Fields to be Updated
                //Display Message to indicate Success
                return View(jobseeker);
            }
            catch
            {
                ViewBag.Message = "Please Re-Apply";
                return View(jobseeker);
            }
        }

        private bool checkIfApplicationExists(string userid,int requisitionid)
        {
            var user = db.Applicants.Where(o => o.UserId == userid).FirstOrDefault();
            var application = db.JobApplications.Where(o => o.JobRequisitionID == requisitionid && o.JobSeekerID == user.ID);
            if (application.Any())
                return true;
            return false;
        }
        private int getuserid()
        {
            var userid = User.Identity.GetUserId();
            var applicant = new TalentContext().Applicants.Where(s => s.UserId == userid).FirstOrDefault();
            return applicant.ID;
        }

        // GET: Job/Create

        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Job/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "JobRequisitionID,JobTitle,Status,NoOfPositionsAvailable,JobDescription,HumanResourcePersonnelID,HeadOfDepartmentID,StartDate,ClosingDate,JobUrl")] JobRequisition jobRequisition)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.JobRequisitions.Add(jobRequisition);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(jobRequisition);
        //}

        //// GET: Job/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    JobRequisition jobRequisition = db.JobRequisitions.Find(id);
        //    if (jobRequisition == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(jobRequisition);
        //}

        //// POST: Job/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "JobRequisitionID,JobTitle,Status,NoOfPositionsAvailable,JobDescription,HumanResourcePersonnelID,HeadOfDepartmentID,StartDate,ClosingDate,JobUrl")] JobRequisition jobRequisition)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(jobRequisition).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(jobRequisition);
        //}

        //// GET: Job/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    JobRequisition jobRequisition = db.JobRequisitions.Find(id);
        //    if (jobRequisition == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(jobRequisition);
        //}

        //// POST: Job/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    JobRequisition jobRequisition = db.JobRequisitions.Find(id);
        //    db.JobRequisitions.Remove(jobRequisition);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        [Route("Apply/UpdateEmployment")]
        public JsonResult UpdateEmployment()
        {
            return Json(new {fash="Baba Nla" },JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
