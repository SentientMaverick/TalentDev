using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalentAcquisition.BusinessLogic.UpdatedDomain;
using TalentAcquisition.DataLayer;

namespace TalentAcquisition.Controllers
{
    public class ComplaintController : Controller
    {
        TalentContext db = new TalentContext();
        #region Views
        [Route("Complaint/Grievance/All")]
        // GET: Complaint
        public ActionResult Index()
        {
            return View(db.GrievanceReports.OrderBy(x=>x.DateCreated));
        }
        [Route("Complaint/Disciplinary/Cases")]
        // GET: Complaint/Details/5
        public ActionResult DisciplinaryCases()
        {
            return View(db.DisciplinaryCases.OrderBy(x=>x.ComplaintDate));
        }
        [Route("Complaint/Grievance/Manage/{id}")]
        public ActionResult ManageGrievance(string id)
        {
            var model = db.GrievanceReports.Where(x=>x.No==id).First();
            return View(model);
        }
        [HttpPost]
        [Route("Complaint/Grievance/Manage/{id}")]
        public ActionResult ManageGrievance(string id,GrievanceReport model)
        {
           var modeldb = db.GrievanceReports.Where(x => x.No == id).First();
            if (model.IsApproved  && !modeldb.IsApproved)
            {
                modeldb.IsApproved = true;
                modeldb.DateApproved = DateTime.UtcNow;
                db.SaveChanges();
            }
            if (model.IsClosed && !modeldb.IsClosed)
            {
                modeldb.IsClosed = true;
                db.SaveChanges();
            }
            //var model = db.GrievanceReports.Where(x => x.No == id).First();
            return View(modeldb);
        }
        // GET: Complaint/Create
        [Route("Complaint/Grievance/Create")]
        public ActionResult Create()
        {
            ViewBag.Employees = db.Employees.Select(p => new { Id = p.ID, Name = p.FirstName+ " "+ p.LastName, Number = p.EmployeeNumber });
            ViewBag.Grievances = db.GrievanceTypes.Select(p => new { p.Code, p.Description });
            ViewBag.EmployeeNumber = new SelectList(db.Employees, "EmployeeNumber", "EmployeeNumber");
            ViewBag.OffenderCode = new SelectList(db.Employees, "EmployeeNumber", "EmployeeNumber");
            ViewBag.GrievanceCode = new SelectList(db.GrievanceTypes, "Code", "Description");
            var model = new GrievanceReport();
            model.No = "GR"+(db.GrievanceReports.Count()+1);
            model.DateCreated = DateTime.UtcNow;
            model.DateApproved = DateTime.UtcNow;
            return View(model);
        }
        [Route("Complaint/Grievance/Create")]
        // POST: Complaint/Create
        [HttpPost]
        public ActionResult Create(GrievanceReport model)
        {
            if (model.EmployeeNumber == "?" || model.OffenderCode == "?")
            {
                ViewBag.Employees = db.Employees.Select(p => new { Id = p.ID, Name = p.FirstName + " " + p.LastName, Number = p.EmployeeNumber });
                ViewBag.Grievances = db.GrievanceTypes.Select(p => new { p.Code, p.Description });
                ViewBag.EmployeeNumber = new SelectList(db.Employees, "EmployeeNumber", "EmployeeNumber");
                ViewBag.OffenderCode = new SelectList(db.Employees, "EmployeeNumber", "EmployeeNumber");
                ViewBag.GrievanceCode = new SelectList(db.GrievanceTypes, "Code", "Description");
                ViewBag.Error = "Please Fill Missing Field(s)";
                if (model.EmployeeNumber == "")
                {
                    ModelState.AddModelError("EmployeeNumber", "Choose a Valid Employee Number");
                }
                if(model.OffenderCode == "")
                {
                    ModelState.AddModelError("EmployeeNumber", "Choose a Valid Employee Number");
                }
                return View(model);
            }
            if (model.GrievanceCode == "?")
            {
                ViewBag.Employees = db.Employees.Select(p => new { Id = p.ID, Name = p.FirstName + " " + p.LastName, Number = p.EmployeeNumber });
                ViewBag.Grievances = db.GrievanceTypes.Select(p => new { p.Code, p.Description });
                ViewBag.EmployeeNumber = new SelectList(db.Employees, "EmployeeNumber", "EmployeeNumber");
                ViewBag.OffenderCode = new SelectList(db.Employees, "EmployeeNumber", "EmployeeNumber");
                ViewBag.GrievanceCode = new SelectList(db.GrievanceTypes, "Code", "Description");
                ViewBag.Error = "Please Fill Missing Field(s)";
                ModelState.AddModelError("GrievanceCode", "Choose a Valid Grievance Type");
                return View(model);
            }
            try
            {
                if (ModelState.IsValid)
                {
                    db.GrievanceReports.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("Grievance/All", "Complaint");
                }
                ViewBag.Error = "Incomplete Form! Please Fill All Fields Appropriately";
                return RedirectToAction("Grievance/Create","Complaint");
            }
            catch
            {
                return View(model);
            }
        }
        // GET: Complaint/Edit/5
        [Route("Complaint/Disciplinary/Manage/{id}")]
        public ActionResult ManageDisciplinaryCases(string id)
        {
            var model= db.DisciplinaryCases.Where(x=>x.Id.ToString()==id).First();
            return View(model);
        }
        [Route("Complaint/Disciplinary/Case")]
        public ActionResult CreateDisciplinaryCase()
        {
            ViewBag.Employees = db.Employees.Select(p => new { Id = p.ID, Name = p.FirstName + " " + p.LastName, Number = p.EmployeeNumber, JobCode=p.OfficePosition.JobID,JobTitle=p.OfficePosition.Title });
            ViewBag.Actions = db.DisciplinaryActions.Select(p => new { p.Code, p.Description });
            ViewBag.IndisciplineTypeCode = new SelectList(db.IndisciplineTypes.ToList(), "Code", "Description");
            ViewBag.DisciplinaryActionCode = new SelectList(db.DisciplinaryActions.ToList(), "Code", "Description");
            var model = new DisciplinaryCase();
            model.ComplaintDate = DateTime.UtcNow;
            model.CaseNumber = "DCR" + (db.DisciplinaryCases.Count() + 1);
            model.ActionStartDate = DateTime.UtcNow;
            model.ActionEndDate = DateTime.UtcNow;
            return View(model);
        }
        [HttpPost]
        [Route("Complaint/Disciplinary/Case")]
        public ActionResult CreateDisciplinaryCase(DisciplinaryCase model)
        {
            if (model.EmployeeNumber == "?" || model.DisciplinaryActionCode == "?" || model.Reasons == "")
            {
                ViewBag.Employees = db.Employees.Select(p => new { Id = p.ID, Name = p.FirstName + " " + p.LastName, Number = p.EmployeeNumber, JobCode = p.OfficePosition.JobID, JobTitle = p.OfficePosition.Title });
                ViewBag.Actions = db.DisciplinaryActions.Select(p => new { p.Code, p.Description });
                ViewBag.IndisciplineTypeCode = new SelectList(db.IndisciplineTypes.ToList(), "Code", "Description");
                ViewBag.DisciplinaryActionCode = new SelectList(db.DisciplinaryActions.ToList(), "Code", "Description");
                ViewBag.Error = "Incomplete Form! Please Fill All Fields Appropriately";
                return View(model);
            }
            if (ModelState.IsValid)
            {
                model.Id = Guid.NewGuid();
                db.DisciplinaryCases.Add(model);
                db.SaveChanges();
                return RedirectToAction("ComplaintManager", "Admin");
            }
            return View(model);
        }
        [Route("Complaint/Grievance/Type")]
        public ActionResult GrievanceType()
        {
            return View();
        }
        [Route("Complaint/Grievance/Action")]
        public ActionResult GrievanceAction()
        {
            return View();
        }
        [Route("Complaint/Indiscipline/Type")]
        public ActionResult IndisciplineType()
        {
            return View();
        }
        [Route("Complaint/Disciplinary/Action")]
        public ActionResult DisciplinaryActionType()
        {
            return View();
        }
        #endregion
        #region JsonActions
        [HttpGet]
        [Route("api/Complaint/GrievanceActions")]
        public JsonResult GetGrievanceActions()
        {
            List<GrievanceAction> actions = db.GrievanceActions.ToList();
            return Json(new { status = "True", actions = actions }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Route("api/Complaint/GrievanceActions")]
        public JsonResult GetGrievanceActions(List<GrievanceAction> actions)
        {
            foreach(var action in actions)
            {
                if (db.GrievanceActions.Where(x => x.Code == action.Code).Any())
                {            
                    db.GrievanceActions.Add(action);
                    db.Entry(action).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    db.GrievanceActions.Add(action);
                }
            }
            db.SaveChanges();
            return Json(new { status = "True", actions = actions }, JsonRequestBehavior.AllowGet);
        }
        [Route("api/Complaint/GrievanceTypes")]
        public JsonResult GetGrievanceTypes()
        {
            List<GrievanceType> types = db.GrievanceTypes.ToList();
            return Json(new { status = "True", types = types }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Route("api/Complaint/GrievanceTypes")]
        public JsonResult GetGrievanceTypes(List<GrievanceType> types)
        {
            foreach (var action in types)
            {
                if (db.GrievanceTypes.Where(x => x.Code == action.Code).Any())
                {
                    db.GrievanceTypes.Add(action);
                    db.Entry(action).State = System.Data.Entity.EntityState.Modified;  
                }
                else
                {
                    db.GrievanceTypes.Add(action);
                }
                db.SaveChanges();
            }
            return Json(new { status = "True", types = types }, JsonRequestBehavior.AllowGet);
        }
        [Route("api/Complaint/DisciplinaryActions")]
        public JsonResult GetDisciplinaryActions()
        {
            List<DisciplinaryAction> actions = db.DisciplinaryActions.ToList();
            return Json(new { status = "True", actions = actions }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Route("api/Complaint/DisciplinaryActions")]
        public JsonResult GetDisciplinaryActions(List<DisciplinaryAction> actions)
        {
            foreach (var action in actions)
            {
                if (db.DisciplinaryActions.Where(x => x.Code == action.Code).Any())
                {
                    db.DisciplinaryActions.Add(action);
                    db.Entry(action).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    db.DisciplinaryActions.Add(action);
                }
                db.SaveChanges();
            }
            return Json(new { status = "True", actions = actions }, JsonRequestBehavior.AllowGet);
        }
        [Route("api/Complaint/IndisciplineTypes")]
        public JsonResult GetIndisciplineTypes()
        {
            List<IndisciplineType> types = db.IndisciplineTypes.ToList();
            return Json(new { status = "True", types=types }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Route("api/Complaint/IndisciplineTypes")]
        public JsonResult GetIndisciplineTypes(List<IndisciplineType> types)
        {
            foreach (var action in types)
            {
                if (db.IndisciplineTypes.Where(x => x.Code == action.Code).Any())
                {
                    db.IndisciplineTypes.Add(action);
                    db.Entry(action).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    db.IndisciplineTypes.Add(action);
                }
                db.SaveChanges();
            }
            return Json(new { status = "True", types = types }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
