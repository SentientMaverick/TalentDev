using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using TalentAcquisition.Core.Domain;
using TalentAcquisition.DataLayer;

namespace TalentAcquisition.Controllers
{
    public class EmployeesController : Controller
    {
        private TalentContext db = new TalentContext();
        private AppManager app = new AppManager();
        private void SetInitializers()
        {
            var var1 = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            var var2 = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            app.UserManager = var2; app.SignInManager = var1;
        }

        // GET: Employees
        [Route("Admin/PersonnelManager")]
        [Route("Admin/Restricted/manage_Employee")]
        public ActionResult Index()
        {
            var employees = db.Employees.Include(e => e.OfficePosition);
            return View(employees.ToList());
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        [Route("Admin/Personnel/Create")]
        [Route("Admin/Restricted/add_Employee")]
        public ActionResult Create()
        {
            ViewBag.OfficePositionID = new SelectList(db.OfficePositions, "OfficePositionID", "Title");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/Personnel/Create")]
        [Route("Admin/Restricted/add_Employee")]
        public ActionResult Create([Bind(Include = "ID,EmployeeNumber,EmploymentDate,OfficePositionID,UserId,FirstName,LastName,DateOfBirth,Password,Address,PhoneNumber")] Employee employee,string UserEmail)
        {
            SetInitializers();
            return app.EmployeeRegistration(employee,UserEmail);
        }

        // GET: Employees/Edit/5
        [Route("Admin/Personnel/Update/{id:int}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.OfficePositionID = new SelectList(db.OfficePositions, "OfficePositionID", "Title", employee.OfficePositionID);
            return View(employee);
        }
        [Route("Admin/Personnel/Onboard/{id:int}/Start")]
        public ActionResult StartOnboarding(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobSeeker applicant = db.Applicants.Find(id);
            if (applicant == null)
            {
                return HttpNotFound();
            }
            var employeerecord = db.Employees.Where(s=>s.UserId == applicant.UserId);
            Employee employee = new Employee();
            if (!employeerecord.Any())
            {
                employee = new Employee
                {
                    FirstName = applicant.FirstName,
                    LastName = applicant.LastName,
                    Address = applicant.Address,
                    DateOfBirth = applicant.DateOfBirth,
                    UserId = applicant.UserId,
                    EmploymentDate = DateTime.Now,
                    PhoneNumber = applicant.PhoneNumber
                };
                db.Employees.Add(employee);
                db.SaveChanges();
            }
            ViewBag.OfficePositionID = new SelectList(db.OfficePositions, "OfficePositionID", "Title", employee.OfficePositionID);
            return View(employee);
        }
        [Route("Admin/Personnel/Onboard/{id:int}/Update")]
        public ActionResult UpdateOnboarding(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobSeeker applicant = db.Applicants.Find(id);
            if (applicant == null)
            {
                return HttpNotFound();
            }
            var employeerecord = db.Employees.Where(s => s.UserId == applicant.UserId);
            Employee employee = new Employee();
            if (!employeerecord.Any())
            {
                employee = new Employee
                {
                    FirstName = applicant.FirstName,
                    LastName = applicant.LastName,
                    Address = applicant.Address,
                    DateOfBirth = applicant.DateOfBirth,
                    UserId = applicant.UserId,
                    EmploymentDate = DateTime.Now,
                    PhoneNumber = applicant.PhoneNumber
                };
                db.Employees.Add(employee);
                db.SaveChanges();
            }
            ViewBag.OfficePositionID = new SelectList(db.OfficePositions, "OfficePositionID", "Title", employee.OfficePositionID);
            return View(employee);
        }
        [Route("Admin/Personnel/Onboard/{id:int}/Complete")]
        public ActionResult Completeboarding(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobSeeker applicant = db.Applicants.Find(id);
            if (applicant == null)
            {
                return HttpNotFound();
            }
            var employeerecord = db.Employees.Where(s => s.UserId == applicant.UserId);
            Employee employee = new Employee();
            if (!employeerecord.Any())
            {
                employee = new Employee
                {
                    FirstName = applicant.FirstName,
                    LastName = applicant.LastName,
                    Address = applicant.Address,
                    DateOfBirth = applicant.DateOfBirth,
                    UserId = applicant.UserId,
                    EmploymentDate = DateTime.Now,
                    PhoneNumber = applicant.PhoneNumber
                };
                db.Employees.Add(employee);
                db.SaveChanges();
            }
            ViewBag.OfficePositionID = new SelectList(db.OfficePositions, "OfficePositionID", "Title", employee.OfficePositionID);
            return View(employee);
        }
        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [Route("Admin/Personnel/Update/{id:int}")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,EmployeeNumber,EmploymentDate,OfficePositionID,UserId,FirstName,LastName,DateOfBirth,Password,Address,PhoneNumber")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OfficePositionID = new SelectList(db.OfficePositions, "OfficePositionID", "Title", employee.OfficePositionID);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
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
