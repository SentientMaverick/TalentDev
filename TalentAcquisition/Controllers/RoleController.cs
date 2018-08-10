using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TalentAcquisition.Core.Domain;
using TalentAcquisition.Filters;
using TalentAcquisition.DataLayer;
using TalentAcquisition.BusinessLogic.UpdatedDomain;

namespace TalentAcquisition.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        TalentContext db = new TalentContext();
        
        #region JobRequirementPartials
        public ActionResult _GetRequirements()
        {
            var requirements = new List<BusinessLogic.UpdatedDomain.JobRequirement>();
            //for(int i = 0; i < 10; i++)
            //{
            //    requirements.Add(new BusinessLogic.UpdatedDomain.JobRequirement());
            //}
            using (var db = new TalentContext())
            {
                ViewBag.Requirements = db.JobQualifications.ToList();
                ViewBag.RequirementCode = db.JobQualifications.Select(o => o.QualificationType).Distinct().ToList();
            }
            return PartialView(requirements);
        }
        public ActionResult _GetRequirementsForEditing(List<BusinessLogic.UpdatedDomain.JobRequirement> requirements)
        {
            using (var db = new TalentContext())
            {
                ViewBag.Requirements = db.JobQualifications.ToList();
                ViewBag.RequirementCode = db.JobQualifications.Select(o => o.QualificationType).Distinct().ToList();
            }
            return PartialView("_GetRequirements", requirements);
        }
        public JsonResult QualificationFilter()
        {

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _CreateNewRequirement(int id)
        {
            try
            {
                ViewBag.ID = id;
                using (var db = new TalentContext())
                {
                    ViewBag.Requirements = db.JobQualifications.ToList();
                    ViewBag.RequirementCode = db.JobQualifications.Select(o => o.QualificationType).Distinct().ToList();
                }
                var requirement = new List<BusinessLogic.UpdatedDomain.JobRequirement>();
                for (int i = 0; i <= id; i++)
                {
                    requirement.Add(new BusinessLogic.UpdatedDomain.JobRequirement());
                }
                return PartialView(requirement);
            }
            catch
            {
                return View();
            }
        }
        public JsonResult _CreateNewRequirementJson(int id)
        {
            ViewBag.ID = id;
            using (var db = new TalentContext())
            {
                ViewBag.Requirements = db.JobQualifications.ToList();
                ViewBag.RequirementCode = db.JobQualifications.Select(o => o.QualificationType).Distinct().ToList();
            }
            // var requirement= new List<BusinessLogic.UpdatedDomain.JobRequirement>();
            return Json(PartialView(), JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region Views
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
                ViewBag.Branches = db.Branches.ToList();
                ViewBag.Positions = db.OfficePositions.ToList();
                var list = db.JobQualifications.ToList();
                ViewBag.Codes=list.GroupBy(o=>o.QualificationCode).Select(x=>x.FirstOrDefault()) ;
                ViewBag.Types = list;
            }
            return View();
        }
        // POST: Role/Create
        [ValidateAntiForgeryToken]
        [HttpPost]
        [Route("Admin/Job/Create")]
        [Route("Admin/manage_officeposition/Create")]
        public ActionResult Create(OfficePosition position, List<JobRequirement> Line, IEnumerable<BusinessLogic.UpdatedDomain.ApplicantEvaluationMetrics> applicantmetrics)
        {
            // position.IsAvailable = false;
            position.DateCreated = DateTime.Now;
            position.Reqirements = "";
            if (!ModelState.IsValid)
            {
                using (var db = new TalentContext())
                {
                    ViewBag.Departments = db.Departments.ToList();
                    ViewBag.Industries = db.Industries.ToList();
                    ViewBag.Branches = db.Branches.ToList();
                    ViewBag.Positions = db.OfficePositions.ToList();
                    ViewBag.Error = "Form was not Submitted. Please fill all Mandatory Fields";
                }
                return View(position);
            }
           try
            {
                using (var db = new TalentContext())
                {

                    db.OfficePositions.Add(position);
                    db.SaveChanges();
                    var quals = db.JobQualifications.ToList();
                    foreach (var item in Line)
                    {
                        item.QualificationID = quals.Where(x => x.QualificationCode == item.QualificationCode &&
                          x.QualificationType == item.QualificationType).FirstOrDefault().ID;
                        item.OfficePositionID = position.OfficePositionID;
                        item.OfficePosition = position;
                        db.JobRequirements.Add(item);
                    }
                    db.SaveChanges();
                    //Insert Logic to prevent adding the same role title 2 or more times to a single department 
                }
               
                return RedirectToAction("jobmanager", "Admin");
            }
            catch
            {
                return View("Error");
            }
        }
        // GET: Role/Details/5
        [Route("Admin/manage_officeposition/Details/{id:int}", Name = "RoleDetails")]
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
        [Route("Admin/Job/{id:int}")]
        [Route("Admin/Job/Edit/{id:int}")]
        // [Route("Admin/Edit/{id:int}")]
        [Route("Admin/manage_officeposition/Edit/{id}")]
        public ActionResult Edit(int id)
        {
            try
            {
                var position = new OfficePosition();
                using (var db = new TalentContext())
                {
                    position = db.OfficePositions.Include("JobRequirements").Where(x => x.OfficePositionID == id).FirstOrDefault();
                    ViewBag.Departments = db.Departments.ToList();
                    ViewBag.Industries = db.Industries.ToList();
                    ViewBag.Branches = db.Branches.ToList();
                    ViewBag.Positions = db.OfficePositions.ToList();
                    ViewBag.JobRequirements = position.JobRequirements.Select(x=>new {x.ID,x.Mandatory,x.ScoreID,
                        x.NeedCode,x.DesiredScore,x.OfficePositionID,x.QualificationCode,
                        x.QualificationDescription,x.QualificationType,x.StageCode,x.Priority }).ToList();
                    ViewBag.ApplicantEvalMetrics = db.ApplicantEvaluationMetrics
                        .Where(x => x.OfficePositionID == id)
                        .Select(y=>new {y.ID,y.OfficePositionID,y.EvaluationCode,y.EvaluationDescription,y.MaximumScore }).ToList();
                    var list = db.JobQualifications.ToList();
                    ViewBag.Codes = list.GroupBy(o => o.QualificationCode).Select(x => x.FirstOrDefault());
                    ViewBag.Types = list;
                    return View(position);
                }
            }
            catch
            {
                return View("Error");
            }
        }
        // POST: Role/Edit/5
        [Route("Admin/Job/{id:int}")]
        [Route("Admin/Job/Edit/{id:int}")]
        [Route("Admin/manage_officeposition/Edit/{id}")]
        [HttpPost]
        public ActionResult Edit(int id, OfficePosition position, List<JobRequirement> Line, List<ApplicantEvaluationMetrics> Evals, IEnumerable<JobRequirement> requirements, IEnumerable<ApplicantEvaluationMetrics> applicantmetrics)
        {
            position.DateCreated = DateTime.Now;
            var dbposition = new OfficePosition();
            using (var db = new TalentContext())
            {
                ViewBag.Departments = db.Departments.ToList();
                ViewBag.Industries = db.Industries.ToList();
                ViewBag.Branches = db.Branches.ToList();
                ViewBag.Positions = db.OfficePositions.ToList();
                if (requirements !=null)
                {
                    ViewBag.SelectedRequirements = requirements.ToList();
                }
                else
                {
                    ViewBag.SelectedRequirements = new List<JobRequirement>();
                }
                
            }
            var status = UpdateApplicantMetrics(applicantmetrics);

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Title", "Error");
                ViewBag.ErrorMessage = "Error";
                return View(position);
                
            }
            try
            {
                using (var db = new TalentContext())
                {
                        db.OfficePositions.Add(position);
                        db.Entry(position).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        var action = UpdateJobRequirements(Line, position.OfficePositionID);
                        var action2 = UpdateApplicantEvaluationRequirements(Evals, position.OfficePositionID);
                        db.SaveChanges();
                       // Insert Logic to prevent adding the same role title 2 or more times to a single department
                }
                ViewBag.Message = "Changes Made Successfully";
                return RedirectToAction("Job/Edit/" + position.OfficePositionID, "Admin");
            }
            catch
            {
                return View("Error");
            }
        }

        private bool UpdateApplicantEvaluationRequirements(List<ApplicantEvaluationMetrics> evals, int officePositionID)
        {
            bool status = false;
            try
            {
                if (evals != null)
                {
                    db.OfficePositions.Find(officePositionID).ApplicantMetrics.Clear();
                    foreach(var eval in evals)
                    {
                        eval.OfficePositionID = officePositionID;
                        db.ApplicantEvaluationMetrics.Add(eval);
                    }
                    db.SaveChanges();
                    status = true;
                }
            }
            catch
            {
                status = false;
            }
            return status;
        }

        [Route("Admin/Job/Delete/{id:int}")]
        // GET: Role/Delete/5
        public ActionResult Delete(int id)
        {
            var dbposition = new OfficePosition();
            using (var db = new TalentContext())
            {
                dbposition = db.OfficePositions.Include("JobRequirements").Where(x => x.OfficePositionID == id).FirstOrDefault();
                db.JobRequirements.RemoveRange(dbposition.JobRequirements);
                db.SaveChanges();
                db.OfficePositions.Remove(dbposition);
                db.SaveChanges();
            }

            return RedirectToAction("Jobmanager", "Admin");
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
        #endregion
        #region ApplicantMetricsPartials
        public ActionResult _NewApplicantMetric(int id, int jobid)
        {
            var evaluation = new List<ApplicantEvaluationMetrics>();
            ViewBag.ID = id;
            for (int i = 0; i <= id; i++)
            {
                evaluation.Add(new ApplicantEvaluationMetrics() {ID=0, OfficePositionID = jobid });
            }
            return PartialView(evaluation);
        }
        public ActionResult _GetApplicantMetrics(int jobid)
        {
            var applicantmetrics = new List<ApplicantEvaluationMetrics>();
            applicantmetrics = db.ApplicantEvaluationMetrics.Where(x => x.OfficePositionID == jobid).ToList();
            ViewBag.JobID = jobid;
            return PartialView(applicantmetrics);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult _SubmitApplicantMetricsForm(IEnumerable<ApplicantEvaluationMetrics> submittedmetrics)
        {
            bool action = false;
            if (submittedmetrics != null && submittedmetrics.Count() >= 0)
            {
                var categories = submittedmetrics.ToList();
                foreach (ApplicantEvaluationMetrics metrics in submittedmetrics)
                {
                    if (ModelState.IsValid)
                    {
                        if (metrics.ID == 0)
                        {
                            db.ApplicantEvaluationMetrics.Add(metrics);
                        }
                        else
                        {
                            db.Entry(metrics).State = System.Data.Entity.EntityState.Modified;
                        }
                        db.SaveChanges();
                    }
                    action = true;
                }
            }

            return Json(action, JsonRequestBehavior.AllowGet);
        }
        public JsonResult _DeleteApplicantMetric(int id)
        {
            var action = false;
            //var evaluation = new Evaluation();
            using (var db = new TalentContext())
            {
                var metrics = db.ApplicantEvaluationMetrics.Find(id);
                if (metrics != null)
                {
                    db.ApplicantEvaluationMetrics.Remove(metrics);
                    db.SaveChanges();
                }
                action = true;
            }
            return Json(action, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region HelperMethods
        public bool UpdateApplicantMetrics(IEnumerable<ApplicantEvaluationMetrics> applicantmetrics)
        {
            bool status = false;
            try
            {
                if (applicantmetrics != null && applicantmetrics.Count() >= 0)
                {
                    var validmetrics = applicantmetrics.Where(x => x.MaximumScore != 0 && x.EvaluationCode != null && x.EvaluationDescription != null).ToList();
                    foreach (ApplicantEvaluationMetrics metrics in validmetrics)
                    {
                        if (metrics.ID == 0)
                        {
                            db.ApplicantEvaluationMetrics.Add(metrics);
                        }
                        else
                        {        
                            var met = db.ApplicantEvaluationMetrics.Find(metrics.ID);
                            if (met == null)
                            {
                                var count = db.ApplicantEvaluationMetrics.Where(x => x.OfficePositionID == metrics.OfficePositionID && x.EvaluationCode == metrics.EvaluationCode).Count();
                                if (count < 1)
                                {
                                    metrics.ID = 0;
                                    db.ApplicantEvaluationMetrics.Add(metrics);
                                }                  
                            }
                            else
                            {
                                //var count = db.ApplicantEvaluationMetrics.Where(x => x.OfficePositionID == metrics.ID && x.EvaluationCode == metrics.EvaluationCode).Count();
                                //if (count < 1)
                                //{
                                //    db.ApplicantEvaluationMetrics.Add(metrics);
                                //}  
                                met.MaximumScore = metrics.MaximumScore;
                                met.EvaluationCode = metrics.EvaluationCode;
                                met.EvaluationDescription = metrics.EvaluationDescription;
                                //db.ApplicantEvaluationMetrics.Add(metrics);
                                db.Entry(met).State = System.Data.Entity.EntityState.Modified;
                            }     
                        }
                        db.SaveChanges();
                    }
                    status = true;
                }
            }
            catch
            {
                status = false;
            }
            return status;
        }
        public bool UpdateJobRequirements(IEnumerable<JobRequirement> requirements,int officepositionid)
        {
            bool status = false;
            try
            {
                if (requirements != null)
                {
                    List<BusinessLogic.UpdatedDomain.JobRequirement> selectedrequirements = requirements.Where(x => x.QualificationCode != null && x.QualificationType != null).ToList();

                    foreach (var requirement in selectedrequirements)
                    {
                        if (requirement.ID == 0)
                        {
                            requirement.OfficePositionID = officepositionid;
                            // requirement.OfficePosition = position; 
                            db.JobRequirements.Add(requirement);
                            //db.Entry(requirement).State = System.Data.Entity.EntityState.Added;
                        }
                        else
                        {
                            db.JobRequirements.Add(requirement);
                            db.Entry(requirement).State = System.Data.Entity.EntityState.Modified;
                        }
                        db.SaveChanges();
                    }
                    var list = db.OfficePositions.Include("JobRequirements").Where(x => x.OfficePositionID == officepositionid)
                        .FirstOrDefault().JobRequirements;
                    var toberemoved = new List<BusinessLogic.UpdatedDomain.JobRequirement>();
                    foreach (var item in list)
                    {
                        if (!selectedrequirements.Where(x => x.ID == item.ID).Any())
                        {
                            toberemoved.Add(item);
                            //db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                        }
                    }
                    db.JobRequirements.RemoveRange(toberemoved);
                    db.SaveChanges();
                }
                status = true;
            }
            catch
            {
                status = false;
            }
            return status;
        }
        #endregion
        public ActionResult MyError()
        {
            return View("Error");
        }
    }
}
