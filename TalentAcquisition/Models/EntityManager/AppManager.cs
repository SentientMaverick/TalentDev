using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using TalentAcquisition.Models;
using TalentAcquisition.DataLayer;
using System.Net;
using TalentAcquisition.Core.Domain;
using System.Data.SqlTypes;

namespace TalentAcquisition
{
   // [Authorize]
    public class AppManager : Controller
    {
        #region FieldVariables
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            set
            {
                _signInManager = value;
            }
        }

        internal bool IsUserInRole(string name, string roles)
        {
            throw new NotImplementedException();
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }

        #endregion

        #region Constructor
        public AppManager()
        {
        }
        public AppManager(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        #endregion
        #region MainMethods
        public async Task<ActionResult> ApplicantLogin(LoginViewModel model, string returnUrl,string origin="")
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Pass in a variable that specifies whether the request is from the public login portal or the backend employee portal
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            return await PerformLogin(model, returnUrl,origin);
        }
        public async Task<ActionResult> EmployeeLogin(LoginViewModel model, string returnUrl,string origin="Employee")
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Pass in a variable that specifies whether the request is from the public login portal or the backend employee portal
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            return await PerformLogin(model, returnUrl,origin);
        }

        internal async Task<ActionResult> SaveBioDetails(JobSeeker applicant)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    using (var db = new TalentContext())
                    {
                        var id = applicant.ID;
                        var originaluserdata = GetUserProfile(id);
                        originaluserdata.FirstName = applicant.FirstName;
                        originaluserdata.LastName = applicant.LastName;
                        originaluserdata.PhoneNumber = applicant.PhoneNumber;
                        originaluserdata.Address = applicant.Address;
                        originaluserdata.DateOfBirth = applicant.DateOfBirth;
                        originaluserdata.IndustryID = applicant.IndustryID;
                        db.Entry(originaluserdata).State = EntityState.Modified;
                          await db.SaveChangesAsync();
                        //db.SaveChanges();
                    }
                    return RedirectToAction("Education", "Applicant");
                }
                return View("ManageProfile", "Applicant", applicant);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex);
                return View("Error");
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public async Task<ActionResult> ApplicantRegistration(RegisterViewModel model, string returnUrl)
        {

            if (ModelState.IsValid)
                    {
                        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                        var result = await UserManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            var context = new TalentContext();
                                using (context)
                                {
                                    var jobseeker = new JobSeeker() { UserId = user.Id, ApplicantNumber = model.UserName };
                                    jobseeker.RegistrationDate = DateTime.Now;
                                    jobseeker.DateOfBirth = DateTime.Now;
                                    context.Applicants.Add(jobseeker);
                                    context.SaveChanges();
                                }
                        
                    // await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                                // return RedirectToAction("Index", "Home");

                    return RedirectToAction("RegistrationSuccess", "Applicant", new {status="sucessful",confirmcode=new Random().Next(200000,999999) });
                       }
                AddErrors(result);
                        }
            return View("Signup",model);
        }

        public ActionResult EmployeeRegistration(Employee employee, string UserEmail)
        {
            if (ModelState.IsValid)
            {
                using (var db = new TalentContext())
                {
                    using (var dbtransact = db.Database.BeginTransaction())
                    {
                        ViewBag.OfficePositionID = new SelectList(db.OfficePositions, "OfficePositionID", "Title", employee.OfficePositionID);
                        try
                        {
                            ApplicationDbContext context = new ApplicationDbContext();


                            //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                            //var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                            var user = new ApplicationUser { UserName = UserEmail, Email = UserEmail };
                            //var result = await UserManager.CreateAsync(user, model.Password);
                            //var user = new ApplicationUser();

                            //user.UserName = employee.EmployeeNumber;
                            //user.Email = "syedshanumcain@gmail.com";
                            // user.Email = UserEmail;
                            string userPWD = employee.Password;

                            var chkUser = UserManager.Create(user, userPWD);

                            if (chkUser.Succeeded)
                            {
                                employee.UserId = user.Id;
                                //user.LockoutEnabled = true;
                                db.Employees.Add(employee);
                                db.SaveChanges();
                            }
                            dbtransact.Commit();
                            ViewBag.Message = "Successfully Created Employee Record.";
                        }
                        catch
                        {
                            dbtransact.Rollback();
                            ViewBag.Message = "Sorry! Please Check if all the fields are field correctly and try again.";
                            return View("Employees/Create",employee);
                        }
                    }
                }
                return View("Employees/Create");
            }
           return View("Employees/Create",employee);
        }
        #endregion
        #region HelperMethods
        private ActionResult RedirectToLocal(string returnUrl)
        {
            try
            {
                //if (Url.IsLocalUrl(returnUrl))
                //{
                    return Redirect(returnUrl);
                //}
                //return RedirectToAction("Dashboard", "Applicant");
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }

            
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        private async Task<ActionResult> PerformLogin(LoginViewModel model, string returnUrl, string origin)
        {
            //var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,shouldLockout:false);

            switch (result)
            {
                case SignInStatus.Success:
                    if (returnUrl != null)
                        return RedirectToLocal(returnUrl);
                    else if (origin == "Employee")
                        return RedirectToAction("Dashboard", "Admin");
                    else
                        return RedirectToAction("Dashboard", "Applicant");

                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        internal JobSeeker GetUserProfile(int userid=0)
        {
            var currentuser = new JobSeeker();
            using (var db = new TalentContext())
            {
                if (userid == 0)
                        {
                            var id = User.Identity.GetUserId();
                            userid = db.Applicants.Where(s => s.UserId == id).FirstOrDefault().ID;
                        }
            
                currentuser = db.Applicants.Where(s => s.ID == userid).FirstOrDefault();
            }
            return currentuser;
        }

        #endregion
    }
  }
