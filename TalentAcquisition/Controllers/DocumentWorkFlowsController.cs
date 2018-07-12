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
using TalentAcquisition.Helper;

namespace TalentAcquisition.Controllers
{
    public class DocumentWorkFlowsController : Controller
    {
        #region Initializers
        private TalentContext db = new TalentContext();
        private readonly IWorkflowHelper _workFlowHelper = new WorkFlowHelper();

        #endregion
        #region Views
        [Route("Approval/Flows/All")]
        // GET: DocumentWorkFlows
        public async Task<ActionResult> Index()
        {
            ViewBag.Employees = db.Employees.ToList();
            return View(await db.ApprovalFlows.ToListAsync());
        }
        // GET: DocumentWorkFlows/Create
        [Route("Approval/Entries/All")]
        public async Task<ActionResult> ApprovalEntries()
        {
            ViewBag.Employees = db.Employees.ToList();
           // ViewBag.Employees = db.Employees.Select(p => new { Id = p.ID, Name = p.FirstName + " " + p.LastName, Number = p.EmployeeNumber, Department = p.OfficePosition.Department.DepartmentName });
            return View(await db.ApprovalEntries.ToListAsync());
        }
        [Route("Approval/Flows/{id}")]
        // GET: DocumentWorkFlows/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentWorkFlow documentWorkFlow = await db.ApprovalFlows.FindAsync(id);
            if (documentWorkFlow == null)
            {
                return HttpNotFound();
            }
            return View(documentWorkFlow);
        }

        // GET: DocumentWorkFlows/Create
        [Route("Approval/Flows/Create")]
        public ActionResult Create()
        {
            var processsnames = new Dictionary<string, string>();
            processsnames.Add("Cash", "Cash Requisition");
            processsnames.Add("Training", "Training Requisition");
            processsnames.Add("Grievance", "Grievance Requisition");
            processsnames.Add("Job", "Job Requisition");
            processsnames.Add("KPI", "Performance Appraisal");
            ViewBag.ProcessName = processsnames.Select(x=>new {x.Key,x.Value });
            ViewBag.Employees = db.Employees.Select(p => new { Id = p.ID, Name = p.FirstName + " " + p.LastName, Number = p.EmployeeNumber, Department = p.OfficePosition.Department.DepartmentName });
            return View();
        }

        
        [Route("Approval/Flows/Create")]
        // POST: DocumentWorkFlows/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Sender,ProcessName,NoOfApprovals,Approver1Id,Approver2Id,Approver3Id,Approver4Id,Approver5Id,IsEnabled")] DocumentWorkFlow documentWorkFlow)
        {
            if (ModelState.IsValid)
            {
                db.ApprovalFlows.Add(documentWorkFlow);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(documentWorkFlow);
        }

        // GET: DocumentWorkFlows/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentWorkFlow documentWorkFlow = await db.ApprovalFlows.FindAsync(id);
            if (documentWorkFlow == null)
            {
                return HttpNotFound();
            }
            return View(documentWorkFlow);
        }

        // POST: DocumentWorkFlows/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Sender,ProcessName,NoOfApprovals,Approver1Id,Approver2Id,Approver3Id,Approver4Id,Approver5Id,IsEnabled")] DocumentWorkFlow documentWorkFlow)
        {
            if (ModelState.IsValid)
            {
                db.Entry(documentWorkFlow).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(documentWorkFlow);
        }

        // GET: DocumentWorkFlows/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentWorkFlow documentWorkFlow = await db.ApprovalFlows.FindAsync(id);
            if (documentWorkFlow == null)
            {
                return HttpNotFound();
            }
            return View(documentWorkFlow);
        }

        // POST: DocumentWorkFlows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            DocumentWorkFlow documentWorkFlow = await db.ApprovalFlows.FindAsync(id);
            db.ApprovalFlows.Remove(documentWorkFlow);
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
        #endregion
    }
}
