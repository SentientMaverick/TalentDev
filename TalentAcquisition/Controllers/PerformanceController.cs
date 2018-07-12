using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalentAcquisition.BusinessLogic.UpdatedDomain;
using TalentAcquisition.DataLayer;
using TalentAcquisition.Helper;
namespace TalentAcquisition.Controllers
{
    public class PerformanceController : Controller
    {
        #region Initializers
        TalentContext db = new TalentContext();
        IWorkflowHelper _workFlowHelper = new WorkFlowHelper();
        #endregion

        #region Views
        [Route("Performance/Appraisal/Personal")]
        // GET: Performance
        public ActionResult SelfAppraisal()
        {
            ViewBag.Employees = db.Employees.Select(p => new { Id = p.ID, Name = p.FirstName + " " + p.LastName, Number = p.EmployeeNumber, Department = p.OfficePosition.Department.DepartmentName,JobTitle=p.OfficePosition.Title,Supervisor=p.OfficePosition.SupervisorID });
            ViewBag.KPICodes = db.AppraisalNonJobKPIs.Select(p=>new { p.Code,p.Description,p.AppraisalClass});
            ViewBag.AppraisalNonJobKPICode = new SelectList(db.AppraisalNonJobKPIs, "Code", "Code");
            ViewBag.AppraisalPeriodCode = new SelectList(db.AppraisalPeriods, "Code", "Description");
            ViewBag.AppraisalTypeCode= new SelectList(db.AppraisalTypes, "Code", "Description");
            var model = new Appraisal();
            model.CreationDate = DateTime.UtcNow;
            model.No ="APPR-"+ String.Format("{0:D5}", db.Appraisals.Count() + 1);
            return View(model);
        }
        [HttpPost]
        [Route("Performance/Appraisal/Personal")]
        // GET: Performance
        public ActionResult SelfAppraisal(Appraisal model,List<JobKPI> items)
        {
            if (!ModelState.IsValid || items==null)
            {
                ViewBag.Employees = db.Employees.Select(p => new { Id = p.ID, Name = p.FirstName + " " + p.LastName, Number = p.EmployeeNumber, Department = p.OfficePosition.Department.DepartmentName, JobTitle = p.OfficePosition.Title, Supervisor = p.OfficePosition.SupervisorID });
                ViewBag.KPICodes = db.AppraisalNonJobKPIs.Select(p => new { p.Code, p.Description, p.AppraisalClass });
                ViewBag.AppraisalNonJobKPICode = new SelectList(db.AppraisalNonJobKPIs, "Code", "Code");
                ViewBag.AppraisalPeriodCode = new SelectList(db.AppraisalPeriods, "Code", "Description");
                ViewBag.AppraisalTypeCode = new SelectList(db.AppraisalTypes, "Code", "Description");
                ViewBag.Error = "Model not Complete.Please Fill all fields Appropriately";
                return View(model);
            }
            //try
            //{
                List<AppraisalKPI> Lines = new List<AppraisalKPI>();
                
                foreach(var item in items)
                {
                    Lines.Add(new AppraisalKPI()
                    {
                        JobKPIId = item.Id.ToString(),
                        JobKPI=item,
                        AppraisalNo=model.No,
                        Appraisal=model
                    });
                    db.JobKPIs.Add(item);
                }
                db.Appraisals.Add(model);
                model.Status = "Open";
                model.Lines = Lines;
                db.SaveChanges();
                return RedirectToAction("Performance","Admin");
            //}
            //catch (Exception ex)
            //{
            //    return View();
            //}
            
        }
        [Route("Performance/Appraisal/Supervisor/Review/{id}")]
        // GET: Performance
        public ActionResult ReviewAppraisal(string id)
        {
            ViewBag.Employees = db.Employees.Select(p => new { Id = p.ID, Name = p.FirstName + " " + p.LastName, Number = p.EmployeeNumber, Department = p.OfficePosition.Department.DepartmentName, JobTitle = p.OfficePosition.Title, Supervisor = p.OfficePosition.SupervisorID });
            ViewBag.KPICodes = db.AppraisalNonJobKPIs.Select(p => new { p.Code, p.Description, p.AppraisalClass });
            ViewBag.AppraisalNonJobKPICode = new SelectList(db.AppraisalNonJobKPIs, "Code", "Code");
            ViewBag.AppraisalPeriodCode = new SelectList(db.AppraisalPeriods, "Code", "Description");
            ViewBag.AppraisalTypeCode = new SelectList(db.AppraisalTypes, "Code", "Description");
            
            var app = db.Appraisals.Where(x => x.No == id);
           // var lines = app.First().Lines.Select(x => x.JobKPI).ToList();
            var lines = db.JobKPIs.Where(x=>x.AppraisalNo==id).Select(x=>new {x.Id,x.KPIGroupCode,x.KPIGroupName,x.Code,x.Description, x.MaxScore,x.EmployeeRating,x.EmployeeRemark,x.SupervisorRating,x.SupervisorRemark, x.Performance }).ToList();
            ViewBag.Lines = lines;
            return View(app.First());
        }
        [HttpPost]
        [Route("Performance/Appraisal/Supervisor/Review/{id}")]
        // GET: Performance
        public ActionResult ReviewAppraisal(string id,Appraisal model, List<JobKPI> items)
        {
            model = db.Appraisals.Where(x => x.No == id).First();
            if (!ModelState.IsValid || items == null)
            {
                ViewBag.Employees = db.Employees.Select(p => new { Id = p.ID, Name = p.FirstName + " " + p.LastName, Number = p.EmployeeNumber, Department = p.OfficePosition.Department.DepartmentName, JobTitle = p.OfficePosition.Title, Supervisor = p.OfficePosition.SupervisorID });
                ViewBag.KPICodes = db.AppraisalNonJobKPIs.Select(p => new { p.Code, p.Description, p.AppraisalClass });
                //ViewBag.AppraisalNonJobKPICode = new SelectList(db.AppraisalNonJobKPIs, "Code", "Code");
                //ViewBag.AppraisalPeriodCode = new SelectList(db.AppraisalPeriods, "Code", "Description");
                //ViewBag.AppraisalTypeCode = new SelectList(db.AppraisalTypes, "Code", "Description");
                ViewBag.Error = "Model not Complete.Please Fill all fields Appropriately";
                return View(model);
            }
            
            var Userid = User.Identity.GetUserId();
            var currentUser = db.Employees.Where(x => x.UserId == Userid).First();
            string usernumber = currentUser.EmployeeNumber.ToString();
            bool y = String.Compare(usernumber,model.AppraisalSupervisor)==0;
            if (!y)
            {
                ViewBag.Employees = db.Employees.Select(p => new { Id = p.ID, Name = p.FirstName + " " + p.LastName, Number = p.EmployeeNumber, Department = p.OfficePosition.Department.DepartmentName, JobTitle = p.OfficePosition.Title, Supervisor = p.OfficePosition.SupervisorID });
                ViewBag.KPICodes = db.AppraisalNonJobKPIs.Select(p => new { p.Code, p.Description, p.AppraisalClass });
                //ViewBag.AppraisalNonJobKPICode = new SelectList(db.AppraisalNonJobKPIs, "Code", "Code");
                //ViewBag.AppraisalPeriodCode = new SelectList(db.AppraisalPeriods, "Code", "Description");
                //ViewBag.AppraisalTypeCode = new SelectList(db.AppraisalTypes, "Code", "Description");
                ViewBag.Error = "You are not the Supervisor! You donot have the Right to modify this entry";
                return View(model);
            }
            model.Status = "Approved by Supervisor";
            foreach(var item in items)
            {
                item.AppraisalNo = id;
                db.JobKPIs.Add(item);
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
            }
            return RedirectToAction("Performance", "Admin");
        }
        [Route("Performance/Appraisal/Approve/{id}")]
        // GET: Performance
        public ActionResult ApproveAppraisal(string id)
        {
            var app = db.Appraisals.Where(x => x.No == id);
            return View(app);
        }
        [Route("Performance/Appraisal/Pending")]
        // GET: Performance
        public ActionResult PendingAppraisals()
        {
            ViewBag.ViewControl = "Pending";
            return View(db.Appraisals);
        }
        [Route("Performance/Appraisal/Manage")]
        // GET: Performance
        public ActionResult ManageAppraisals()
        {
            ViewBag.ViewControl = "All";
            return View("PendingAppraisals", db.Appraisals);
        }
        [Route("Performance/Appraisal/Approved")]
        // GET: Performance
        public ActionResult ApprovedAppraisals()
        {
            ViewBag.ViewControl = "Approved";
            return View("PendingAppraisals", db.Appraisals);
        }
        [Route("Performance/Appraisal/Type")]
        // GET: Performance
        public ActionResult AppraisalType()
        {
            return View();
        }
        [Route("Performance/Appraisal/Grade")]
        // GET: Performance
        public ActionResult AppraisalGrade()
        {
            return View();
        }
        [Route("Performance/Appraisal/Period")]
        // GET: Performance
        public ActionResult AppraisalPeriod()
        {
            return View();
        }
        [Route("Performance/Appraisal/Category")]
        // GET: Performance
        public ActionResult AppraisalCategory()
        {
            return View();
        }
        [Route("Performance/Appraisal/Templates")]
        // GET: Performance/Create
        public ActionResult Templates()
        {
            return View(db.AppraisalNonJobKPIs);
        }
        [Route("Performance/Appraisal/Template/Create")]
        // GET: Performance/Create
        public ActionResult Create()
        {
            return View();
        }
        [Route("Performance/Appraisal/Template/Create")]
        // POST: Performance/Create
        [HttpPost]
        public ActionResult Create(AppraisalNonJobKPI model,List<NonJobKPI> items)
        {
            //try
            //{
                if (ModelState.IsValid)
                {
                    foreach(var item in items)
                    {
                    //item.Code = model.Code;
                    //item.AppraisalClass = model.AppraisalClass;
                        item.Id = Guid.NewGuid();
                    }
                    model.Lines = items;
                    db.AppraisalNonJobKPIs.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("Performance", "Admin");
            }
                return View(model);
            //}
            //catch
            //{
            //    return View();
            //}
        }
        [Route("Performance/Appraisal/Template/Edit/{id}")]
        // GET: Performance/Edit/5
        public ActionResult Edit(string id)
        {
            var model = db.AppraisalNonJobKPIs.Where(x => x.Code == id);
            ViewBag.Lines = model.First().Lines.Select(p=>new { p.Id,p.MaxScore,p.Code,p.Description,p.AppraisalNonJobKPICode,p.AppraisalClass}).ToList();
            return View(model.First());
        }
        // POST: Performance/Edit/5
        [Route("Performance/Appraisal/Template/Edit/{id}")]
        [HttpPost]
        public ActionResult Edit(string id, AppraisalNonJobKPI model, List<NonJobKPI> items)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.AppraisalNonJobKPIs.Add(model);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Performance","Admin");
            }
            catch
            {
                return View();
            }
        }
        [Route("Performance/Appraisal/Template/Delete")]
        // GET: Performance/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        [Route("Performance/Appraisal/Template/Delete")]
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
        #endregion

        #region JsonActions
        [HttpGet]
        [Route("api/Performance/Appraisal/Grades")]
        public JsonResult GetAppraisalGrades()
        {
            List<AppraisalGrade> grades = new List<AppraisalGrade>();
            grades = db.AppraisalGrades.OrderByDescending(x=>x.Value).ToList();
            return Json(new { status = "True", grades = grades }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Route("api/Performance/Appraisal/Grades")]
        public JsonResult GetAppraisalGrades(List<AppraisalGrade> grades)
        {
            foreach (var grade in grades)
            {
                if (db.AppraisalGrades.Where(x => x.Name == grade.Name).Any())
                {
                    db.AppraisalGrades.Add(grade);
                    db.Entry(grade).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    db.AppraisalGrades.Add(grade);
                }
            }
            db.SaveChanges();
            return Json(new { status = "True", grades = grades }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [Route("api/Performance/Appraisal/Categories")]
        public JsonResult GetAppraisalCategories()
        {
            List<AppraisalCategory> categories = db.AppraisalCategories.ToList();
            return Json(new { status = "True", categories = categories }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Route("api/Performance/Appraisal/Categories")]
        public JsonResult GetAppraisalCategories(List<AppraisalCategory> categories)
        {
            foreach (var category in categories)
            {
                if (db.AppraisalCategories.Where(x => x.Code == category.Code).Any())
                {
                    db.AppraisalCategories.Add(category);
                    db.Entry(category).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    db.AppraisalCategories.Add(category);
                }
            }
            db.SaveChanges();
            return Json(new { status = "True", categories = categories }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [Route("api/Performance/Appraisal/Types")]
        public JsonResult GetAppraisalTypes()
        {
            List<AppraisalType> types = db.AppraisalTypes.ToList();
            return Json(new { status = "True", types = types }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Route("api/Performance/Appraisal/Types")]
        public JsonResult GetAppraisalTypes(List<AppraisalType> types)
        {
            foreach (var type in types)
            {
                if (db.AppraisalTypes.Where(x => x.Code == type.Code).Any())
                {
                    db.AppraisalTypes.Add(type);
                    db.Entry(type).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    db.AppraisalTypes.Add(type);
                }
            }
            db.SaveChanges();
            return Json(new { status = "True", types = types }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [Route("api/Performance/Appraisal/Periods")]
        public JsonResult GetAppraisalPeriods()
        {
            List<AppraisalPeriod> periods = db.AppraisalPeriods.ToList();
            return Json(new { status = "True", periods = periods }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Route("api/Performance/Appraisal/Periods")]
        public JsonResult GetAppraisalPeriods(List<AppraisalPeriod> periods)
        {
            foreach (var period in periods)
            {
                if (db.AppraisalPeriods.Where(x => x.Code == period.Code).Any())
                {
                    db.AppraisalPeriods.Add(period);
                    db.Entry(period).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    db.AppraisalPeriods.Add(period);
                }
            }
            db.SaveChanges();
            return Json(new { status = "True", periods = periods }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [Route("api/GetAppraisalKPIFromTemplate")]
        public JsonResult GetAppraisalKPIFromTemplate(string id,string appraisalno)
        {
            List<NonJobKPI> TemplateLines = db.NonJobKPIs.Where(x => x.AppraisalNonJobKPICode == id).ToList();
            List<JobKPI> JobKPILines = new List<JobKPI>();
            JobKPILines = ConvertToKPI(TemplateLines,appraisalno);
            return Json(new { status = "True", lines = JobKPILines }, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region HelperMethods
        private List<JobKPI> ConvertToKPI(List<NonJobKPI> templateLines, string appraisalno)
        {
            List<JobKPI> JobKPILines = new List<JobKPI>();
            if (templateLines == null)
            {
                throw new ArgumentNullException();
            }
            foreach(var item in templateLines)
            {

                var line = new JobKPI
                {
                    Id = Guid.NewGuid(),
                    AppraisalNo = appraisalno,
                    Code = item.Code,
                    Description = item.Description,
                    KPIGroupCode=item.AppraisalClass,
                    KPIGroupName=item.AppraisalClass,
                    MaxScore=item.MaxScore
                };
                JobKPILines.Add(line);
            }
            return JobKPILines;
        }
        #endregion
    }
}
