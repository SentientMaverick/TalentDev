using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalentAcquisition.BusinessLogic.UpdatedDomain;
using TalentAcquisition.DataLayer;

namespace TalentAcquisition.Controllers
{
    public class TrainingController : Controller
    {
        TalentContext db = new TalentContext();
        [Route("Trainings/All")]
        // GET: Training
        public ActionResult Index()
        {
            return View(db.Trainings.OrderBy(x=>x.ApplicationDate));
        }
        [Route("Trainings/Personal/Schedule")]
        // GET: Training
        public ActionResult Schedule()
        {
            var id= User.Identity.GetUserId();
            var department = db.Employees.Where(x=>x.UserId==id).First().OfficePosition.Department;
            var Schedule = db.Trainings.Where(x => x.EmployeeDepartment == department.DepartmentName);
            return View(Schedule);
        }
        [Route("Trainings/Course")]
        // GET: Training
        public ActionResult Course()
        {
            return View();
        }
        [Route("Trainings/Provider")]
        // GET: Training
        public ActionResult Provider()
        {
            return View();
        }

        [Route("Trainings/Request/Create")]
        // GET: Training
        public ActionResult NewRequest()
        {
            ViewBag.Employees = db.Employees.Select(p => new { Id = p.ID, Name = p.FirstName + " " + p.LastName, Number = p.EmployeeNumber, Department = p.OfficePosition.Department.DepartmentName });
            ViewBag.Courses = db.TrainingCourses.Select(p=>new { p.Code,p.Description});
            ViewBag.TrainingCourseCode = new SelectList(db.TrainingCourses.ToList(), "Code", "Description");
            ViewBag.TrainingProviderCode = new SelectList(db.TrainingProviders.ToList(), "Code", "Name");
            var model = new Training();
            model.ApplicationDate = DateTime.UtcNow;
            model.ApplicationNo = "TRR" + (db.Trainings.Count() + 2);
            model.StartDate = DateTime.UtcNow;
            model.StopDate = DateTime.UtcNow;
            return View(model);
        }
        [Route("Trainings/Request/Create")]
        [HttpPost]
        // GET: Training
        public ActionResult NewRequest(Training training)
        {
            if (training.EmployeeNo ==null || training.TrainingGroupNo == 0 
                || training.ParticipantNo == 0 || training.Duration == 0)
            {
                ViewBag.Error = "Incomplete Form ! Please Fill all Fields";
                ViewBag.Employees = db.Employees.Select(p => new { Id = p.ID, Name = p.FirstName + " " + p.LastName, Number = p.EmployeeNumber, Department = p.OfficePosition.Department.DepartmentName });
                ViewBag.Courses = db.TrainingCourses.Select(p => new { p.Code, p.Description });
                ViewBag.TrainingCourseCode = new SelectList(db.TrainingCourses.ToList(), "Code", "Description");
                ViewBag.TrainingProviderCode = new SelectList(db.TrainingProviders.ToList(), "Code", "Name");
                return View(training);
            }
            if (ModelState.IsValid)
            {
                db.Trainings.Add(training);
                db.SaveChanges();

                return RedirectToAction("Request/Create", "Trainings");
            }
            return View(training);
        }
        [Route("Trainings/Request/Approve/{id}")]
        // GET: Training
        public ActionResult ApproveRequest(string id)
        {
            ViewBag.Employees = db.Employees.Select(p => new { Id = p.ID, Name = p.FirstName + " " + p.LastName, Number = p.EmployeeNumber, Department = p.OfficePosition.Department.DepartmentName });
            ViewBag.Courses = db.TrainingCourses.Select(p => new { p.Code, p.Description });
            ViewBag.TrainingCourseCode = new SelectList(db.TrainingCourses.ToList(), "Code", "Description");
            ViewBag.TrainingProviderCode = new SelectList(db.TrainingProviders.ToList(), "Code", "Name");
            var model = db.Trainings.Where(x=>x.ApplicationNo==id).First();
            //model.ApplicationDate = DateTime.UtcNow;
            //model.ApplicationNo = "TRR" + (db.Trainings.Count() + 2);
            //model.StartDate = DateTime.UtcNow;
            //model.StopDate = DateTime.UtcNow;
            return View(model);
        }
        [HttpPost]
        [Route("Trainings/Request/Approve/{id}")]
        // GET: Training
        public ActionResult ApproveRequest(string id,Training model)
        {
            ViewBag.Employees = db.Employees.Select(p => new { Id = p.ID, Name = p.FirstName + " " + p.LastName, Number = p.EmployeeNumber, Department = p.OfficePosition.Department.DepartmentName });
            ViewBag.Courses = db.TrainingCourses.Select(p => new { p.Code, p.Description });
            ViewBag.TrainingCourseCode = new SelectList(db.TrainingCourses.ToList(), "Code", "Description");
            ViewBag.TrainingProviderCode = new SelectList(db.TrainingProviders.ToList(), "Code", "Name");
            //model.ApplicationDate = DateTime.UtcNow;
            //model.ApplicationNo = "TRR" + (db.Trainings.Count() + 2);
            //model.StartDate = DateTime.UtcNow;
            //model.StopDate = DateTime.UtcNow;
            
            try
            {
                if (ModelState.IsValid)
                 {
                    var model2 = db.Trainings.Where(x => x.ApplicationNo == id).First();
                    model2.Status = "Approved";
                    model2.ApprovedCost = model.ApprovedCost;
                    db.SaveChanges();
                    return RedirectToAction("All","Trainings");
                }
                return View(model);
            }
            catch
            {
                return View(model);
            }
 
        }
        #region JsonActions
        [HttpGet]
        [Route("api/Training/Courses")]
        public JsonResult GetTrainingCourses()
        {
            List<TrainingCourse> courses = db.TrainingCourses.ToList();
            return Json(new { status = "True", courses = courses }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Route("api/Training/Courses")]
        public JsonResult GetTrainingCourses(List<TrainingCourse> courses)
        {
            foreach (var course in courses)
            {
                if (db.TrainingCourses.Where(x => x.Code == course.Code).Any())
                {
                    db.TrainingCourses.Add(course);
                    db.Entry(course).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    db.TrainingCourses.Add(course);
                }
            }
            db.SaveChanges();
            return Json(new { status = "True", courses = courses }, JsonRequestBehavior.AllowGet);
        }
        [Route("api/Training/Providers")]
        public JsonResult GetTrainingProviders()
        {
            List<TrainingProvider> providers = db.TrainingProviders.ToList();
            return Json(new { status = "True", providers = providers }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Route("api/Training/Providers")]
        public JsonResult GetTrainingProviders(List<TrainingProvider> providers)
        {
            foreach (var provider in providers)
            {
                if (db.TrainingProviders.Where(x => x.Code == provider.Code).Any())
                {
                    db.TrainingProviders.Add(provider);
                    db.Entry(provider).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    db.TrainingProviders.Add(provider);
                }
                db.SaveChanges();
            }
            return Json(new { status = "True", providers = providers }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
