using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using TalentAcquisition.Core.Domain;
namespace TalentAcquisition.Models.ViewModel
{
    public class AssignToGroupViewModel
    {
        [Key]
        public int MyProperty { get; set; }
        public AssignToGroupViewModel()
        {
            Priviledges = new List<SelectPriviledge>();
            Employees = new List<Employee>();
        }
        public AssignToGroupViewModel(List<ApplicationRole> allroles, List<ApplicationRole> roles, List<Employee> employees)
        {
            this.Priviledges = ConvertRolesToPriviledges(allroles, roles);
            this.Employees = employees;
        }
        private List<SelectPriviledge> ConvertRolesToPriviledges(List<ApplicationRole> allroles, List<ApplicationRole> roles)
        {
            List<SelectPriviledge> _priviledges = new List<SelectPriviledge>();
            foreach (var role in allroles)
            {
                if (roles.Any(x => x.Id == role.Id))
                {
                    _priviledges.Add(new SelectPriviledge { Selected = true, Name = role.Name });
                }
                else
                {
                    _priviledges.Add(new SelectPriviledge { Selected = false, Name = role.Name });
                }
            }
            return _priviledges;
        }
        public Group Group { get; set; }
        public List<SelectPriviledge> Priviledges { get; private set; }
        public List<Employee> Employees { get; private set; }
    }
    public class SelectPriviledge
    {
        public bool Selected { get; set; }
        [Key]
        public string Name { get; set; }
    }
    public class AssignEmployeeToGroupViewModel
    {
        [Key]
        public int MyProperty { get; set; }
        public AssignEmployeeToGroupViewModel()
        {
            Groups = new List<Group>();
            Employees = new List<Employee>();
        }
        public AssignEmployeeToGroupViewModel(List<Employee> employees, List<Group> groups)
        {
            this.Employees = employees;
            this.Groups = groups;
        }
        [Display(Name ="Group Name")]
        public int GroupID { get; set; }
        [Display(Name = "Employee Name")]
        public int EmployeeID { get; set; }
        public List<Employee> Employees { get; private set; }
        public List<Group> Groups { get; private set; }
    }
}