using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using Talent.HRM.Services.Interfaces;
using System.Security.Policy;

namespace Talent.HRM.Services.Email
{
     public class NotifyOnboardingEmail: IEmailMessaging
    {
        public NotifyOnboardingEmail(string applicantemail, string applicantname, string jobtitle, string joburl)
        {
            Applicantemail = applicantemail;
            Applicantname = applicantname;
            Jobtitle = jobtitle;
            Joburl = joburl;
        }

        public string Applicantemail { get; set; }
        public string Applicantname { get; set; }
        public string Jobtitle { get; set; }
        public string Joburl { get; set; }
        public void SendEmail(string to, string from)
        {

        }
        public void SendEmailToApplicant()
        {
            try
            {
                MailMessage mailMessage = new MailMessage("info@talenthrm.net", this.Applicantemail);
                mailMessage.Subject = "Start Onboarding Process";
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = "Hello, \n\n Dear Applicant "+this.Applicantname+ ",Congratulations! \n\n Please be informed that you have been selected for the position of " 
                    + this.Jobtitle + ". Click on the <a href ='"+ this.Joburl+ "'> Link </a> on the portal to start . \n\nBest Regards.";
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
