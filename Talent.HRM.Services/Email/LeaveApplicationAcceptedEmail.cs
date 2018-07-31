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
   public class LeaveApplicationAcceptedEmail : IEmailMessaging
    {
        private string Message { get; set; }
        public LeaveApplicationAcceptedEmail(string message)
        {
            Message = message;
        }
        public void SendEmail(string to, string from)
        {
            try
            {
                MailMessage mailMessage = new MailMessage("info@talenthrm.net", to);
                mailMessage.Subject = "Leave Application Approval Letter";
                mailMessage.IsBodyHtml = true;
                //mailMessage.Body = "Hello, \n\n Dear Applicant " + this.Applicantname + ",Congratulations! \n\n Please be informed that you have been offered the position of "
                //    + this.Jobtitle + ". \n\n"+ this.Message+ ". \n\n Best Regards.";
                mailMessage.Body = this.Message;
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

        public void SendEmailToApplicant()
        {
            
        }
    }
}
