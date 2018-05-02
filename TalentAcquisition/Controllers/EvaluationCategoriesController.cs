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
    public class EvaluationCategoriesController : Controller
    {
        private TalentContext db = new TalentContext();

        // GET: EvaluationCategories
        public ActionResult Index()
        {
            var evaluationCategories = db.EvaluationCategories.Include(e => e.Interview);
            return View(evaluationCategories.ToList());
        }

        // GET: EvaluationCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EvaluationCategory evaluationCategory = db.EvaluationCategories.Find(id);
            if (evaluationCategory == null)
            {
                return HttpNotFound();
            }
            return View(evaluationCategory);
        }

        // GET: EvaluationCategories/Create
        public ActionResult Create()
        {
            ViewBag.InterviewID = new SelectList(db.Interviews, "InterviewID", "JobApplicationID");
            return View();
        }

        // POST: EvaluationCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,InterviewID,EvaluationCode,EvaluationDescription,Deleted")] EvaluationCategory evaluationCategory)
        {
            if (ModelState.IsValid)
            {
                db.EvaluationCategories.Add(evaluationCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InterviewID = new SelectList(db.Interviews, "InterviewID", "SchedulingFinalNote", evaluationCategory.InterviewID);
            return View(evaluationCategory);
        }

        // GET: EvaluationCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EvaluationCategory evaluationCategory = db.EvaluationCategories.Find(id);
            if (evaluationCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.InterviewID = new SelectList(db.Interviews, "InterviewID", "SchedulingFinalNote", evaluationCategory.InterviewID);
            return View(evaluationCategory);
        }

        // POST: EvaluationCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,InterviewID,EvaluationCode,EvaluationDescription,Deleted")] EvaluationCategory evaluationCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(evaluationCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InterviewID = new SelectList(db.Interviews, "InterviewID", "SchedulingFinalNote", evaluationCategory.InterviewID);
            return View(evaluationCategory);
        }

        // GET: EvaluationCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EvaluationCategory evaluationCategory = db.EvaluationCategories.Find(id);
            if (evaluationCategory == null)
            {
                return HttpNotFound();
            }
            return View(evaluationCategory);
        }

        // POST: EvaluationCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EvaluationCategory evaluationCategory = db.EvaluationCategories.Find(id);
            db.EvaluationCategories.Remove(evaluationCategory);
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
