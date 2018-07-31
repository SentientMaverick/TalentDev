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

namespace TalentAcquisition.Controllers
{
    public class LeaveApplicationsController : Controller
    {
        private TalentContext db = new TalentContext();
        private IEmployeeRepository _repo;
        public LeaveApplicationsController()
        {
            _repo = new EmployeeRepository(db);
        }
        [Route("Admin/LeaveApplication")]
        public async Task<ActionResult> LeaveApplication()
        {
            return View(await db.LeaveApplications.ToListAsync());
        }
        // GET: LeaveApplications
        [Route("Employee/LeaveApplication/All")]
        public async Task<ActionResult> Index()
        {
            var currentemployeeid = User.Identity.GetUserId();
            var currentemployee = _repo.GetAll().Where(x => x.UserId == currentemployeeid).FirstOrDefault();
            return View(await db.LeaveApplications.Where(x=>x.EmployeeId==currentemployee.ID.ToString()).ToListAsync());
        }
        [ChildActionOnly]
        public ActionResult GetApplicationsList()
        {
            return PartialView(db.LeaveApplications.Where(x=>x.LeaveAppStatus==
            BusinessLogic.UpdatedDomain.LeaveApplication.LeaveApplicationStatus.Pending).ToList());
        }
        [ChildActionOnly]
        public ActionResult GetApprovedApplicationsList()
        {
            return PartialView(db.LeaveApplications.Where(x => x.LeaveAppStatus ==
            BusinessLogic.UpdatedDomain.LeaveApplication.LeaveApplicationStatus.Approved).ToList());
        }
        [ChildActionOnly]
        public ActionResult GetRejectedApplicationsList()
        {
            return PartialView( db.LeaveApplications.Where(x => x.LeaveAppStatus ==
            BusinessLogic.UpdatedDomain.LeaveApplication.LeaveApplicationStatus.Rejected).ToList());
        }
        // GET: LeaveApplications/Details/5
        [Route("LeaveApplication/Details/{id}")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveApplication leaveApplication = await db.LeaveApplications.FindAsync(id);
            if (leaveApplication == null)
            {
                return HttpNotFound();
            }
            return View(leaveApplication);
        }
        [Route("Employee/LeaveApplication/Create/Plan/{planid}")]
        [Route("Employee/LeaveApplication/Create")]
        // GET: LeaveApplications/Create
        public ActionResult Create(int? planid)
        {
            var model = new LeaveApplication();
            var userid = User.Identity.GetUserId();
            var employee = db.Employees.Where(x => x.UserId == userid).FirstOrDefault();
            model.EmployeeId = employee.ID.ToString();
            model.EmployeeName = employee.FullName;
            model.ApplyDate = DateTime.Now;
            if (planid != null)
            {
                ManageEmployeeLeave leaveplan = db.ManageEmployeeLeaves
                                                   .Where(x => x.ID == planid)
                                                   .FirstOrDefault();
                //ManageEmployeeLeave leaveplan = db.ManageEmployeeLeaves
                //                              .Where(x => x.ID == planid && x.EmployeeId == model.EmployeeId)
                //                              .FirstOrDefault();
                if (leaveplan != null)
                {
                    model.StartDate = leaveplan.StartDate;
                    model.LeaveType = leaveplan.LeaveType;
                    model.LeaveLimit = leaveplan.LeaveLimit;
                    model.LeaveLimit = leaveplan.LeaveLimit;
                    model.EndDate = leaveplan.EndDate;
                    model.LeavePlanID = leaveplan.ID;
                    model.TotalLeaveTaken = leaveplan.TotalLeaveTaken;
                    model.TotalLeaveAvailable = leaveplan.TotalLeaveAvailable;
                    model.Duration = leaveplan.Duration;
                    model.LeavePlanStatus = "Approved";
                    model.LeaveAppStatus = BusinessLogic.UpdatedDomain.LeaveApplication.LeaveApplicationStatus.Pending;
                }
                else
                {
                    model.StartDate = DateTime.Now;
                    model.EndDate = DateTime.Now;
                    ViewBag.LeaveType = new SelectList(db.LeaveType_Limits.Where(x => x.LeaveType == "Emergency").ToList(), "LeaveType", "LeaveType");
                    model.LeaveLimit = db.LeaveType_Limits.Where(x => x.LeaveType == "Emergency").FirstOrDefault().Limit;
                    model.LeavePlanStatus = "Pending";
                    model.LeaveAppStatus = BusinessLogic.UpdatedDomain.LeaveApplication.LeaveApplicationStatus.Pending;
                    ViewBag.Error = "Leave Plan Does Not Exist";
                }
            }
            else
            {
                model.StartDate = DateTime.Now;
                model.EndDate = DateTime.Now;
                ViewBag.LeaveType = new SelectList(db.LeaveType_Limits.ToList(), "LeaveType", "LeaveType");
                model.LeaveLimit = null;
                model.LeavePlanStatus = "None";
                model.LeaveAppStatus = BusinessLogic.UpdatedDomain.LeaveApplication.LeaveApplicationStatus.Pending;
                ViewBag.LeaveTypesList = db.LeaveType_Limits.Select(c => new { c.ID, c.Limit,c.LeaveType });
             }
             return View(model);
        }

