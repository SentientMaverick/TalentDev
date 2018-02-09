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
    public class IndustryController : Controller
    {

        [Route("Admin/manage_industries")]
        [Authorize]// GET: Industry
        public ActionResult Index()
        {
            try
            {
                var industries = new List<Industry>();
                using (var db = new TalentContext())
                {
                    industries = db.Industries.ToList();
                }
                return View(industries);
            }
            catch
            {
                return View("Error");
            }
        }
        [Route("Admin/manage_industries/Create")]
        // GET: Department/Create
        public ActionResult Create()
        {
            return View();
        }

        [Route("Admin/manage_industries/Create")]
        // POST: Department/Create
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(Industry industry)
        {
            if (!ModelState.IsValid)
            {
                return View(industry);
            }
            try
            {
                // TODO: Add insert logic here
                using (var db = new TalentContext())
                {
                    db.Industries.Add(industry);
                    db.SaveChanges();
                }
                return RedirectToAction("manage_industries", "Admin");
            }
            catch
            {
                return View("Error");
            }
        }

        [Route("Admin/manage_industries/Edit/{id:int}")]
        // GET: Department/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                Industry industry = new Industry();
                using (var db = new TalentContext())
                {
                    industry = db.Industries.Find(id);
                }
                return View(industry);
            }
            catch
            {
                return View("Error");
            }

        }

        [ValidateAntiForgeryToken]
        [Route("Admin/manage_industries/Edit/{id:int}")]
        // POST: Department/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Industry industry)
        {
            if (!ModelState.IsValid)
            {
                return View(industry);
            }

            try
            {
                using (var db = new TalentContext())
                {
                    db.Entry(industry).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("manage_industries", "Admin");
            }
            catch
            {
                return View("Error");
            }

        }

        [Route("Admin/manage_industries/Delete/{id:int}")]
        // GET: Department/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var industry = new Industry();
                using (var db = new TalentContext())
                {
                    industry = db.Industries.Find(id);
                }
                return View(industry);
            }
            catch
            {
                return View("Error");
            }
        }
        [ValidateAntiForgeryToken]
        [Route("Admin/manage_industries/Delete/{id:int}")]
        // POST: Department/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Industry industry)
        {
            try
            {

                using (var db = new TalentContext())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            industry = db.Industries.Find(id);

                            var attachedroles = db.OfficePositions.Where(o => o.IndustryID == id);
                            //int? unassignedRoleId = db.OfficePositions.Where(o => o.Title == "unassigned").FirstOrDefault().OfficePositionID;
                            //int? unassignedDepartmentId = db.Departments.Where(o => o.DepartmentName == "unassigned").FirstOrDefault().DepartmentID;
                            if (attachedroles.Any())
                            {
                                foreach (var item in attachedroles)
                                {
                                    //assign each offfice position to an empty department denoted by unassigned department
                                    //also 
                                }
                            }

                            db.Industries.Remove(industry);
                            db.SaveChanges();
                            dbTransaction.Commit();
                        }
                        catch
                        {
                            dbTransaction.Rollback();
                        }
                    }

                }
                return RedirectToAction("manage_industries", "Admin");
            }
            catch
            {
                return View("Error");
            }
        }

    }
}
