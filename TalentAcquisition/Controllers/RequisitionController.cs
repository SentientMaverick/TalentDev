using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TalentAcquisition.BusinessLogic.Domain;
using TalentAcquisition.BusinessLogic.UpdatedDomain;
using TalentAcquisition.Core.Domain;
using TalentAcquisition.DataLayer;
using TalentAcquisition.Filters;
using TalentAcquisition.Models.Core;
using Humanizer;

namespace TalentAcquisition.Controllers
{
    [AuthorizeEmployee]
    public class RequisitionController : Controller
    {
        TalentContext context = new TalentContext();
        #region Views
        // GET: Requisition
        [Route("Admin/Requisitions")]
        public ActionResult Index()
        {
            return View();
        }
        [Route("Admin/JobrequisitionManager")]
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
            var req = new TalentContext().JobRequisitions.Count();
            requisition.RequisitionNo = "TR" + String.Format("{0:D6}", req + 1);
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Couldn't Create Requisition; Form was not Completed Properly";
                return View(requisition);
            }
            try
            {
                List<Skill> selectedskills = checks.Where(x => x.Checked == true)
                                        .Select(o => new Skill { ID = o.Id, Name = o.Name }).ToList();
                var selectedint = selectedskills.Select(o => o.ID).ToList();

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
            JobRequisition jobRequisition = db.JobRequisitions.Include("Skills").Where(x => x.JobRequisitionID == id).First();
            if (jobRequisition == null)
            {
                return HttpNotFound();
            }
            ViewBag.RequisitionID = jobRequisition.JobRequisitionID;
            return View(jobRequisition);
        }
        // [HttpGet]
        [Route("Admin/matchskill/{id:int}", Name = "MatchSkill")]
        public ActionResult MatchSkill(int? id, string details, string filter)
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
                ViewBag.MatchedApplicants = new List<MatchedApplicant>();
                if (jobRequisition.MatchedApplicants.Count() > 0)
                {
                    ViewBag.MatchedApplicants = db.MatchedApplicants.Include("JobSeeker").Where(x => x.JobRequisitionID == id).ToList();
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
        [Route("Admin/matchskill/{id:int}/ScreenApplicant/{applicantid:int}")]
        public ActionResult VerifyMatchedApplicant(int? id, int? applicantid, string details, string filter)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                if (applicantid == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                TalentContext db = new TalentContext();
                JobRequisition jobRequisition = db.JobRequisitions.Include("Skills").Where(x => x.JobRequisitionID == id).First();
                if (jobRequisition == null)
                {
                    return HttpNotFound();
                }
                JobSeeker applicant = db.Applicants.Include("skills").Where(x => x.ID == applicantid).First();
                if (applicant == null)
                {
                    return HttpNotFound();
                }
                ViewBag.applicantid = applicantid;
                ViewBag.Applicant = applicant;
                ViewBag.RequisitionID = jobRequisition.JobRequisitionID;
                return View(jobRequisition);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
        [Route("Admin/matchskill/{id:int}/ScreenApplicant/{applicantid:int}")]
        [HttpPost]
        public ActionResult VerifyMatchedApplicant(int id, int applicantid)
        {
            try
            {
                using (var db = new TalentContext())
                {
                    var existingapplication = db.JobApplications.Where(x => x.JobRequisitionID == id && x.JobSeekerID == applicantid);
                    if (existingapplication.Any())
                    {
                        existingapplication.First().ApplicationStatus = ApplicationStatus.Screened;
                        db.SaveChanges();
                    }
                    else
                    {
                        var application = new JobApplication()
                        {
                            JobRequisitionID = id,
                            JobSeekerID = applicantid,
                            ApplicationStatus = ApplicationStatus.Screened,
                            RegistrationDate = DateTime.Now
                        };
                        db.JobApplications.Add(application);
                        db.SaveChanges();
                    }

                }
                return RedirectToAction("Matchskill/" + id, "Admin");
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
        [Route("Admin/Requisition/Approve/{id:int}/{details}/", Name = "ApproveLink")]
        public ActionResult Approve(int? id)
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
        [Route("Admin/Requisition/Approve/{id:int}/{details}/")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Approve(int id)
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
            jobRequisition.Status = JobRequisition.JobRequisitionStatus.Approved;
            db.Entry(jobRequisition).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            ViewBag.Message = "Job Successfully Published";
            return RedirectToAction("Requisitions", "Admin");
        }
        [Route("Admin/Requisition/Edit/{id:int}/{details}")]
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
        [Route("Admin/Requisition/Edit/{id:int}/{details}")]
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
                    return RedirectToAction("Requisitions", "Admin");
                    //return View(requisition);
                }
            }
            catch
            {
                //return;
                return View("Error");
            }

        }
        [Route("Admin/Requisition/reject/{id:int}/{details}")]
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
        [Route("Admin/Requisition/reject/{id:int}/{details}")]
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
        #endregion
        #region PartialViews
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
            ViewBag.RequisitionID = id;
            var activeapplications = new List<JobApplication>();
            activeapplications = MatchingTheCandidatesWithApplicationAlgorithm(id);
            List<JobSeeker> _matchedapplicants = activeapplications.Select(x => x.JobSeeker).ToList();
            UpdateMatchedApplicantRecord(_matchedapplicants, id);
            return PartialView("_GetNewApplications", activeapplications);
        }
        private void UpdateMatchedApplicantRecord(List<JobSeeker> matchedapplicants, int requisitionid)
        {
            using (var db = new TalentContext())
            {
                var prevmatchedapplicants = db.MatchedApplicants.Include("JobSeeker").Where(x => x.JobRequisitionID == requisitionid);
                foreach (var applicant in matchedapplicants)
                {
                    if (!prevmatchedapplicants.Where(x => x.JobSeeker.ID == applicant.ID).Any())
                    {
                        var matchedrecord = new MatchedApplicant() { JobRequisitionID = requisitionid, JobSeekerID = applicant.ID };
                        db.MatchedApplicants.Add(matchedrecord);
                        db.SaveChanges();
                    }
                }
            }
        }
        public ActionResult _GetMatchingApplicants(int id)
        {
            ViewBag.RequisitionID = id;
            var activeapplicants = new List<JobSeeker>();
            activeapplicants = MatchingTheCandidatesAlgorithm(id);
            UpdateMatchedApplicantRecord(activeapplicants, id);
            return PartialView(activeapplicants);
        }
        public ActionResult _GetPreviouslyMatchedApplicants(int id)
        {
            ViewBag.RequisitionID = id;
            var activeapplicants = new List<JobSeeker>();
            var db = new TalentContext();
            var matchedapplicantsrecords = db.MatchedApplicants.Include("JobSeeker.Skills").Where(x => x.JobRequisitionID == id);
            activeapplicants = matchedapplicantsrecords.Select(x => x.JobSeeker).ToList();
            var jbk = db.JobApplications.Include("JobSeeker")
                    .Where(x => x.JobRequisitionID == id).Select(x => x.JobSeeker).ToList();
            foreach (var jb in jbk)
            {
                activeapplicants.Remove(jb);
            }
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
                activerequisitions = db.JobRequisitions.Where(job => job.Status == JobRequisition.JobRequisitionStatus.Approved || job.Status==JobRequisition.JobRequisitionStatus.Posted ).ToList();
            }
            return PartialView(activerequisitions);
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
            var job = new JobRequisition();
            using (var db = new TalentContext())
            {
                ViewBag.Departments = db.Departments.ToList();
                ViewBag.Positions = Enumerable.Empty<SelectListItem>();
                var req = db.JobRequisitions.Count();
                job.RequisitionNo = "TR" + String.Format("{0:D6}", req + 1);
            }
            // var numgen = new Random();

