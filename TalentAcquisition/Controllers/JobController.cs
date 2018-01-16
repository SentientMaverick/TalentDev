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
        public PartialViewResult _GetRecentJobs()
        {
            return PartialView(getJobs());
        }
        [AllowAnonymous]
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
                var alljobs= from s in db.JobRequisitions select s;
                var publishedjobs = alljobs.Where(o => o.Status.Value
                == JobRequisition.JobRequisitionStatus.Posted);
                var filteredjobs = new List<JobRequisition> { };
                if (skillOrProfession != "")
                {
                    filteredjobs = publishedjobs.Where(s => s.JobDescription.Contains(skillOrProfession)).ToList();
                }
                if (department != "")
                {
                    filteredjobs.Union(publishedjobs.Where(s => s.OfficePosition.Department.DepartmentName.ToString() == department).ToList());

                }
                if (specialization == "")
                {
                    filteredjobs.Union(publishedjobs.Where(s => s.OfficePosition.Industry.Name.ToString() == skillOrProfession).ToList());
                }
                var requiredjobs = from pj in publishedjobs
                                   join oj in db.OfficePositions
                                   on pj.OfficePositionID equals oj.OfficePositionID
                                   join dp in db.Departments on oj.DepartmentID equals dp.DepartmentID
                                   join ind in db.Industries on oj.IndustryID equals ind.IndustryId
                                   where (oj.Title.Contains(skillOrProfession)||
                                   ind.Name.Contains(specialization) ||
                                   dp.DepartmentName.Contains(department))
                                   select pj;
                //jobs = requiredjobs.ToList();
                jobs = filteredjobs;
                return jobs;
            }
        }

        private bool checkalluserfields()
        {
            return false;
        }

        private List<JobRequisition> getJobs()
        {
            var jobs = new List<JobRequisition>();
            using (db)
            {
                // from s in db.Students select s;
                var anyjobs = db.JobRequisitions.Where(o => o.Status.Value 
                == JobRequisition.JobRequisitionStatus.Posted).Take(10);
                jobs = anyjobs.ToList();
                    return jobs;
            }
        }

        // GET: Job/Details/5
        [Route("Job/{id:int}/{name}", Name = "JobDetails")]
        public ActionResult Details(int? id)
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

        [Route("Apply/{id:int}", Name = "ApplyLink")]
        public ActionResult Apply(int id)
        {
            try
            {
                if (id == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                JobRequisition job = db.JobRequisitions.Find(id);
                if (job == null)
                {
                    return HttpNotFound();
                }
                var application = new JobApplication();
                //get the details of the applicant
                var IsProfileCompleted = checkalluserfields();
                if(IsProfileCompleted)
                {
                    //Display Message to indicate Success
                    try
                    {

                    }
                    catch
                    {

                    }
                    //Create new Job Application
                }
                //Obtain List of Fields to be Updated
                //Display Message to indicate Success
                return View();
            }
            catch
            {
                return View("Error");
            }
        }
        // GET: Job/Create

        public ActionResult Create()
        {
            return View();
        }

        // POST: Job/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JobRequisitionID,JobTitle,Status,NoOfPositionsAvailable,JobDescription,HumanResourcePersonnelID,HeadOfDepartmentID,StartDate,ClosingDate,JobUrl")] JobRequisition jobRequisition)
        {
            if (ModelState.IsValid)
            {
                db.JobRequisitions.Add(jobRequisition);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jobRequisition);
        }

        // GET: Job/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Job/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JobRequisitionID,JobTitle,Status,NoOfPositionsAvailable,JobDescription,HumanResourcePersonnelID,HeadOfDepartmentID,StartDate,ClosingDate,JobUrl")] JobRequisition jobRequisition)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobRequisition).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jobRequisition);
        }

        // GET: Job/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Job/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobRequisition jobRequisition = db.JobRequisitions.Find(id);
            db.JobRequisitions.Remove(jobRequisition);
            db.SaveChanges();
            return RedirectToAction("Index");
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
