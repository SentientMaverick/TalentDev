using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TalentAcquisition.BusinessLogic.UpdatedDomain;
using TalentAcquisition.Core.Domain;
using TalentAcquisition.DataLayer;

namespace TalentAcquisition.Controllers
{
    public class EmployeesController : Controller
    {
        #region Iniializer
        private TalentContext db = new TalentContext();
        private AppManager app = new AppManager();
        public string UserId { get; private set; }
        private void SetInitializers()
        {
            var var1 = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            var var2 = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            app.UserManager = var2; app.SignInManager = var1;
            app.Serverpath = Server.MapPath("~/Uploads/");
        }
        public EmployeesController()
        {
           
        }
        #endregion

        #region Views
        // GET: Employees
        [Route("Admin/PersonnelManager")]
        [Route("Admin/Restricted/manage_Employee")]
        public ActionResult Index()
        {
            var employees = db.Employees.Include(e => e.OfficePosition);
            return View(employees.ToList());
        }
        [HttpGet]
        [Route("Employees/Profile")]
        public ActionResult EmployeeProfile()
        {
            UserId = User.Identity.GetUserId();
            string userid = UserId;
            var employee = db.Employees.Where(x => x.UserId == userid).First();
            ViewBag.OfficePositionID = new SelectList(db.OfficePositions, "OfficePositionID", "Title", employee.OfficePositionID);
            return View("Profile",employee);
        }
        [Route("Employees/Profile")]
        [HttpPost]
        public ActionResult EmployeeProfile(Employee employee)
        {
            if (ModelState.IsValid)
            {
                using (var db = new TalentContext())
                {
                    var originaluserdata = db.Employees.Find(employee.ID);
                    originaluserdata.FirstName = employee.FirstName;
                    originaluserdata.LastName = employee.LastName;
                    originaluserdata.PhoneNumber = employee.PhoneNumber;
                    originaluserdata.Address = employee.Address;
                    originaluserdata.PhoneNumber = employee.PhoneNumber;
                    originaluserdata.Password = employee.Password;
                    db.Entry(originaluserdata).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Dashboard","Admin");
            }
            ViewBag.OfficePositionID = new SelectList(db.OfficePositions, "OfficePositionID", "Title", employee.OfficePositionID);
            return View("Profile", employee);
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
            ViewBag.AdminPriviledge = false;
            if (User.IsInRole("Admin"))
            {
                ViewBag.AdminPriviledge = true;
            }
            return View(employee);
        }

        // GET: Employees/Create
        [Route("Admin/Personnel/Create")]
        [Route("Admin/Restricted/add_Employee")]
        public ActionResult Create(int? guideid, Employee data = null)
        {
            var employee = new Employee();
            if (data != null)
            {
                employee = data;
            }
            var employeecount = db.Employees.Count();
            employee.EmployeeNumber = @"SN/" + String.Format("{0:D4}", employeecount + 1);
            ViewBag.OfficePositionID = new SelectList(db.OfficePositions, "OfficePositionID", "Title");
            if (guideid != null)
            {
                var guide = new WelcomeGuide();
                using (var db = new TalentContext())
                {
                    var _guide = db.WelcomeGuides.Find(guideid);
                    if (_guide == null)
                    {
                        return HttpNotFound();
                    }
                    guide = _guide;

                    if ((guide.JobSeekerID == null) || (guide.JobSeekerID == 0))
                    {
                        string[] names = guide.Name.Split(' ');
                        employee.FirstName = names[0];
                        employee.LastName = names[1];
                        employee.DateOfBirth = DateTime.Now.AddYears(-20);
                    }
                    else
                    {
                        var jobapplicant = db.Applicants.Where(x => x.ID == guide.JobSeekerID).FirstOrDefault();
                        //employee.FirstName = jobapplicant.FirstName;
                        //employee.LastName = jobapplicant.LastName;
                        string[] names = guide.Name.Split(' ');
                        employee.FirstName = names[0];
                        employee.LastName = names[1];
                        employee.DateOfBirth = jobapplicant.DateOfBirth;
                        employee.PhoneNumber = jobapplicant.PhoneNumber;
                        employee.Address = jobapplicant.Address;
                    }
                    employee.EmploymentDate = guide.StartDate;
                    employee.OfficePositionID = db.OfficePositions.Where(x => x.Title == guide.Position).First().OfficePositionID;

                }
                ViewBag.GuideCreation = true;
                return View(employee);
            }
            ViewBag.GuideCreation = false;
            return View(employee);
        }
        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/Personnel/Create")]
        [Route("Admin/Restricted/add_Employee")]
        public async Task<ActionResult> Create([Bind(Include = "ID,EmployeeNumber,EmploymentDate,OfficePositionID,UserId,FirstName,LastName,DateOfBirth,Password,Address,PhoneNumber")] Employee employee, string UserEmail, HttpPostedFileBase ImageData)
        {
            try
            {
                SetInitializers();
                return await app.EmployeeRegistration(employee, UserEmail, ImageData);
            }
            catch
            {
                return View(employee);
            }
        }
        [HttpPost]
        [Route("Admin/Personnel/Update/{id:int}")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,EmployeeNumber,EmploymentDate,OfficePositionID,UserId,FirstName,LastName,DateOfBirth,Password,Address,PhoneNumber")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                using (var db = new TalentContext())
                {
                    var originaluserdata = db.Employees.Find(employee.ID);
                    originaluserdata.FirstName = employee.FirstName;
                    originaluserdata.LastName = employee.LastName;
                    originaluserdata.PhoneNumber = employee.PhoneNumber;
                    originaluserdata.Address = employee.Address;
                    originaluserdata.PhoneNumber = employee.PhoneNumber;
                    originaluserdata.OfficePositionID = employee.OfficePositionID;
                    originaluserdata.Password = employee.Password;
                    db.Entry(originaluserdata).State = EntityState.Modified;
                    db.SaveChanges();
                }
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
        #endregion

        #region RedundantOnboarding
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
        #endregion

        #region JsonActions
        [Route("api/employee/getskills")]
        public JsonResult GetSkillList(int id=1, int skillId=0)
        {
            if (id == 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            List<BusinessLogic.Domain.Skill> skills = db.Employees.Find(id).Skills.Select(O=>new BusinessLogic.Domain.Skill {ID=O.ID,Name=O.Name }).ToList();
            return Json(new { status="True",skills=skills }, JsonRequestBehavior.AllowGet);
        }
        [Route("api/employee/addskill")]
        [HttpPost]
        public JsonResult AddSkill(int id,string Title)
        {
            if (id == 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            var employee = db.Employees.Where(x => x.ID == id);
            if (employee == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(Title))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            var skill = new BusinessLogic.Domain.Skill { Name = Title, IndustryId = null };
            employee.FirstOrDefault().Skills.Add((BusinessLogic.Domain.Skill)skill);
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public JsonResult RemoveSkill(int id, int skillId)
        {
            if (id == 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            var employee= db.Employees.Where(x => x.ID == id);
            if (employee == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            var skill = db.Skills.Where(x => x.ID == skillId);
            if (skill == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            employee.FirstOrDefault().Skills.Remove((BusinessLogic.Domain.Skill)skill);
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

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
