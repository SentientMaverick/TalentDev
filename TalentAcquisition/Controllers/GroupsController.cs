using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TalentAcquisition.Core.Domain;
using TalentAcquisition.DataLayer;
using TalentAcquisition.Models.ViewModel;
using TalentAcquisition.Repositories;
using TalentAcquisition.Repositories.Interfaces;

namespace TalentAcquisition.Controllers
{
    public class GroupsController : Controller
    {
        private TalentContext db = new TalentContext();
        private IdentityManager _manager = new IdentityManager();
        private IEmployeeRepository _repo;

        public GroupsController()
        {
            _repo = new EmployeeRepository(db);
        }
        [Route("Groups/All")]
        // GET: Groups
        public ActionResult Index()
        {
            return View(db.Groups.ToList());
        }
        // GET: Groups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            ManageGroupViewModel model = new ManageGroupViewModel();
            model.Group = group;
            model.Members = _manager.getEmployeesInGroup(id).Select(x=>new GroupMember { Id=x.ID,Number=x.EmployeeNumber,Name=x.FullName}).ToList();
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }
        public ActionResult Create()
        {
            return View();
        }
        // GET: Groups/Create
        public ActionResult AssignRoleToGroup(int id)
        {
            if (id == 0)
            {
                return HttpNotFound();
            }
            var group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            var employeelist = db.Employees.ToList();
            var groups = db.Groups.ToList();

            var allroles = new List<ApplicationRole>();
            ViewBag.Employee = new SelectList(employeelist, "ID", "FirstName");

            allroles = db.ApplicationRoles.ToList();
            var broles = db.ApplicationRoleGroups.Where(x => x.GroupId == id).ToList();
            List<ApplicationRole> roles = broles.Select(x => new ApplicationRole
            { Id = x.RoleId, Name = x.Role.Name, Description = x.Role.Description }).ToList();
            AssignToGroupViewModel model = new AssignToGroupViewModel(allroles, roles, employeelist);
            model.Group = group;
            return View(model);
        }
        [HttpPost]
        public ActionResult AssignRoleToGroup(int id, AssignToGroupViewModel model, List<SelectPriviledge> priviledges)
        {
            if (id == 0)
            {
                return HttpNotFound();
            }
            var group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }

            if (!ModelState.IsValid)
            {
                IdentityManager _manager = new IdentityManager();
                _manager.ClearGroupRoles(id);
                foreach (var priviledge in model.Priviledges)
                {
                    if (priviledge.Selected)
                    {
                        _manager.AddRoleToGroup(id, priviledge.Name);
                    }
                }
                return RedirectToAction("AssignRoleToGroup/" + id);
            }
            return View(model);
        }
        [Route("Permissions/Groups/AddUser")]
        public ActionResult AssignEmployeeToGroup(int? id)
        {
            var employeelist = _repo.GetAll().ToList();
            var groups = db.Groups.ToList();
            AssignEmployeeToGroupViewModel model = new AssignEmployeeToGroupViewModel(employeelist, groups);
           
            if (id != null)
            {
                model.EmployeeID = (int)id;
                ViewBag.Employee = new SelectList(employeelist, "ID", "FirstName", id);
            }
            else
            {
                ViewBag.Employee = new SelectList(employeelist, "ID", "FirstName");
            }
            return View(model);
        }
        [HttpPost]
        [Route("Permissions/Groups/AddUser")]
        public ActionResult AssignEmployeeToGroup(AssignEmployeeToGroupViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IdentityManager _manager = new IdentityManager();
                    _manager.AddUserToGroupUpdated(model.EmployeeID, model.GroupID);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }
        [Route("Groups/RemoveUser/{id}")]
        public ActionResult RemoveUserFromGroup(int id,int employeeid)
        {
            _manager.RemoveUserFromGroup(employeeid,id);
            return RedirectToAction("Details/"+ id, "Groups");
        }
        [Route("Permissions/Groups/ManageUser")]
        public ActionResult ManageEmployeeGroup(int? id)
        {
            var employeelist = _repo.GetAll().ToList();
            var groups = db.Groups.ToList();
            AssignEmployeeToGroupViewModel model = new AssignEmployeeToGroupViewModel(employeelist, groups);

            if (id != null)
            {
                model.EmployeeID = (int)id;
                ViewBag.Employee = new SelectList(employeelist, "ID", "FirstName", id);
            }
            else
            {
                ViewBag.Employee = new SelectList(employeelist, "ID", "FirstName");
            }
            return View(model);
        }
        [HttpPost]
        [Route("Permissions/Groups/ManageUser")]
        public ActionResult ManageEmployeeGroup(AssignEmployeeToGroupViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IdentityManager _manager = new IdentityManager();
                    _manager.AddUserToGroupUpdated(model.EmployeeID, model.GroupID);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Groups.Add(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(group);
        }
        // GET: Groups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }
        // POST: Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(group);
        }
        // GET: Groups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }
        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _manager.DeleteGroup(id);
            //Group group = db.Groups.Find(id);
            //db.Groups.Remove(group);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }
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