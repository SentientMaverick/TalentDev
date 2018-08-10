using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TalentAcquisition.BusinessLogic.UpdatedDomain;
using TalentAcquisition.DataLayer;
using TalentAcquisition.Repositories.Interfaces;
using TalentAcquisition.Repositories;
using Microsoft.AspNet.Identity;

namespace TalentAcquisition.Controllers
{
    public class AnnouncementsController : Controller
    {
        private TalentContext db = new TalentContext();
        private readonly IEmployeeRepository _employeeRepository;

        public AnnouncementsController()
        {
            _employeeRepository = new EmployeeRepository(db);
        }
        // GET: Announcements
        #region Views
        public async Task<ActionResult> Index()
        {
            return View(await db.Announcements.ToListAsync());
        }

        // GET: Announcements/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement announcement = await db.Announcements.FindAsync(id);
            if (announcement == null)
            {
                return HttpNotFound();
            }
            return View(announcement);
        }

        // GET: Announcements/Create
        public ActionResult Create()
        {
            var model = new Announcement();
            model.CreatedAt = DateTime.Now;
            model.LastUpdatedAt = DateTime.Now;
            return View();
        }

        // POST: Announcements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,EmployeeID,EmployeeName,Title,Body,Deleted,CreatedAt,LastUpdatedAt")] Announcement announcement)
        {
            if (string.IsNullOrEmpty(announcement.Title) || string.IsNullOrEmpty(announcement.Body))
            {
                ViewBag.Error = "Form was not Completed Properly";
                return View(announcement);
            }
            var id = User.Identity.GetUserId();
            var user = _employeeRepository.GetAll().Where(x => x.UserId == id).FirstOrDefault();
            if(user == null)
            {
                return View("Error");
            }
             announcement.EmployeeID = user.EmployeeNumber;
             announcement.EmployeeName = user.FullName;
             announcement.CreatedAt = DateTime.Now;
             announcement.LastUpdatedAt = DateTime.Now;
             db.Announcements.Add(announcement);
                await db.SaveChangesAsync();
                return RedirectToAction("Dashboard","Admin");
            
            
        }

        // GET: Announcements/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement announcement = await db.Announcements.FindAsync(id);
            if (announcement == null)
            {
                return HttpNotFound();
            }
            return View(announcement);
        }

        // POST: Announcements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,EmployeeID,EmployeeName,Title,Body,Deleted,CreatedAt,LastUpdatedAt")] Announcement announcement)
        {
            if (string.IsNullOrEmpty(announcement.Title) || string.IsNullOrEmpty(announcement.Body))
            {
                ViewBag.Error = "Form was not Completed Properly";
                return View(announcement);
            }
            if (ModelState.IsValid)
            {
                announcement.LastUpdatedAt = DateTime.Now;
                db.Entry(announcement).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Error = "Retry Again";
            return View(announcement);
        }

        // GET: Announcements/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement announcement = await db.Announcements.FindAsync(id);
            if (announcement == null)
            {
                return HttpNotFound();
            }
            return View(announcement);
        }

        // POST: Announcements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Announcement announcement = await db.Announcements.FindAsync(id);
            db.Announcements.Remove(announcement);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        #endregion
        #region PartialViews
        [ChildActionOnly]
        public ActionResult _GetLatestAnnouncements()
        {
            var announcements = db.Announcements.Where(x => x.Deleted != true).Take(5).ToList();
            return PartialView(announcements);
        }
        #endregion
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
