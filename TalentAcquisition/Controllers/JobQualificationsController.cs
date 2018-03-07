using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TalentAcquisition.BusinessLogic.UpdatedDomain;
using TalentAcquisition.DataLayer;

namespace TalentAcquisition.Controllers
{
    public class JobQualificationsController : Controller
    {
        private TalentContext db = new TalentContext();

        // GET: JobQualifications
        public ActionResult Index()
        {
            return View(db.JobQualifications.ToList());
        }

        // GET: JobQualifications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobQualification jobQualification = db.JobQualifications.Find(id);
            if (jobQualification == null)
            {
                return HttpNotFound();
            }
            return View(jobQualification);
        }

        // GET: JobQualifications/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: JobQualifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,QualificationType,QualificationCode,Description")] JobQualification jobQualification)
        {
            if (ModelState.IsValid)
            {
                db.JobQualifications.Add(jobQualification);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jobQualification);
        }

        // GET: JobQualifications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobQualification jobQualification = db.JobQualifications.Find(id);
            if (jobQualification == null)
            {
                return HttpNotFound();
            }
            return View(jobQualification);
        }

        // POST: JobQualifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,QualificationType,QualificationCode,Description")] JobQualification jobQualification)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobQualification).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jobQualification);
        }

        // GET: JobQualifications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobQualification jobQualification = db.JobQualifications.Find(id);
            if (jobQualification == null)
            {
                return HttpNotFound();
            }
            return View(jobQualification);
        }

        // POST: JobQualifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobQualification jobQualification = db.JobQualifications.Find(id);
            db.JobQualifications.Remove(jobQualification);
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
