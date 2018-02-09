using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TalentAcquisition.Core.Domain;
using TalentAcquisition.DataLayer;
using TalentAcquisition.Models;
using TalentAcquisition.Models.ViewModel;

namespace TalentAcquisition.Controllers
{
    public class AdminController : Controller
    {
        private AppManager app = new AppManager();
        // GET: Admin
        [AllowAnonymous]
        // GET: Applicant
        public ActionResult Portal()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                SetInitializers();
                return await app.EmployeeLogin(model, returnUrl);
            }
            catch
            {

                return RedirectToAction("Portal",model);
            }
        }
        //[Route("Employee/Dashboard")]
        [Route("Admin/Dashboard")]
        public ActionResult Dashboard()
        {
            SetUserSessionID();
            ViewBag.UserID = TempData["userid"];
            return View();
        }
        [ChildActionOnly]
        public ActionResult _GetNotifications()
        {
            SetUserSessionID();
            int id = (int)TempData["userid"];
            AdminDashboardNotification notification = GetNotifications(id);
            
            return PartialView(notification);
        }

        private AdminDashboardNotification GetNotifications(int? id)
        {
            var notification = new AdminDashboardNotification();
            notification.TotalNotificationCount = 0;
            notification.newJobRequisitionCount = 0;
            notification.activeRequisitionCount = 0;
            try
            {
                var jobs = new List<JobRequisition>();
                using (var db = new TalentContext())
                {
                    var alljobs = db.JobRequisitions;
                    var publishedjobs = alljobs.Where(o => o.Status.Value
                     != JobRequisition.JobRequisitionStatus.Completed);
                    var activejobs = publishedjobs.Where(o => o.Status.Value
                     == JobRequisition.JobRequisitionStatus.Posted);
                    notification.activeRequisitionCount = !Equals(activejobs.Count(), null) ? activejobs.Count() : 0;
                    var newrequisitions = publishedjobs.Where(o => o.Status.Value
                      == JobRequisition.JobRequisitionStatus.Created);
                    notification.newJobRequisitionCount = !Equals(newrequisitions.Count(), null) ? newrequisitions.Count() : 0;

                    notification.TotalNotificationCount = notification.activeRequisitionCount + notification.newJobRequisitionCount;
                    if (activejobs.Any())
                    {
                        //int count = 1;
                        foreach (var item in activejobs)
                        {
                            var url = item.JobTitle.Split(' ');
                            var notificationitem = new DashboardNotification
                            {
                                ID = item.JobRequisitionID,
                                Title = "New Requisition for " +
                                  item.JobTitle + " Starting Date:" + item.StartDate,
                                url = "/Job/" + item.JobRequisitionID + "/" + String.Join("-", url)
                            };
                          notification.Notifications.Add(notificationitem);
                            
                            //count++;
                        }
                        if (newrequisitions.Any())
                        {
                            //int count = 1;
                            foreach (var item in newrequisitions)
                            {
                                var url = item.JobTitle.Split(' ');
                                var notificationitem= new DashboardNotification
                                {
                                    ID = item.JobRequisitionID,
                                    Title = "New Requisition for " +
                                  item.JobTitle + " Starting Date:" + item.StartDate,
                                    url = "/Job/" + item.JobRequisitionID + "/" + String.Join("-", url)
                                };
                                notification.Notifications.Add(notificationitem);
                                //count++;
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            return notification;
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Portal", "Admin");
        }          
        // GET: Admin/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        [Route("Admin/Application/{id:int}/{details}")]
        public ActionResult Application(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TalentContext db = new TalentContext();
            //Core.Domain.JobApplication jobApplication = db.JobApplications.Find(id);
            //ONE OF THE EFFECTS OF BAD PROGRAMMING
            Core.Domain.JobApplication jobApplication = db.JobApplications
                .Where(application => application.JobApplicationID == id).FirstOrDefault();
            if (jobApplication == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicationID = jobApplication.JobApplicationID;
            ViewBag.JobSeekerID = jobApplication.JobSeeker.ID;
            ViewBag.RequisitionID = jobApplication.JobRequisitionID;
            return View(jobApplication);
        }
        // GET: Admin/Create

        [ChildActionOnly]
        public JsonResult UpdateApplication(int applicationid)
        {
            string updatestatus = "failed";
            try
            {
                updatestatus = "success";
            }
            catch
            {

            }
            return Json(new { status = updatestatus },JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            return View();
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Edit/5
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

        // POST: Admin/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
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


        private void SetInitializers()
        {
            var var1 = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            var var2 = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            app.UserManager = var2; app.SignInManager = var1;
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        private void SetUserSessionID()
        {
            if (TempData["userid"] == null)
            {
                var userid = User.Identity.GetUserId();
                var applicantid = new TalentContext().Employees.Where(s => s.UserId == userid).FirstOrDefault().ID;
                TempData["userid"] = applicantid;
            }

        }

    }
}
