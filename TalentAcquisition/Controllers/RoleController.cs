using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalentAcquisition.Core.Domain;
using TalentAcquisition.Filters;
using TalentAcquisition.DataLayer;

namespace TalentAcquisition.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        // GET: Role
        //[AuthorizeRoles("SuperAdmin")]
        [Route("Admin/manage_officeposition")]
        public ActionResult Index()
        {
            try
            {
                var Positions = new List<OfficePosition>();
                using (var db = new TalentContext())
                {
                    ViewBag.Departments = db.Departments.ToList();
                    ViewBag.Industries = db.Industries.ToList();
                    Positions = db.OfficePositions.ToList();
                }

                return View(Positions);
            }
            catch
            {
                return View("Error");
            }
            
        }
        [ChildActionOnly]
        public ActionResult _GetOfficePositions()
        {
            try
            {
                var Positions = new List<OfficePosition>();
                using (var db = new TalentContext())
                {
                    Positions = db.OfficePositions.ToList();
                }
                return PartialView(Positions);
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: Role/Create
        [Route("Admin/Job/Create")]
        [Route("Admin/manage_officeposition/Create")]
        public ActionResult Create()
        {
            using (var db = new TalentContext())
            {
                ViewBag.Departments = db.Departments.ToList();
                ViewBag.Industries = db.Industries.ToList();
            }
            return View();
        }

        // POST: Role/Create
        [ValidateAntiForgeryToken]
        [HttpPost]
        [Route("Admin/manage_officeposition/Create")]
        public ActionResult Create(OfficePosition position)
        {
           // position.IsAvailable = false;
            if (!ModelState.IsValid)
            {
                return View(position);
            }
            try
            {
                using (var db = new TalentContext())
                {
                    db.OfficePositions.Add(position);
                    db.SaveChanges();
                    //Insert Logic to prevent adding the same role title 2 or more times to a single department 
                }

               return RedirectToAction("jobmanager","Admin");
            }
            catch
            {
                return View("Error");
            }
        }
        // GET: Role/Details/5
        [Route("Admin/Job/{id:int}")]
        [Route("Admin/manage_officeposition/Details/{id:int}",Name ="RoleDetails")]
        public ActionResult Details(int id)
        {
            try
            {
                var position = new OfficePosition();
                using (var db = new TalentContext())
                {
                    position = db.OfficePositions.Find(id);
                }

                return View(position);
            }
            catch
            {
                return View("Error");
            }
        }

        // GET: Role/Edit/5
        [Route("Admin/Edit/{id:int}")]
        [Route("Admin/manage_officeposition/Edit/{id}")]
        public ActionResult Edit(int id)
        {
            try
            {
                var position = new OfficePosition();
                using (var db = new TalentContext())
                {
                    position = db.OfficePositions.Find(id);
                    ViewBag.Departments = db.Departments.ToList();
                    ViewBag.Industries = db.Industries.ToList();
                }

                return View(position);
            }
            catch
            {
                return View("Error");
            }
        }
        // POST: Role/Edit/5
        [Route("Admin/Job/Edit/{id:int}")]
        [Route("Admin/manage_officeposition/Edit/{id}")]
        [HttpPost]
        public ActionResult Edit(int id, OfficePosition position)
        {
            if (!ModelState.IsValid)
            {
                return View(position);
            }
            try
            {
                using (var db = new TalentContext())
                {
                    db.Entry(position).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    //Insert Logic to prevent adding the same role title 2 or more times to a single department 
                }
                return RedirectToAction("manage_officeposition", "Admin");
            }
            catch
            {
                return View();
            }
        }

        // GET: Role/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Role/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