        // POST: LeaveApplications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("Employee/LeaveApplication/Create/Plan/{planid}")]
        [Route("Employee/LeaveApplication/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "LeaveAppID,LeavePlanID,EmployeeId,EmployeeName,LeaveType,LeaveLimit,TotalLeaveTaken,TotalLeaveAvailable,StartDate,EndDate,Duration,ApplyDate,LeavePlanStatus,LeaveAppStatus")] LeaveApplication leaveApplication)
        {
            if (ModelState.IsValid)
            {
                if (leaveApplication.LeaveLimit < leaveApplication.Duration)
                {
                    ViewBag.Error = "Leave Duration Cannot exceed leave limit!";
                    leaveApplication.EndDate = leaveApplication.StartDate.AddDays((double)leaveApplication.LeaveLimit - 1);
                    return View(leaveApplication);
                }
                db.LeaveApplications.Add(leaveApplication);
                await db.SaveChangesAsync();
                //await Task.Delay(50);
                return RedirectToAction("Index");
            }

            return View(leaveApplication);
        }

        // GET: LeaveApplications/Edit/5
        [Route("LeaveApplication/Approve/{id}")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveApplication leaveApplication = await db.LeaveApplications.FindAsync(id);
            if (leaveApplication == null)
            {
                return HttpNotFound();
            }
            return View(leaveApplication);
        }

        // POST: LeaveApplications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("LeaveApplication/Approve/{id}")]
        public async Task<ActionResult> Edit([Bind(Include = "LeaveAppID,LeavePlanID,EmployeeId,EmployeeName,LeaveType,LeaveLimit,TotalLeaveTaken,TotalLeaveAvailable,StartDate,EndDate,Duration,ApplyDate,LeavePlanStatus,LeaveAppStatus")] LeaveApplication leaveApplication)
        {
            if (ModelState.IsValid)
            {
                db.Entry(leaveApplication).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("LeaveApplication","Admin");
            }
            return View(leaveApplication);
        }
        // GET: LeaveApplications/Delete/5
        [Route("LeaveApplication/Delete/{id}")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaveApplication leaveApplication = await db.LeaveApplications.FindAsync(id);
            if (leaveApplication == null)
            {
                return HttpNotFound();
            }
            return View(leaveApplication);
        }
        // POST: LeaveApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            LeaveApplication leaveApplication = await db.LeaveApplications.FindAsync(id);
            db.LeaveApplications.Remove(leaveApplication);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public JsonResult GetLeavetypeLimit()
        {
            return Json(db.LeaveType_Limits.ToList(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetLeaveLimit(string Limit)
        {

            //    var limit = db.LeaveType_Limits.Where(c=>c.ID==Id).Select(c => c.Limit);
            SetEmployeeSessionID();
            var id = Session["employeeid"];
            var employee = _repo.GetEmployee((int)id);
            var existingleaveplan = db.ManageEmployeeLeaves
                                      .Where(x => x.EmployeeId == employee.EmployeeNumber)
                                      .Where(x => x.LeaveType == Limit)
                                      .Where(x => x.Status == ManageEmployeeLeave.LeaveStatus.Approved).ToList();
            if (existingleaveplan.Any())
            {
                var leavetaken = existingleaveplan.Sum(x => x.TotalLeaveTaken);
                //  var limit = new Dictionary<string, dynamic>();
                var leaveremaining = existingleaveplan.FirstOrDefault().LeaveLimit - leavetaken;
                var limit = db.LeaveType_Limits
                               .Where(c => c.LeaveType == Limit)
                               .Select(c => new { ID = c.ID, Limit = leaveremaining });
                //limit.Add("ID", existingleaveplan.LastOrDefault().LeaveType);
                //limit.Add("Limit", existingleaveplan.FirstOrDefault().LeaveLimit -leavetaken);
                return Json(limit, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var limit = db.LeaveType_Limits.Where(c => c.LeaveType == Limit).Select(c => new { c.ID, c.Limit });
                return Json(limit, JsonRequestBehavior.AllowGet);
            }
            //var limit = from a in db.LeaveType_Limits where a.ID == Id select a;


            //  return Json(limit);
        }
        private void SetEmployeeSessionID()
        {
            if (Session["employeeid"] == null)
            {
                var userid = User.Identity.GetUserId();
                var emp = new TalentContext().Employees.Where(s => s.UserId == userid);
                if (emp.Any())
                {
                    var empid = emp.FirstOrDefault().ID;
                    Session["employeeid"] = empid;
                }
                else
                {
                    // throw new HttpException(HttpStatusCode.BadRequest,"BadRequest");
                    // throw new HttpException();
                }
            }
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
