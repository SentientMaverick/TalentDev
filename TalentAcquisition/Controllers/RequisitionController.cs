using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TalentAcquisition.BusinessLogic.Domain;
using TalentAcquisition.Core.Domain;
using TalentAcquisition.DataLayer;
using TalentAcquisition.Models.Core;

namespace TalentAcquisition.Controllers
{
    public class RequisitionController : Controller
    {
        // GET: Requisition
        [Route("Admin/Requisitions")]
        public ActionResult Index()
        {
            return View();
        }
        [Route("Admin/Requisitions/Monitor")]
        public ActionResult Monitor()
        {
            return View();
        }
        [Route("Admin/Requisition/Create")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Route("Admin/Requisition/Create")]
        public ActionResult Create(JobRequisition requisition, List<CheckModel> checks)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Couldn't Create Requisition";
                return View(requisition);
            }
            try
            {
                List<Skill> selectedskills = checks.Where(x => x.Checked == true)
                                        .Select(o => new Skill { ID = o.Id, Name = o.Name }).ToList();
                var selectedint = selectedskills.Select(o => o.ID).ToList();
                // var selectedint = new List<int>() {1,2,3 };
                
                requisition.Status = JobRequisition.JobRequisitionStatus.Created;
                requisition.PublishedDate = DateTime.Now;
                using (var db = new TalentContext())
                {
                    var skills = db.Skills
                               .Where(x => selectedint.Contains(x.ID))
                               .ToList();
                    db.JobRequisitions.Add(requisition);
                    db.SaveChanges();

                    requisition.Skills.Union(skills);
                    requisition.Skills.Intersect(skills);
                    requisition.Skills = skills;

                    db.SaveChanges();
                }
                ViewBag.Message = "Successfully Created Requisition";
                return View();
            }
            catch
            {
                return View("Error");
            }
        }
        [Route("Admin/Requisition/{id:int}/{details}", Name = "RequisitionLink")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TalentContext db = new TalentContext();
            JobRequisition jobRequisition = db.JobRequisitions.Include("Skills").Where(x=>x.JobRequisitionID==id).First();
            if (jobRequisition == null)
            {
                return HttpNotFound();
            }
            ViewBag.RequisitionID = jobRequisition.JobRequisitionID;
            return View(jobRequisition);
        }
       // [HttpGet]
        [Route("Admin/matchskill/{id:int}", Name = "MatchSkill")]
        public ActionResult MatchSkill(int? id,string details,string filter)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                TalentContext db = new TalentContext();
                JobRequisition jobRequisition = db.JobRequisitions.Include("Skills").Where(x => x.JobRequisitionID == id).First();
                if (jobRequisition == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Filter = filter;
                ViewBag.RequisitionID = jobRequisition.JobRequisitionID;
                return View(jobRequisition);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
        [Route("Admin/publish/{id:int}/{details}/", Name = "PublishLink")]
        public ActionResult Publish(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TalentContext db = new TalentContext();
            JobRequisition jobRequisition = db.JobRequisitions.Include("Skills").Where(x => x.JobRequisitionID == id).First();
            if (jobRequisition == null)
            {
                return HttpNotFound();
            }
            ViewBag.RequisitionID = jobRequisition.JobRequisitionID;
            return View(jobRequisition);
        }
        [Route("Admin/publish/{id:int}/{details}")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Publish(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            TalentContext db = new TalentContext();
            JobRequisition jobRequisition = db.JobRequisitions.Find(id);
            if (jobRequisition == null)
            {
                return HttpNotFound();
            }
            jobRequisition.Status = JobRequisition.JobRequisitionStatus.Posted;
            db.Entry(jobRequisition).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            ViewBag.Message = "Job Successfully Published";
            return RedirectToAction("Requisitions", "Admin");
        }
        [Route("Admin/Edit/{id:int}/{details}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TalentContext db = new TalentContext();
            JobRequisition jobRequisition = db.JobRequisitions.Include("Skills").Where(x => x.JobRequisitionID == id).First();
            if (jobRequisition == null)
            {
                return HttpNotFound();
            }
            ViewBag.RequisitionID = jobRequisition.JobRequisitionID;
            ViewBag.Departments = db.Departments.ToList();
            ViewBag.Industries = db.Departments.ToList();
            return View(jobRequisition);
        }
        [Route("Admin/Edit/{id:int}/{details}", Name = "Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, JobRequisition requisition, List<CheckModel> checks)
        {
            ViewBag.Message = "Changes Were Not Saved. Pls Try Again";
            if (!ModelState.IsValid)
            {
                return View(requisition);
            }
            try
            {
                List<Skill> selectedskills = checks.Where(x => x.Checked == true)
                                            .Select(o => new Skill { ID = o.Id, Name = o.Name }).ToList();
            var selectedint = selectedskills.Select(o => o.ID).ToList();
            using (var db = new TalentContext())
                {
                    JobRequisition jobRequisition = db.JobRequisitions.Find(id);
                    
                    jobRequisition.JobResponsibilities = requisition.JobResponsibilities;
                    jobRequisition.EducationalRequirements = requisition.EducationalRequirements;
                    jobRequisition.AgeLimit = requisition.AgeLimit;
                    jobRequisition.JobDescription = jobRequisition.JobDescription;
                    jobRequisition.Location = requisition.Location;
                    jobRequisition.ClosingDate = requisition.ClosingDate;
                    jobRequisition.StartDate = requisition.StartDate;
                    jobRequisition.YearsOfExperience = requisition.YearsOfExperience;
                    jobRequisition.NoOfPositionsAvailable = requisition.NoOfPositionsAvailable;
                    jobRequisition.Status = JobRequisition.JobRequisitionStatus.Created;
                    jobRequisition.PublishedDate = DateTime.Now;
                    db.Entry(jobRequisition).State = System.Data.Entity.EntityState.Modified;

                    var skills = db.Skills
                                   .Where(x => selectedint.Contains(x.ID))
                                   .ToList();
                   // db.JobRequisitions.Add(requisition);
                    db.SaveChanges();
                    requisition.Skills.Union(skills);
                    requisition.Skills.Intersect(skills);
                    requisition.Skills = skills;

                    db.SaveChanges();
                    ViewBag.Message = "Changes Were succesfully Saved";
                    return RedirectToAction("Requisitions","Admin");
                    //return View(requisition);
                }
           }
            catch
            {
                //return;
               return View("Error");
            }

        }
        [Route("Admin/reject/{id:int}/{details}")]
        public ActionResult Reject(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TalentContext db = new TalentContext();
            JobRequisition jobRequisition = db.JobRequisitions.Find(id);
            if (jobRequisition == null)
            {
                return HttpNotFound();
            }
            ViewBag.RequisitionID = jobRequisition.JobRequisitionID;
            return View(jobRequisition);
        }
        [HttpPost]
        [Route("Admin/reject/{id:int}/{details}")]
        public ActionResult Reject(int? id, JobRequisition requisition)
        {
            JobRequisition jobRequisition;
            using (var db = new TalentContext())
            {
                jobRequisition = db.JobRequisitions.Find(id);
                jobRequisition.RejectionNote = requisition.RejectionNote;
                jobRequisition.Status = JobRequisition.JobRequisitionStatus.Rejected;
                db.Entry(jobRequisition).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                ViewBag.Message = "Action Successful";
            }
            return View();
        }
        [Route("Admin/close/{id:int}/{details}/", Name = "Close")]
        public ActionResult Close(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TalentContext db = new TalentContext();
            JobRequisition jobRequisition = db.JobRequisitions.Find(id);
            if (jobRequisition == null)
            {
                return HttpNotFound();
            }
            ViewBag.RequisitionID = jobRequisition.JobRequisitionID;
            return View(jobRequisition);
        }
        [HttpPost]
        [Route("Admin/close/{id:int}/{details}/")]
        public ActionResult Close(int id)
        {
            try
            {
                using (var db = new TalentContext())
                {
                    var job = db.JobRequisitions.Find(id);
                    job.Status = JobRequisition.JobRequisitionStatus.Completed;
                    db.SaveChanges();
                }
                //return View();
                return RedirectToAction("Requisitions", "Admin");
            }
            catch
            {
                return View("Error");
            }

        }

        [ChildActionOnly]
        public ActionResult _GetNewApplications(int id)
        {
            var activeapplications = new List<JobApplication>();
            //int id = (int)ViewBag.RequisitionID;
            var db = new TalentContext();
            //using (var db = new TalentContext())
            //{
            activeapplications = db.JobApplications.Where(job => job.JobRequisitionID == id && job.ApplicationStatus == ApplicationStatus.Applied).ToList();
            //}
            return PartialView(activeapplications);
        }
        [ChildActionOnly]
        public ActionResult _GetClosedApplications(int id)
        {
            var activeapplications = new List<JobApplication>();
            var db = new TalentContext();
            activeapplications = db.JobApplications.Where(job => job.JobRequisitionID == id).
                    Where(job => job.ApplicationStatus == ApplicationStatus.Canceled).ToList();
            return PartialView("_GetNewApplications", activeapplications);
        }

        public ActionResult _GetMatchingApplications(int id)
        {
            var activeapplications = new List<JobApplication>();
            activeapplications = MatchingTheCandidatesByApplicationAlgorithm(id);
            return PartialView("_GetNewApplications", activeapplications);
        }
        public ActionResult _GetMatchingApplicants(int id)
        {
            var activeapplicants = new List<JobSeeker>();
            activeapplicants = MatchingTheCandidatesAlgorithm(id);
            return PartialView(activeapplicants);
        }

        [ChildActionOnly]
        public ActionResult _GetScreenedApplications(int id)
        {
            var activeapplications = new List<JobApplication>();
            var db = new TalentContext();
            activeapplications = db.JobApplications.Where(job => job.JobRequisitionID == id).
                    Where(job => (job.ApplicationStatus == ApplicationStatus.Screened)
                    || (job.ApplicationStatus == ApplicationStatus.Interview)
                    || (job.ApplicationStatus == ApplicationStatus.JobOffer)).ToList();
            return PartialView("_GetNewApplications", activeapplications);
        }
        [ChildActionOnly]
        [Route("Admin/Requisitions/_GetActiveRequisitions")]
        public ActionResult _GetActiveRequisitions()
        {
            var activerequisitions = new List<JobRequisition>();
            using (var db = new TalentContext())
            {
                activerequisitions = db.JobRequisitions.Where(job => job.Status == JobRequisition.JobRequisitionStatus.Posted).ToList();
            }
            return PartialView("_GetIncomingRequisitions", activerequisitions);
        }
        [ChildActionOnly]
        public ActionResult _GetIncomingRequisitions()
        {
            var activerequisitions = new List<JobRequisition>();
            using (var db = new TalentContext())
            {
                activerequisitions = db.JobRequisitions.Where(job => job.Status == JobRequisition.JobRequisitionStatus.Created).ToList();
            }
            return PartialView(activerequisitions);
        }
        //[ChildActionOnly]
        public ActionResult _GetRejectedRequisitions()
        {
            var rejectrequisitions = new List<JobRequisition>();
            using (var db = new TalentContext())
            {
                rejectrequisitions = db.JobRequisitions.Where(job => job.Status == JobRequisition.JobRequisitionStatus.Rejected).ToList();
            }
            return PartialView("_GetIncomingRequisitions", rejectrequisitions);
        }
        public ActionResult _GetRequisition(int? id)
        {
            var requisition = new JobRequisition();
            using (var db = new TalentContext())
            {
                requisition = db.JobRequisitions.Find(id);
            }
            return PartialView(requisition);
        }
        [ChildActionOnly]
        public ActionResult _GetDetailsFromRole()
        {
            using (var db = new TalentContext())
            {
                ViewBag.Departments = db.Departments.ToList();
                ViewBag.Positions = Enumerable.Empty<SelectListItem>();
                
            }
            return PartialView();
        }
        public ActionResult _GetSkills(int? id)
        {
            var user = new JobRequisition();
            var sk = new List<CheckModel>();
            using (var db = new TalentContext())
            {
                if (id == null)
                {
                    ViewBag.Skills = db.Skills.ToList();
                    var list = new List<CheckModel>();
                    foreach (var item in (List<Skill>)ViewBag.Skills)
                    {
                            list.Add(new CheckModel { Id = item.ID, Name = item.Name, Checked = false });
                    }
                    ViewBag.SelectedSkills = list;
                    sk = list;
                }
            else
              {
                    user = db.JobRequisitions.Include("Skills").Where(s => s.JobRequisitionID == id).FirstOrDefault();
                    ViewBag.Skills = db.Skills.ToList();
                    var list = new List<CheckModel>();
                    foreach (var item in (List<Skill>)ViewBag.Skills)
                    {
                        if (user.Skills.Contains(item))
                        {
                            list.Add(new CheckModel { Id = item.ID, Name = item.Name, Checked = true });
                        }
                        else
                        {
                            list.Add(new CheckModel { Id = item.ID, Name = item.Name, Checked = false });
                        }
                    }
                    ViewBag.SelectedSkills = list;
                    sk = list;

                }
            }
            return PartialView("_GetSkills", sk);
        }
        [Route("Requisition/RolesInDepartment")]
        public JsonResult RolesInDepartment(int departmentid)
        {
            var db = new TalentContext();
            var positions = db.OfficePositions.Where(O => O.DepartmentID == departmentid)
                     .Select(c => new { Value = c.OfficePositionID, Text = c.Title }).ToList();
            var positionslist = new SelectList(positions, "Value", "Text", "Select Any Item");
            ViewBag.Positions = positionslist;
            return Json(positions, "application/json", JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetInformationForRole(int officeroleid)
        {
            var db = new TalentContext();
            OfficePosition details = db.OfficePositions.Find(officeroleid);
            return Json(new { Title = details.Title, Responsibilities = details.Reqirements }, "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
        }

        private List<JobSeeker> MatchingTheCandidatesAlgorithm(int id)
        {
            var matchedApplicants = new List<JobSeeker>();
            var requisition = new JobRequisition();
            var db = new TalentContext();
            var Applications = ApplicationsForRequisition(id);
            requisition = db.JobRequisitions.Include("OfficePosition").Where(o=>o.JobRequisitionID==id).FirstOrDefault();
            //matchedApplicants= db.Applicants.Include("Skills").Where(apl => apl.IndustryID == requisition.OfficePosition.IndustryID).ToList();
            matchedApplicants = db.Applicants.Include("Skills").ToList();
            matchedApplicants = CheckApplicantsForMatch(requisition, matchedApplicants);
            return matchedApplicants;
        }

        private List<JobSeeker> CheckApplicantsForMatch(JobRequisition requisition, List<JobSeeker> allApplicants)
        {
            var matchedApplicants = new List<JobSeeker>();
           // var matchedApplications = new List<JobApplication>();
            //var allApplicants = new List<JobSeeker>();
            int _passed = 0, _failed;

            foreach (var mit in allApplicants)
            {
                _passed = 0; _failed = 0;
                ComparisonOne(ref _passed, ref _failed, requisition.Skills, mit.Skills);
                ComparisonTwo(ref _passed, ref _failed, requisition.MinimumQualification,requisition.HighestQualification, mit.HighestQualification);
                ComparisonThree(ref _passed, ref _failed, requisition.AgeLimit, mit.Age);

                if (_passed >= 2)
                {
                    matchedApplicants.Add(mit);
                }
            }
            return matchedApplicants;
        }

        private void ComparisonTwo(ref int _passed, ref int _failed, GradeObtained? minimumQualification, GradeObtained? highestQualification, GradeObtained? CandidatehighestQualification)
        {
            if ((CandidatehighestQualification>=minimumQualification)&&(CandidatehighestQualification<=highestQualification))
            {
                _passed++;
            }
            
        }

        private List<JobApplication> MatchingTheCandidatesByApplicationAlgorithm(int? id)
        {
            var matchedApplicants = new List<JobSeeker>();
            var matchedApplications = new List<JobApplication>();
            var requisition = new JobRequisition();
            var db = new TalentContext();
            var Applications = ApplicationsForRequisition(id);
            requisition = db.JobRequisitions.Find(id);
            //matchedApplicants = CheckApplicationsForMatch(requisition, Applications);
            matchedApplications = CheckApplicationsForMatch(requisition, Applications);
           /// return matchedApplicants;
            return matchedApplications;
        }

        private List<JobApplication> CheckApplicationsForMatch(JobRequisition requisition, IQueryable<JobApplication> applications)
        {
            //var matchedApplicants = new List<JobSeeker>();
            var matchedApplications = new List<JobApplication>();
            //var allApplicants = new List<JobSeeker>();
            int _passed = 0, _failed;

            foreach (var mot in applications)
            {
                    var mit = mot.JobSeeker;
                    _passed = 0; _failed = 0;
                    ComparisonOne(ref _passed, ref _failed, requisition.Skills, mit.Skills);
                    ComparisonTwo(ref _passed, ref _failed, requisition.MinimumQualification, requisition.HighestQualification, mit.HighestQualification);
                    ComparisonThree(ref _passed, ref _failed, requisition.AgeLimit, mit.Age);

                    if (_passed >= 2)
                    {
                    matchedApplications.Add(mot);
                    }
                }
            return matchedApplications;
        }
        private void ComparisonThree(ref int _passed, ref int _failed, int ageLimit, int age)
        {
            if (_passed >= 1)
            {
                if (ageLimit>=age)
                {
                    _passed++;
                }
            }
        }
        private void ComparisonOne(ref int _passed, ref int _failed, ICollection<Skill> requiredskills, ICollection<Skill> applicantskills)
        {
            int requiredSkillsCount = requiredskills.Count;
            int possedskills=0;
            foreach(var v in requiredskills)
            {
                if (applicantskills.Contains(v))
                {
                    possedskills++;
                }
            }
            if(possedskills >= (requiredSkillsCount * 0.6))
                _passed++;
        }
        private IQueryable<JobApplication> ApplicationsForRequisition(int? id)
        {
           // var Applications = IQueryable<JobApplication>();
            var db = new TalentContext();
           var Applications = db.JobApplications.Include("JobSeeker.Skills").Where(job => job.JobRequisitionID == id && job.ApplicationStatus >= ApplicationStatus.Applied);
            return Applications;
        }
    }
}