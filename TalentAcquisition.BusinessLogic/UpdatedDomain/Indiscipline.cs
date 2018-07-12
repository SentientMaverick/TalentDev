using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TalentAcquisition.BusinessLogic.UpdatedDomain
{
    public class IndisciplineType
    {
        [Key]
        public string Code { get; set; }
        public string Description { get; set; }
    }
    public class DisciplinaryAction
    {
        [Key]
        public string Code { get; set; }
        public string Description { get; set; }
    }
    public class DisciplinaryCase
    {
        [Key]
        public Guid Id { get; set; }
        [Display(Name = "Case Number")]
        public string CaseNumber { get; set; }
        [Display(Name = "Employee Number")]
        public string EmployeeNumber { get; set; }
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }
        [Display(Name ="Job Title Code")]
        public string JobTitleCode { get; set; }
        [Display(Name = "Job Title Name")]
        public string JobTitleName { get; set; }
        [Display(Name = "Date")]
        public DateTime ComplaintDate { get; set; }
        [Display(Name = "Indiscipline Type")]
        public string IndisciplineTypeCode { get; set; }
        [Display(Name = "Disciplinary Action Code")]
        public string DisciplinaryActionCode { get; set; }
        [Display(Name = "Action Details")]
        public string ActionDetails { get; set; }
        [Display(Name = "Action Start Date")]
        public DateTime ActionStartDate { get; set; }
        [Display(Name = "Action End Date")]
        public DateTime ActionEndDate { get; set; }
        public string Reasons { get; set; }
        public bool Posted { get; set; }
        public virtual IndisciplineType IndisciplineType { get; set; }
        public virtual DisciplinaryAction DisciplinaryAction { get; set; }
    }
}