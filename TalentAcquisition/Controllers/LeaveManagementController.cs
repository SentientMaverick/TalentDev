using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using TalentAcquisition.BusinessLogic.Domain;
using TalentAcquisition.BusinessLogic.UpdatedDomain;
using TalentAcquisition.Core.Domain;
using TalentAcquisition.DataLayer;
using TalentAcquisition.Filters;
using TalentAcquisition.Models.Core;
using System.Data.Entity;

namespace TalentAcquisition.Controllers
{
    public class LeaveManagementController : Controller
    {
        TalentContext db = new TalentContext();
        // GET: LeaveManagement
        //[Route("Admin/Leave")]
        [HttpGet]
        [Route("Admin/LeavePlan")]
        public ActionResult Index()
        {
            var model = new LeaveManage();
            return View(model);
        }
        [Route("LeaveManagement/Settings")]
        public ActionResult Settings()
        {
            return View();
        }

        [ValidateInput (false)]
        [HttpPost]
        public ActionResult Index(LeaveManage mails)
        {
            MailMessage mailMessage = new MailMessage();
            MailAddress fromMail = new MailAddress("TalentHrm@gmail.com");
            var Empmails = db.Employees.Select(c => c.OfficialEmail).ToString();
            //mails.EmployeesMail = Empmails;
            //foreach (var empmail in Empmails.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
            //{
            //    mails.EmployeesMail = empmail;
             //mailMessage.To.Add(new MailAddress("bolfas2009@gmail.com"));
            //}
            //mails = Empmails
            mails.EmployeesMail = "bolfas2009@gmail.com";
            mailMessage.To.Add(new MailAddress(mails.EmployeesMail));
            mailMessage.From = fromMail;
            mailMessage.Subject = "Leave Plan";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = mails.MessageBody;
                //"Hello, \n\n Dear Staff, Please be informed that you are expected to send your leave plan across to the HOD \n\nBest Regards.";
            //mailMessage.CC.Add("adgarba@erpschoolafrica.com");
            //mailMessage.Bcc.Add("info@codeware.com.ng");
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "mail.oohwantech.com";
            smtpClient.EnableSsl = false;
            NetworkCredential NetworkCred = new NetworkCredential("lukman@oohwantech.com", "Me@digit01");
            smtpClient.UseDefaultCredentials = true;
            smtpClient.Credentials = NetworkCred;
            smtpClient.Port = 25;
            smtpClient.Send(mailMessage);
            Response.Write("Message Sent");
            return View(mails);
        }
        [ChildActionOnly]
        public ActionResult _GetLeavePlanList()
        {
            var leaveplanlist = new List<ManageEmployeeLeave>();
            leaveplanlist = db.ManageEmployeeLeaves.Where(c => c.Status == ManageEmployeeLeave.LeaveStatus.Pending).ToList();
            return PartialView(leaveplanlist);  
        }

        [ChildActionOnly]
        public ActionResult _GetApprovedLeavePlanList()
        {
            var Approvedleaveplanlist = new List<ManageEmployeeLeave>();
            Approvedleaveplanlist = db.ManageEmployeeLeaves.Where(c => c.Status == ManageEmployeeLeave.LeaveStatus.Approved).ToList();
            return PartialView(Approvedleaveplanlist);
        }

        [ChildActionOnly]
        public ActionResult _GetRejectedLeavePlanList()
        {
            var Rejectedleaveplanlist = new List<ManageEmployeeLeave>();
            Rejectedleaveplanlist = db.ManageEmployeeLeaves.Where(c => c.Status == ManageEmployeeLeave.LeaveStatus.Rejected).ToList();
            return PartialView(Rejectedleaveplanlist);
        }

        // GET: ManageEmployeeLeaves/Edit/5
        [Route("LeaveManagement/Plan/Approve/{id}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ManageEmployeeLeave manageEmployeeLeave = db.ManageEmployeeLeaves.Find(id);
            if (manageEmployeeLeave == null)
            {
                return HttpNotFound();
            }
            return View(manageEmployeeLeave);
        }

        // POST: ManageEmployeeLeaves/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("LeaveManagement/Plan/Approve/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,EmployeeId,EmployeeName,StartDate,EndDate,Duration,ApplyDate,Status,LeaveType,LeaveLimit,LeaveType_Limit_ID")] ManageEmployeeLeave manageEmployeeLeave)
        {
            if (ModelState.IsValid)
            {
               // manageEmployeeLeave.TotalLeaveAvailable = manageEmployeeLeave.Duration;
                db.Entry(manageEmployeeLeave).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(manageEmployeeLeave);
        }

    }
}