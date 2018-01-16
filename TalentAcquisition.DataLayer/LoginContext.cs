using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TalentAcquisition.DataLayer
{
    public class LoginContext: BaseContext<LoginContext>
    {
        public static LoginContext Create()
            {
                return new LoginContext();
            }
    }

    /// <summary>
    // The ApplicationDbContext Handles all of the User Login Functions 
    /// </summary>
    public class ApplicationDbContext : BaseContext<ApplicationDbContext>
    {

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}