using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TalentAcquisition.BusinessLogic.UpdatedDomain;
using TalentAcquisition.DataLayer;

namespace TalentAcquisition.Controllers
{
    public class ExitInterviewsController : Controller
    {
        private TalentContext db = new TalentContext();

        // GET: ExitInterviews
        [Route("Exit/Records/All")]
        public async Task<ActionResult> Index()
        {
            return View(await db.ExitInterviews.ToListAsync());
        }

        // GET: ExitInterviews/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExitInterview exitInterview = await db.ExitInterviews.FindAsync(id);
            if (exitInterview == null)
            {
                return HttpNotFound();
            }
            return View(exitInterview);
        }

        // GET: ExitInterviews/Create
        [Route("Exit/Records/Create")]
        public ActionResult Create()
        {
            var model = new ExitInterview();
            var no = db.ExitInterviews.Count();
            model.No = "TR" + String.Format("{0:D6}", no + 1);
            ViewBag.Employees = db.Employees.Select(p => new { Id = p.ID, Name = p.FirstName + " " + p.LastName, Number = p.EmployeeNumber, Department = p.OfficePosition.Department.DepartmentName });

            return View(model);
        }

        // POST: ExitInterviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "No,EmployeeNo,EmployeeName,Reason,OtherReasons,LeavingOn,ReEmploy,InterviewDate,InterviewerNo,InterviewerName,Comment,Status,ApproveProcess,ProcessCompleted")] ExitInterview exitInterview)
        {
            if (ModelState.IsValid)
            {
                db.ExitInterviews.Add(exitInterview);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(exitInterview);
        }

        // GET: ExitInterviews/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExitInterview exitInterview = await db.ExitInterviews.FindAsync(id);
            if (exitInterview == null)
            {
                return HttpNotFound();
            }
            return View(exitInterview);
        }

        // POST: ExitInterviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "No,EmployeeNo,EmployeeName,Reason,OtherReasons,LeavingOn,ReEmploy,InterviewDate,InterviewerNo,InterviewerName,Comment,Status,ApproveProcess,ProcessCompleted")] ExitInterview exitInterview)
        {
            if (ModelState.IsValid)
            {
                db.Entry(exitInterview).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(exitInterview);
        }

        // GET: ExitInterviews/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExitInterview exitInterview = await db.ExitInterviews.FindAsync(id);
            if (exitInterview == null)
            {
                return HttpNotFound();
            }
            return View(exitInterview);
        }

        // POST: ExitInterviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            ExitInterview exitInterview = await db.ExitInterviews.FindAsync(id);
            db.ExitInterviews.Remove(exitInterview);
            await db.SaveChangesAsync();
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
