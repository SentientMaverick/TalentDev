using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalentAcquisition.Core.Domain;
using TalentAcquisition.DataLayer;

namespace TalentAcquisition.Controllers
{
    public class DepartmentController : Controller
    {
        [Authorize]
        // GET: Department
        [Route("Admin/manage_departments")]
        public ActionResult Index()
        {
            try
            {
                var departments = new List<Department>();
                using (var db = new TalentContext())
                {
                    departments = db.Departments.ToList();
                }
                return View(departments);
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: Department/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [Route("Admin/manage_departments/Create")]
        // GET: Department/Create
        public ActionResult Create()
        {
            return View();
        }

        [Route("Admin/manage_departments/Create")]
        // POST: Department/Create
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(Department department)
        {
            if (!ModelState.IsValid)
            {
                return View(department);
            }
            try
            {
                // TODO: Add insert logic here
                using (var db=new TalentContext())
                {
                    db.Departments.Add(department);
                    db.SaveChanges();
                }
                return RedirectToAction("manage_departments","Admin");
            }
            catch
            {
                return View("Error");
            }
        }

        [Route("Admin/manage_departments/Edit/{id:int}")]
        // GET: Department/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var department = new Department();
                using (var db = new TalentContext())
                {
                    department = db.Departments.Find(id);
                }
                return View(department);
            }
            catch
            {
                return View("Error");
            }

        }

        [ValidateAntiForgeryToken]
        [Route("Admin/manage_departments/Edit/{id:int}")]
        // POST: Department/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Department department)
        {
            if (!ModelState.IsValid)
            {
                return View(department);
            }

            try
            {
                using (var db = new TalentContext())
                {
                    db.Entry(department).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("manage_departments", "Admin");
            }
            catch
            {
                return View("Error");
            }

        }

        [Route("Admin/manage_departments/Delete/{id:int}")]
        // GET: Department/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var department = new Department();
                using (var db = new TalentContext())
                {
                    department = db.Departments.Find(id);
                }
                return View(department);
            }
            catch
            {
                return View("Error");
            }
        }
        [ValidateAntiForgeryToken]
        [Route("Admin/manage_departments/Delete/{id:int}")]
        // POST: Department/Delete/5
        [HttpPost]
        public ActionResult Delete(int id,Department department)
        {
            try
            {
                
                using (var db = new TalentContext())
                    {
                        using (var dbTransaction=db.Database.BeginTransaction())
                        {
                            try
                            {
                            department = db.Departments.Find(id);

                           // var attachedroles = db.OfficePositions.Where(o => o.DepartmentID == id);
                            var attachedroles = department.OfficePositions;
                            //int? unassignedRoleId = db.OfficePositions.Where(o => o.Title == "unassigned").FirstOrDefault().OfficePositionID;
                            //int? unassignedDepartmentId = db.Departments.Where(o => o.DepartmentName == "unassigned").FirstOrDefault().DepartmentID;
                            if (attachedroles.Any())
                            {
                                foreach (var item in attachedroles)
                                {

                                }
                            }
                           
                                db.Departments.Remove(department);
                                db.SaveChanges();
                                dbTransaction.Commit();
                            }
                            catch
                            {
                                dbTransaction.Rollback();
                            }
                        }
                        
                    }
                    return RedirectToAction("manage_departments", "Admin");
            }
            catch
            {
                return View("Error");
            }
        }
    }
}
