using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TalentAcquisition.Core.Domain
{
    public class Employee:Person
    {
        public Employee()
        {
            this.Groups = new HashSet<ApplicationUserGroup>();
        }
        [Display(Name ="Employee Number")]
        public string EmployeeNumber { get; set; }
        [Display(Name = "Employement Date")]
        public DateTime EmploymentDate { get; set; }
        [Display(Name = "Role")]
        public int OfficePositionID { get; set; }
        [Display(Name = "Nationality")]
        public string Nationality { get; set; }
        [Display(Name = "Marriage Status")]
        public string MaritalStatus { get; set; }
        [Display(Name = "Next Of Kin - Name")]
        public string NextofKinName { get; set; }
        [Display(Name = "Next Of Kin - Relationship")]
        public string NextofKinRelationship { get; set; }
        [Display(Name = "Next Of Kin - Address")]
        public string NextofKinAddress { get; set; }
        [Display(Name = "Next Of Kin - Contact")]
        public string NextofKinContact { get; set; }
        [Display(Name = "Passport Number")]
        public string PassportNumber { get; set; }
        [Display(Name = "Tax Identification Number")]
        public string TIN { get; set; }
        public bool Disabled { get; set; }
        public virtual ICollection<WorkHistory> WorkHistorys { get; set; }
        public virtual ICollection<BusinessLogic.Domain.Skill> Skills { get; set; }
        public virtual ICollection<ProfessionalCertification> ProfessionalCertifications { get; set; }
        public virtual ICollection<JobApplication> JobApplications {get;set;}
        public virtual OfficePosition OfficePosition { get; set; }
        public virtual ICollection<Interview> Interviews { get; set; }
        public virtual ICollection<JobRequisition> JobRequisitions { get; set; }
        public virtual ICollection<ApplicationUserGroup> Groups { get; set; }
    }
    public class ApplicationUserGroup
    {
        [Required]
        public virtual int EmployeeID { get; set; }
        [Required]
        public virtual int GroupId { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Group Group { get; set; }
    }
    public class ApplicationRole
    {
        public ApplicationRole() : base() { }


        public ApplicationRole(string name, string description)
        {
            this.Description = description;
            this.Name = name;
        }
        public virtual string Description { get; set; }
        public virtual string Name { get; set; }
        [Key]
        public virtual string Id { get; set; }
    }
    public class ApplicationRoleGroup
    {
        public virtual string RoleId { get; set; }
        public virtual int GroupId { get; set; }

        public virtual ApplicationRole Role { get; set; }
        public virtual Group Group { get; set; }
    }
    public class Group
    {
        public Group()
        {
        }


        public Group(string name) : this()
        {
            Roles = new List<ApplicationRoleGroup>();
            Name = name;
        }


        [Key]
        [Required]
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }
        public virtual ICollection<ApplicationRoleGroup> Roles { get; set; }
    }
}