            return PartialView(job);
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
            var employeecount = db.Employees.Where(x => x.OfficePositionID == officeroleid).Count();
            var vaccantposts = details.Posts - employeecount;
            return Json(new { Title = details.Title, Responsibilities = details.Reqirements, Posts = details.Posts, AvailablePosts = vaccantposts }, "application/json", Encoding.UTF8, JsonRequestBehavior.AllowGet);
        } 
        #endregion
        #region Algorithm
        private List<JobSeeker> MatchingTheCandidatesAlgorithm(int id)
        {
            var matchedApplicants = new List<JobSeeker>();
            var requisition = new JobRequisition();
            //var db = new TalentContext();
            using (var db = new TalentContext())
            {
                requisition = db.JobRequisitions.Include("OfficePosition").Where(o => o.JobRequisitionID == id).FirstOrDefault();
               var jbk=db.JobApplications.Include("JobSeeker")
                    .Where(x => x.JobRequisitionID == id).Select(x => x.JobSeeker).ToList();
                matchedApplicants = db.Applicants.Include("Skills").ToList();
                foreach (var jb in jbk)
                {
                    matchedApplicants.Remove(jb);
                }
                matchedApplicants = CheckApplicantsForMatch(requisition, matchedApplicants);
            }
               
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
                ComparisonTwo(ref _passed, ref _failed, requisition.MinimumQualification, requisition.HighestQualification, mit.HighestQualification);
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
            if ((CandidatehighestQualification >= minimumQualification) && (CandidatehighestQualification <= highestQualification))
            {
                _passed++;
            }

        }
        private List<JobApplication> MatchingTheCandidatesWithApplicationAlgorithm(int? id)
        {
            //var matchedApplicants = new List<JobSeeker>();
            var matchedApplications = new List<JobApplication>();
            var requisition = new JobRequisition();  
            using (var db = new TalentContext())
            {
               // var Applications = ApplicationsForRequisition(id, db);
                var Applications = db.JobApplications.Include("JobSeeker.Skills").Where(job => job.JobRequisitionID == id && job.ApplicationStatus==ApplicationStatus.Applied).ToList();
                requisition = db.JobRequisitions.Find(id);
                //matchedApplicants = CheckApplicationsForMatch(requisition, Applications);
                matchedApplications = CheckApplicationsForMatch(requisition, Applications);
            }          
            /// return matchedApplicants;
            return matchedApplications;
        }
        private List<JobApplication> CheckApplicationsForMatch(JobRequisition requisition, List<JobApplication> applications)
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
                if (ageLimit >= age)
                {
                    _passed++;
                }
            }
        }
        private void ComparisonOne(ref int _passed, ref int _failed, ICollection<Skill> requiredskills, ICollection<Skill> applicantskills)
        {
            int requiredSkillsCount = requiredskills.Count;
            int possedskills = 0;
            foreach (var v in requiredskills)
            {
                if (applicantskills.Contains(v))
                {
                    possedskills++;
                }
            }
            if (possedskills >= (requiredSkillsCount * 0.6))
                _passed++;
        }
        private List<JobApplication> ApplicationsForRequisition(int? id,TalentContext db)
        {
            //var Applications =ne IQueryable<JobApplication>();
            var context = new TalentContext();
            //var Applications = db.JobApplications.Include("JobSeeker.Skills").Where(job => job.JobRequisitionID == id && job.ApplicationStatus >= ApplicationStatus.Applied);
            var Applications = context.JobApplications.Include("JobSeeker.Skills").Where(job => job.JobRequisitionID == id).ToList();
           // var Applications = context.JobApplications.Include("JobSeeker.Skills").Where(job => job.JobRequisitionID == id);
            return Applications;            
        } 
        #endregion
    }
}