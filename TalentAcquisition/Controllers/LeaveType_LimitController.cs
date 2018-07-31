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
    public class LeaveType_LimitController : Controller
    {
        private TalentContext db = new TalentContext();

        // GET: LeaveType_Limit
        [Route("LeaveTypes/All")]
        public ActionResult Index()
        {
            return View(db.LeaveType_Limits.ToList());
        }

        // GET: LeaveType_Limit/Details/5
        [Route("LeaveTypes/Details/{id}")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveType_Limit leaveType_Limit = db.LeaveType_Limits.Find(id);
            if (leaveType_Limit == null)
            {
                return HttpNotFound();
            }
            return View(leaveType_Limit);
        }

        // GET: LeaveType_Limit/Create
        [Route("LeaveTypes/Create")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: LeaveType_Limit/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("LeaveTypes/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,LeaveType,Limit,RequiresPlan")] LeaveType_Limit leaveType_Limit)
        {
            if (ModelState.IsValid)
            {
                db.LeaveType_Limits.Add(leaveType_Limit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(leaveType_Limit);
        }

        // GET: LeaveType_Limit/Edit/5
        [Route("LeaveTypes/Edit/{id}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveType_Limit leaveType_Limit = db.LeaveType_Limits.Find(id);
            if (leaveType_Limit == null)
            {
                return HttpNotFound();
            }
            return View(leaveType_Limit);
        }

        // POST: LeaveType_Limit/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("LeaveTypes/Edit/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,LeaveType,Limit,RequiresPlan")] LeaveType_Limit leaveType_Limit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(leaveType_Limit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(leaveType_Limit);
        }

        // GET: LeaveType_Limit/Delete/5
        [Route("LeaveTypes/Delete/{id}")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveType_Limit leaveType_Limit = db.LeaveType_Limits.Find(id);
            if (leaveType_Limit == null)
            {
                return HttpNotFound();
            }
            return View(leaveType_Limit);
        }

        // POST: LeaveType_Limit/Delete/5
        [Route("LeaveTypes/Delete/{id}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LeaveType_Limit leaveType_Limit = db.LeaveType_Limits.Find(id);
            db.LeaveType_Limits.Remove(leaveType_Limit);
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
