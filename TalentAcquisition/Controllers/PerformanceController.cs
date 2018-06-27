using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TalentAcquisition.Controllers
{
    public class PerformanceController : Controller
    {
        [Route("Performance/Appraisal/Personal")]
        // GET: Performance
        public ActionResult SelfAppraisal()
        {
            return View();
        }
        [Route("Performance/Appraisal/Manage")]
        // GET: Performance
        public ActionResult ManageAppraisals()
        {
            return View();
        }
        [Route("Performance/Appraisal/Grade")]
        // GET: Performance
        public ActionResult AppraisalGrade()
        {
            return View();
        }
        [Route("Performance/Appraisal/Category")]
        // GET: Performance
        public ActionResult AppraisalCategory()
        {
            return View();
        }
        
        // GET: Performance/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        [Route("Performace/Appraisal/Template/Create")]
        // GET: Performance/Create
        public ActionResult Create()
        {
            return View();
        }
        [Route("Performace/Appraisal/Template/Create")]
        // POST: Performance/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [Route("Performace/Appraisal/Template/Edit")]
        // GET: Performance/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        // POST: Performance/Edit/5
        [Route("Performace/Appraisal/Template/Edit")]
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [Route("Performace/Appraisal/Template/Delete")]
        // GET: Performance/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        [Route("Performace/Appraisal/Template/Delete")]
        // POST: Performance/Delete/5
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
