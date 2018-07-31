using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Mvc;
using TalentAcquisition.BusinessLogic.UpdatedDomain;
using TalentAcquisition.DataLayer;
using TalentAcquisition.Repositories.Interfaces;
using TalentAcquisition.Repositories;

namespace TalentAcquisition.Controllers
{
    public class ManageEmployeeLeavesController : Controller
    {
        private TalentContext db = new TalentContext();
        private IEmployeeRepository _repo;
        public ManageEmployeeLeavesController()
        {
            _repo = new EmployeeRepository(db);
        }
        // GET: ManageEmployeeLeaves
        [Route("Employee/LeavePlan")]
        public ActionResult Index()
        {
            return View(db.ManageEmployeeLeaves.ToList());
        }

        // GET: ManageEmployeeLeaves/Details/5
        [Route("Employee/LeavePlan/Details/{id:int}")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ManageEmployeeLeave manageEmployeeLeave = db.ManageEmployeeLeaves.Find(id);
            if (manageEmployeeLeave == null)
            {
                return HttpNotFound();
            }
            return View(manageEmployeeLeave);
        }

        // GET: ManageEmployeeLeaves/Create
        [Route("Employee/LeavePlan/Create")]
        public ActionResult Create()
        {
            var model = new ManageEmployeeLeave();
            
            
            return View(model);
        }

        // POST: ManageEmployeeLeaves/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("Employee/LeavePlan/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,EmployeeId,EmployeeName,StartDate,EndDate,Duration,ApplyDate,Status,LeaveType,LeaveLimit,LeaveType_Limit_ID")] ManageEmployeeLeave manageEmployeeLeave)
        {
           
            if (ModelState.IsValid)
            {
                if (manageEmployeeLeave.LeaveLimit < manageEmployeeLeave.Duration)
                {
                    ViewBag.Error = "Leave Duration Cannot exceed leave limit!";
                    manageEmployeeLeave.EndDate = manageEmployeeLeave.StartDate.AddDays((double)manageEmployeeLeave.LeaveLimit - 1);
                    return View(manageEmployeeLeave);
                }
                //LeaveType_Limit leavetypelimit = new LeaveType_Limit();
                //if (leavetypelimit.ID == 0)
                //{
                //    ModelState.AddModelError("", "Select Leave Type");
                //}
                //int selectvalue = leavetypelimit.ID;
                //ViewBag.SelectedValue = leavetypelimit.Limit;
                //List<LeaveType_Limit> leavetypelist = new List<LeaveType_Limit>();
                //leavetypelist = (from c in db.LeaveType_Limits select c).ToList();
                //leavetypelist.Insert(0, new LeaveType_Limit { ID = 0, LeaveType = "Select" });
                                
                SetEmployeeSessionID();
                var id = Session["employeeid"];
                var empdetails = db.Employees.Select(c => c.EmployeeNumber).FirstOrDefault();
                var empname = db.Employees.Select(c => c.FirstName).FirstOrDefault();
                var emplastname = db.Employees.Select(c => c.LastName).FirstOrDefault();
                var empfullname = empname + " " + emplastname; 
                manageEmployeeLeave.EmployeeId = empdetails.ToString();
                manageEmployeeLeave.EmployeeName = empfullname.ToString();
                var diff1 = (manageEmployeeLeave.EndDate - manageEmployeeLeave.StartDate).Days;
                manageEmployeeLeave.Duration = diff1;
                manageEmployeeLeave.ApplyDate = DateTime.Now;
                //var status = (int)ManageEmployeeLeave.LeaveStatus.Pending;
                //manageEmployeeLeave.LeaveType = ViewBag.SelectedValue;
                
                manageEmployeeLeave.Status = ManageEmployeeLeave.LeaveStatus.Pending;
                //manageEmployeeLeave.LeaveType = db.LeaveType_Limits.Distinct().Select(c => new SelectListItem() { Text = c.LeaveType, Value = c.ID.ToString()}).ToString();
                 db.ManageEmployeeLeaves.Add(manageEmployeeLeave);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(manageEmployeeLeave);
        }

        public JsonResult GetLeavetypeLimit()
        {
            return Json(db.LeaveType_Limits.Where(x=>x.RequiresPlan==true).ToList(), JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //public JsonResult GetLeaveLimit(string LimitId)
        //{
        //    int Id =Convert.ToInt32(LimitId);
        ////    var limit = db.LeaveType_Limits.Where(c=>c.ID==Id).Select(c => c.Limit);
        //    var limit = db.LeaveType_Limits.Where(c => c.ID == Id).Select(c => new {c.ID, c.Limit });

        //    //var limit = from a in db.LeaveType_Limits where a.ID == Id select a;

        //    return Json(limit, JsonRequestBehavior.AllowGet);
        //  //  return Json(limit);
        //}

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
                var limit= db.LeaveType_Limits
                               .Where(c => c.LeaveType == Limit)
                               .Select(c => new { ID=c.ID, Limit=leaveremaining });
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

        // GET: ManageEmployeeLeaves/Edit/5
        [Route("Employee/LeavePlan/Edit/{id:int}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ManageEmployeeLeave manageEmployeeLeave = db.ManageEmployeeLeaves.Find(id);
            if (manageEmployeeLeave == null)
            {
                return HttpNotFound();
            }
            return View(manageEmployeeLeave);
        }

        // POST: ManageEmployeeLeaves/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("Employee/LeavePlan/Edit/{id:int}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,EmployeeId,EmployeeName,StartDate,EndDate,Duration,ApplyDate,Status,LeaveType,LeaveLimit,LeaveType_Limit_ID")] ManageEmployeeLeave manageEmployeeLeave)
        {
            if (ModelState.IsValid)
            {
                db.Entry(manageEmployeeLeave).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(manageEmployeeLeave);
        }

        // GET: ManageEmployeeLeaves/Delete/5
        [Route("Employee/LeavePlan/Delete/{id:int}")]
        public ActionResult Delete(int? id)
        {
            if (id == null)   
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ManageEmployeeLeave manageEmployeeLeave = db.ManageEmployeeLeaves.Find(id);
            if (manageEmployeeLeave == null)
            {
                return HttpNotFound();
            }
            return View(manageEmployeeLeave);
        }

        // POST: ManageEmployeeLeaves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ManageEmployeeLeave manageEmployeeLeave = db.ManageEmployeeLeaves.Find(id);
            db.ManageEmployeeLeaves.Remove(manageEmployeeLeave);
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
