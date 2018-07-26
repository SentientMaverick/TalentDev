using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TalentAcquisition.BusinessLogic.Domain;
using TalentAcquisition.BusinessLogic.UpdatedDomain;
using TalentAcquisition.Core.Domain;

namespace TalentAcquisition.DataLayer
{

    public class IdentityManager
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private readonly TalentContext db = new TalentContext();

        private readonly RoleManager<IdentityRole> _roleManager = new RoleManager<IdentityRole>(
            new RoleStore<IdentityRole>(new ApplicationDbContext()));

        private readonly UserManager<ApplicationUser> _userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(new ApplicationDbContext()));

        public bool RoleExists(string name)
        {
            return _roleManager.RoleExists(name);
        }
        public IdentityResult CreateRole(string name, string description = "")
        {
            // Swap ApplicationRole for IdentityRole:
            //_roleManager.Create()
            // return _roleManager.Create(new ApplicationRole(name, description));
            return _roleManager.Create(new IdentityRole(name));
        }

        public List<Employee> getEmployeesInGroup(int? groupId)
        {
            using (var db = new TalentContext())
            {
                Group group = db.Groups.Find(groupId);
                // IQueryable<ApplicationUser> groupUsers = _db.Users.Where(u => u.Groups.Any(g => g.GroupId == group.Id));
                List<Employee> groupUsers = db.Employees.Where(u => u.Groups.Any(g => g.GroupId == group.Id)).ToList();
                return groupUsers;
            }
        }
        public string CreateRoleReturnString(string name)
        {
            // Swap ApplicationRole for IdentityRole:
            //_roleManager.Create()
            // return _roleManager.Create(new ApplicationRole(name, description));
            string roleid = null;
            var role = new IdentityRole(name);
            var action = _roleManager.Create(role);
            if (action.Succeeded)
            {
                return role.Id.ToString();
            }
            return roleid;
        }
        public IdentityResult CreateUser(ApplicationUser user, string password)
        {
            return _userManager.Create(user, password);
        }
        public IdentityResult AddUserToRole(string userId, string roleName)
        {
            return _userManager.AddToRole(userId, roleName);
        }
        public void ClearUserRoles(string userId)
        {
            ApplicationUser user = _userManager.FindById(userId);
            var currentRoles = new List<IdentityUserRole>();

            currentRoles.AddRange(user.Roles);
            foreach (IdentityUserRole role in currentRoles)
            {
                var rolename = _roleManager.Roles.Where(x => x.Id == role.RoleId).First().Name;
                _userManager.RemoveFromRole(userId, rolename);
            }
        }
        public void RemoveFromRole(string userId, string roleName)
        {
            _userManager.RemoveFromRole(userId, roleName);
        }
        public void DeleteRole(string roleId)
        {
            IQueryable<ApplicationUser> roleUsers = _db.Users.Where(u => u.Roles.Any(r => r.RoleId == roleId));
            // ApplicationRole role = db.ApplicationRoles.Find(roleId);
            IdentityRole role = _db.Roles.Find(roleId);

            foreach (ApplicationUser user in roleUsers)
            {
                RemoveFromRole(user.Id, role.Name);
            }
            _db.Roles.Remove(role);
            _db.SaveChanges();
        }
        public void CreateGroup(string groupName)
        {
            if (GroupNameExists(groupName))
            {
                throw new GroupExistsException(
                    "A group by that name already exists in the database. Please choose another name.");
            }

            var newGroup = new Group(groupName);
            db.Groups.Add(newGroup);
            _db.SaveChanges();
        }
        public bool GroupNameExists(string groupName)
        {
            return db.Groups.Any(gr => gr.Name == groupName);
        }
        public void ClearUserGroups(string userId)
        {
            ClearUserRoles(userId);
            ApplicationUser user = _db.Users.Find(userId);
            Employee employee = db.Employees.Where(x => x.UserId == userId).First();
            employee.Groups.Clear();
            // user.Groups.Clear();
            db.SaveChanges();
            _db.SaveChanges();
        }
        public void AddUserToGroup(string userId, int groupId)
        {
            Group group = db.Groups.Find(groupId);
            ApplicationUser user = _db.Users.Find(userId);

            //var userGroup = new ApplicationUserGroup
            //{
            //    Group = group,
            //    GroupId = group.Id,
            //    User = user,
            //    UserId = user.Id
            //};

            foreach (ApplicationRoleGroup role in group.Roles)
            {
                _userManager.AddToRole(userId, role.Role.Name);
            }
            // user.Groups.Add(userGroup);
            _db.SaveChanges();
        }
        public void AddUserToGroupUpdated(int id, int groupId)
        {
            Group group = db.Groups.Find(groupId);
            Employee employee = db.Employees.Find(id);

            ApplicationUser user = _db.Users.Find(employee.UserId);


            var userGroup = new ApplicationUserGroup
            {
                Group = group,
                GroupId = group.Id,
                Employee = employee,
                EmployeeID = id
            };

            foreach (ApplicationRoleGroup role in group.Roles)
            {
                _userManager.AddToRole(employee.UserId, role.Role.Name);
            }
            employee.Groups.Add(userGroup);
            // user.Groups.Add(userGroup);
            db.SaveChanges();
            _db.SaveChanges();
        }
        public void RemoveUserFromGroup(int id, int groupId)
        {
            Group group = db.Groups.Find(groupId);
            Employee employee = db.Employees.Find(id);

            ApplicationUser user = _db.Users.Find(employee.UserId);

            var userGroup = db.ApplicationUserGroups.Where(x => x.GroupId == groupId && x.EmployeeID == id).First();
            foreach (ApplicationRoleGroup role in group.Roles)
            {
                List<ApplicationUserGroup> ff = db.ApplicationUserGroups
                    .Where(r => r.GroupId == groupId && r.EmployeeID == id
                         && r.Group.Roles.Any(o => o.RoleId ==role.RoleId)).ToList();
                int groupsWithRole = ff.Count();

                if (groupsWithRole == 1)
                {
                    RemoveFromRole(employee.UserId, role.Role.Name);
                }
               // _userManager.RemoveFromRole(employee.UserId, role.Role.Name);
            }
            employee.Groups.Remove(userGroup);
            db.SaveChanges();
            _db.SaveChanges();
        }
        public void DeleteGroup(int groupId)
        {
            Group group = db.Groups.Find(groupId);

            // Clear the roles from the group:
            ClearGroupRoles(groupId);
            db.Groups.Remove(group);
            db.SaveChanges();
        }
        public void AddRoleToGroup(int groupId, string roleName)
        {
            Group group = db.Groups.Find(groupId);
            IdentityRole irole = _db.Roles.FirstOrDefault(r => r.Name == roleName);
            ApplicationRole role = db.ApplicationRoles.First(r => r.Name == roleName);

            var newgroupRole = new ApplicationRoleGroup
            {
                GroupId = group.Id,
                Group = group,
                RoleId = role.Id,
                Role = role
            };

            // make sure the groupRole is not already present
            //if (!group.Roles.Contains(newgroupRole))
            if (!group.Roles.Any(x => x.GroupId == newgroupRole.GroupId && x.RoleId == role.Id))
            {
                group.Roles.Add(newgroupRole);
                db.SaveChanges();
            }

            // Add all of the users in this group to the new role:
            // IQueryable<ApplicationUser> groupUsers = _db.Users.Where(u => u.Groups.Any(g => g.GroupId == group.Id));
            IQueryable<Employee> groupUsers = db.Employees.Where(u => u.Groups.Any(g => g.GroupId == group.Id));
            foreach (Employee user in groupUsers)
            {
                if (!(_userManager.IsInRole(user.UserId, roleName)))
                {
                    AddUserToRole(user.UserId, role.Name);
                }
            }
        }
        public IdentityResult DisableUserCredentials(Employee employee)
        {
            var user = _userManager.FindById(employee.UserId);
            if (user != null)
            {
                ClearUserGroups(employee.UserId);
                return _userManager.Delete(user);
            }
            return new IdentityResult("Dereferenced User");
        }
        public void ClearGroupRoles(int groupId)
        {
            using (var db = new TalentContext())
            {
                Group group = db.Groups.Find(groupId);
                // IQueryable<ApplicationUser> groupUsers = _db.Users.Where(u => u.Groups.Any(g => g.GroupId == group.Id));
                List<Employee> groupUsers = db.Employees.Where(u => u.Groups.Any(g => g.GroupId == group.Id)).ToList();

                foreach (ApplicationRoleGroup role in group.Roles)
                {
                    string currentRoleId = role.RoleId;
                    foreach (Employee user in groupUsers)
                    {
                        // Is the user a member of any other groups with this role?
                        //int groupsWithRole = user.Groups.Count(g => g.Group.Roles.Any(r => r.RoleId == currentRoleId));
                        List<ApplicationUserGroup> ff = db.ApplicationUserGroups.Where(r => r.GroupId == groupId && r.EmployeeID == user.ID && r.Group.Roles.Any(o => o.RoleId == currentRoleId)).ToList();
                        // var kk = db.Employees.First(x => x.ID == user.ID).Groups;
                        //.Roles.Any(r => r.RoleId == currentRoleId);
                        int groupsWithRole = ff.Count();

                        // This will be 1 if the current group is the only one:
                        if (groupsWithRole == 1)
                        {
                            RemoveFromRole(user.UserId, role.Role.Name);
                        }
                    }
                }
                group.Roles.Clear();
                db.SaveChanges();
                _db.SaveChanges();
            }
        }
    }
    [Serializable]
    public class GroupExistsException : Exception
    {
        public GroupExistsException()
        {
        }

        public GroupExistsException(string message) : base(message)
        {
        }

        public GroupExistsException(string message, Exception inner) : base(message, inner)
        {
        }

        protected GroupExistsException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}