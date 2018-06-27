using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TalentAcquisition.Core.Domain;

namespace TalentAcquisition.BusinessLogic.UpdatedDomain
{
    public class AppraisalGrade
    {
        public string GradeName { get; set; }
        public string Gradevalue { get; set; }
    }
    public class AppraisalCategory
    {
        [Key]
        public string Code { get; set; }
        public string Description { get; set; }
    }
    public class AppraisalTemplate
    {
        public Guid Id { get; set; }
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<Appraisal> AppraisalLines { get; set; }
    }
    public class Appraisal
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string OrderNumber { get; set; }
        public string Question { get; set; }
        public string QuestionType { get; set; }
        public string GradeCode { get; set; }
        public string GradeCategory { get; set; }
    }
}