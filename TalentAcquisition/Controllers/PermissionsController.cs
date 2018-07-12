using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using TalentAcquisition.DataLayer;
using TalentAcquisition.Core.Domain;
using System.Web.Mvc;

namespace TalentAcquisition.Controllers
{
    public class PermissionsController : Controller
    {
        public ApplicationDbContext Context { get; set; }
        public TalentContext db { get; set; }
        public PermissionsController()
        {
            ApplicationDbContext context = new ApplicationDbContext();
              Context = context;
            db = new TalentContext();
        }
        // GET: Permissions
        [Route("Permissions/All")]
        public ActionResult Index()
        {
            return View(db.ApplicationRoles.ToList());
        }
        // GET: Roles/Details/5
        public ActionResult Details(string id)
        {
            return View(db.ApplicationRoles.Where(x=>x.Id==id));
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            ApplicationRole Role = new ApplicationRole();
            return View(Role);
        }
        // POST: Roles/Create
        [HttpPost]
        public ActionResult Create(ApplicationRole Role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IdentityManager _manager = new IdentityManager();
                    var result = _manager.CreateRoleReturnString(Role.Name);
                    if (!String.IsNullOrEmpty(result))
                    {
                        Role.Id = result;
                        db.ApplicationRoles.Add(Role);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    return View(Role);
                }
                return View(Role);
            }
            catch
            {
                return View(Role);
            }
        }
        public ActionResult Edit(string id)
        {
            return View(db.ApplicationRoles.Where(x => x.Id == id));
        }
    }
}
