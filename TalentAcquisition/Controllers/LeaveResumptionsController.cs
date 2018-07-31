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
using Microsoft.AspNet.Identity;
using TalentAcquisition.Repositories.Interfaces;
using TalentAcquisition.Repositories;
using Talent.HRM.Services.Interfaces;
using Talent.HRM.Services.Email;
using System.IO;

namespace TalentAcquisition.Controllers
{
    public class LeaveResumptionsController : Controller
    {
        private TalentContext db = new TalentContext();
        private IEmployeeRepository _repo;
        private IEmailMessaging _emailhandler;
        public LeaveResumptionsController()
        {
            _repo = new EmployeeRepository(db);
        }
        [Route("Admin/LeaveResumption")]
        // GET: LeaveResumptions
        public ActionResult Index()
        {
            return View();
        }
        [Route("Employee/LeaveApplication/Existing")]
        // GET: LeaveResumptions
        public async Task<ActionResult> LeaveApplicationExisting()
        {
            var currentemployeeid = User.Identity.GetUserId();
            var currentemployee = _repo.GetAll().Where(x => x.UserId == currentemployeeid).FirstOrDefault();
            return View(await db.LeaveApplications
                                .Where(x => x.EmployeeId == currentemployee.ID.ToString())
                                .Where(y=>y.LeaveAppStatus==LeaveApplication.LeaveApplicationStatus.Approved)
                                .ToListAsync());
        }
        [ChildActionOnly]
        public ActionResult GetPendingList()
        {
            return PartialView(db.LeaveResumptions.Where(x => x.Status ==
            BusinessLogic.UpdatedDomain.LeaveResumption.LeaveResumptionStatus.Pending).ToList());
        }
        [ChildActionOnly]
        public ActionResult GetApprovedList()
        {
            return PartialView(db.LeaveResumptions.Where(x => x.Status ==
            BusinessLogic.UpdatedDomain.LeaveResumption.LeaveResumptionStatus.Approved).ToList());
        }
        [ChildActionOnly]
        public ActionResult GetRejectedList()
        {
            return PartialView(db.LeaveResumptions.Where(x => x.Status ==
            BusinessLogic.UpdatedDomain.LeaveResumption.LeaveResumptionStatus.Rejected).ToList());
        }
        // GET: LeaveResumptions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveResumption leaveResumption = await db.LeaveResumptions.FindAsync(id);
            if (leaveResumption == null)
            {
                return HttpNotFound();
            }
            return View(leaveResumption);
        }

        [Route("Employee/LeaveApplication/{id}/Resume")]
        // GET: LeaveResumptions/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var application = db.LeaveApplications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            var model = new LeaveResumption();
            model.ApplyDate = DateTime.Now;
            model.EmployeeId = application.EmployeeId;
            model.EmployeeName = application.EmployeeName;
            model.LeaveAppID = application.LeaveAppID;
            model.StartDate = application.StartDate;
            model.LeaveType = application.LeaveType;
            model.ResumptionDate = DateTime.Now;
            model.Status = LeaveResumption.LeaveResumptionStatus.Pending;
            model.Duration = model.ResumptionDate.Day - model.StartDate.Day;
            return View(model);
        }
        // POST: LeaveResumptions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("Employee/LeaveApplication/{id}/Resume")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,LeaveAppID,EmployeeId,EmployeeName,StartDate,ResumptionDate,Duration,Status,ApplyDate,LeaveType")] LeaveResumption leaveResumption)
        {
            if (ModelState.IsValid)
            {
                db.LeaveResumptions.Add(leaveResumption);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(leaveResumption);
        }
        // GET: LeaveResumptions/Edit/5
        [Route("LeaveResumption/{id}/Approve")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveResumption leaveResumption = await db.LeaveResumptions.FindAsync(id);
            if (leaveResumption == null)
            {
                return HttpNotFound();
            }
            return View(leaveResumption);
        }
        // POST: LeaveResumptions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("LeaveResumption/{id}/Approve")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,LeaveAppID,EmployeeId,EmployeeName,StartDate,ResumptionDate,Duration,Status,ApplyDate,LeaveType")] LeaveResumption leaveResumption)
        {
            if (ModelState.IsValid)
            {
                var employee = _repo.GetEmployee(int.Parse(leaveResumption.EmployeeId));
                if (leaveResumption.Status == LeaveResumption.LeaveResumptionStatus.Approved)
                {
                    LeaveApplication leaveapplication = db.LeaveApplications.Find(leaveResumption.LeaveAppID);
                    if (leaveapplication.LeavePlanID != null)
                    {
                        ManageEmployeeLeave leaveplan = db.ManageEmployeeLeaves.Find(leaveapplication.LeavePlanID);
                        if (leaveplan.TotalLeaveAvailable==null)
                        {
                            leaveplan.TotalLeaveAvailable = leaveplan.LeaveLimit;
                        }
                        if (leaveplan.TotalLeaveTaken == null)
                        {
                            leaveplan.TotalLeaveTaken = 0;
                        }
                        leaveplan.TotalLeaveTaken += leaveResumption.Duration;
                        leaveplan.TotalLeaveAvailable -= leaveResumption.Duration;
                    }
                    if (leaveapplication.TotalLeaveAvailable == null)
                    {
                        leaveapplication.TotalLeaveAvailable = leaveapplication.LeaveLimit;
                    }
                    if (leaveapplication.TotalLeaveTaken == null)
                    {
                        leaveapplication.TotalLeaveTaken = 0;
                    }
                    leaveapplication.TotalLeaveAvailable -= leaveResumption.Duration;
                    leaveapplication.TotalLeaveTaken += leaveResumption.Duration;

                    string FilePath = Server.MapPath("~/Assets/EmailTemplate/LeaveApproved.cshtml");
                    StreamReader str = new StreamReader(FilePath);
                    string MailText = str.ReadToEnd();
                    str.Close();

                    MailText = MailText.Replace("[Date]", DateTime.Now.ToShortDateString());
                    MailText = MailText.Replace("[StartDate]", leaveResumption.StartDate.ToShortDateString());
                    MailText = MailText.Replace("[ResumptionDate]", leaveResumption.ResumptionDate.ToShortDateString());
                    MailText = MailText.Replace("[Name]", employee.FullName);
                    MailText = MailText.Replace("[HRName]", "Bolaji Fashola");
                    _emailhandler = new LeaveApplicationAcceptedEmail(MailText);
                    _emailhandler.SendEmail("ayandaoluwatosin@gmail.com", "");
                }
                db.Entry(leaveResumption).State = EntityState.Modified;
                await db.SaveChangesAsync();
                //await Task.Delay(50);
                return RedirectToAction("Index");
            }
            return View(leaveResumption);
        }

        // GET: LeaveResumptions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveResumption leaveResumption = await db.LeaveResumptions.FindAsync(id);
            if (leaveResumption == null)
            {
                return HttpNotFound();
            }
            return View(leaveResumption);
        }

        // POST: LeaveResumptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            LeaveResumption leaveResumption = await db.LeaveResumptions.FindAsync(id);
            db.LeaveResumptions.Remove(leaveResumption);
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
