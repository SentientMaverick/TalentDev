using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using Talent.HRM.Services.Interfaces;

namespace Talent.HRM.Services.Email
{
    public  class ConfirmInterviewEmail : IEmailMessaging
    {
        public ConfirmInterviewEmail(string applicantemail, string applicantname)
        {
            Applicantemail = applicantemail;
            Applicantname = applicantname;
        }
        public string Applicantemail { get; set; }
        public string Applicantname { get; set; }
        public void SendEmail(string to, string from)
        {

        }
        public void SendEmailToApplicant()
        {
            try
            {
                MailMessage mailMessage = new MailMessage("info@talenthrm.net", this.Applicantemail);
                //MailMessage mailMessage = new MailMessage("info@stconboading.net", "bolfas2009@gmail.com");
                mailMessage.Subject = "Interview Scheduling Process-Applicant Availabilty and Date Selection";
                mailMessage.IsBodyHtml = false;
                mailMessage.Body = "Hello, \n\n Dear Applicant , Please be informed that you have been selected for an Interview. Kindly visit your account on the portal to confirm your attendance \n\nBest Regards.";
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
            }
            catch (Exception ex)
            {
                //LblEmailStatus.Text = "Could not send the e-mail - error: " + ex.Message;
            }
        }
    }
}
