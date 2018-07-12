using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TalentAcquisition.Core.Domain;

namespace TalentAcquisition.BusinessLogic.UpdatedDomain
{
    public class AppraisalType
    {
        [Key]
        public string Code { get; set; }
        public string Description { get; set; }
    }
    public class AppraisalGrade
    {
        [Key]
        public string Name { get; set; }
        public string Value { get; set; }
    }
    public class AppraisalCategory
    {
        [Key]
        public string Code { get; set; }
        public string Description { get; set; }
    }
    public class AppraisalPeriod
    {
        [Key]
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }   
        public bool Closed { get; set; }
    }
    public class KPIGroup
    {
        [Key]
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
    public class Appraisal
    {
        public Appraisal()
        {
            this.Lines = new HashSet<AppraisalKPI>();
        }
        [Key]
        public string No { get; set; }
        [Display(Name = "Appraisal Type Code")]
        public string AppraisalTypeCode { get; set; }
        [Display(Name = "Appraisal KPI Code")]
        public string AppraisalNonJobKPICode { get; set; }
        [Display(Name = "Appraisal Class")]
        public string AppraisalClass { get; set; }
        [Display(Name = "Appraisal Period Code")]
        public string AppraisalPeriodCode { get; set; }
        [Display(Name = "Appraisee Code")]
        public string AppraiseeCode { get; set; }
        [Display(Name = "Appraisal Supervisor")]
        public string AppraisalSupervisor { get; set; }
        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }
        [Display(Name = "Appraisee Name")]
        public string AppraisalName { get; set; }
        [Display(Name = "Appraisee Job Title")]
        public string AppraisalJobTitle { get; set; }
        [Display(Name = "Appraisal Supervisor Name")]
        public string AppraisalSupervisorName { get; set; }
        public string Status { get; set; }
        public virtual AppraisalType AppraisalType { get; set; }
        public virtual AppraisalNonJobKPI AppraisalNonJobKPI { get; set; }
        public virtual AppraisalPeriod AppraisalPeriod { get; set; }
        public virtual ICollection<AppraisalKPI> Lines { get; set; }
    }
    public class AppraisalKPI
    {
        public string AppraisalNo { get; set; }
        public string JobKPIId { get; set; }
        public JobKPI JobKPI { get; set; }
        public Appraisal Appraisal { get; set; }

    }
    public class AppraisalNonJobKPI
    {
        [Key]
        public string Code { get; set; }
        public string Description { get; set; }
        [Display(Name = "Appraisal Class")]
        public string AppraisalClass { get; set; }
        public virtual ICollection<NonJobKPI> Lines { get; set; }
    }
    public class NonJobKPI
    {
        [Key]
        public Guid Id { get; set; }
        [Display(Name = "Header Code")]
        public string AppraisalNonJobKPICode { get; set; }
        [Display(Name = "Appraisal Class")]
        public string AppraisalClass { get; set; }
        [Display(Name = "KPI Code")]
        public string Code { get; set; }
        [Display(Name = "KPI Description")]
        public string Description { get; set; }
        [Display(Name = "Maximum Score")]
        public int MaxScore { get; set; }
        public virtual AppraisalNonJobKPI AppraisalNonJobKPI { get; set; }
    }
    public class JobKPI
    {
        [Key]
        public Guid Id { get; set; }
        public string AppraisalNo { get; set; }
        [Display(Name = "KPI Group")]
        public string KPIGroupCode { get; set; }
        [Display(Name = "KPI Group Name")]
        public string KPIGroupName { get; set; }
        [Display(Name = "Competency Code")]
        public string Code { get; set; }
        [Display(Name = "Competency Description")]
        public string Description { get; set; }
        [Display(Name = "Competency Target")]
        public decimal MaxScore { get; set; }
        [Display(Name = "Employee Rating")]
        public decimal EmployeeRating { get; set; }
        [Display(Name = "Employee Remark")]
        public string EmployeeRemark { get; set; }
        [Display(Name = "Actual Rating")]
        public decimal SupervisorRating { get; set; }
        [Display(Name = "Supervisor Remark")]
        public string SupervisorRemark { get; set; }
        [Display(Name = "Performance %")]
        public decimal Performance { get; set; }
        public virtual ICollection<AppraisalKPI> Appraisals { get; set; }
    }
}