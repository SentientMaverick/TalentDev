using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talent.HRM.Services.Interfaces
{
    public interface IEmailMessaging
    {
        void SendEmail(string to, string from);
        void SendEmailToApplicant();
       }
}
