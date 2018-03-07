using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TalentAcquisition.Core.Domain;
using TalentAcquisition.DataLayer;

namespace TalentAcquisition.Controllers
{
    public class TeamController : Controller
    {
        // GET: Team
        [Route("Team/Requisitions")]
        public ActionResult Index()
        {
            return View();
        }
        [OutputCache(Duration =60)]
        public int _getUserId(){
            var db = new TalentContext();
            var userguid = User.Identity.GetUserId();
            var userid = db.Employees.Where(x => x.UserId == userguid).FirstOrDefault().ID;
            return userid;
        }
        [OutputCache(Duration = 120)]
        public ActionResult _GetRequisitionsForTeamMember()
        {
            var activerequisitions = new List<JobRequisition>();
            var activeinterview = new List<Interview>();
            using (var db = new TalentContext())
            {
                var userid = _getUserId(); 
                var interviewsForTeamMember = db.InterviewDetails.Where(x=>x.TeamMember1ID == userid 
                || x.TeamMember2ID == userid || x.TeamMember3ID == userid || x.TeamMember4ID == userid).ToList();

            foreach(var vm in interviewsForTeamMember)
                {
                     activeinterview.Add(db.Interviews.Find(vm.InterviewID));
                }

            foreach (var cm in activeinterview)
                {
                    activerequisitions.Add(db.JobRequisitions.Find(cm.JobRequisitionID));
                }
            }
            return PartialView(activerequisitions);
        }
        [Route("Team/Requisitions/{id:int}/Interviews", Name = "TeamRequisition")]
        public ActionResult _GetInterviewsForRequisition(int? id)
        {
            //
            var activeinterview = new List<Interview>();
            using (var db = new TalentContext())
            {
                activeinterview = db.Interviews.Where(x => x.JobRequisitionID == id).ToList();
            }
            return PartialView(activeinterview);
        }

        [Route("Team/Requisition/Interview/{id:int}", Name = "TeamRequisitionLink")]
        public ActionResult _GetRecommendationForTeamMember(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var db = new TalentContext();
            Interview interview = db.Interviews.Find(id);
            if (interview == null)
            {
                return HttpNotFound();
            }
            ViewBag.requisitionid = interview.JobRequisitionID;
            ViewBag.applicationid = interview.JobApplicationID;
            ViewBag.applicantid = new TalentContext().JobApplications.Where(o => o.JobRequisitionID == interview.JobRequisitionID && o.JobApplicationID == interview.JobApplicationID).FirstOrDefault().JobSeekerID;

            return View();
        }
    }
}