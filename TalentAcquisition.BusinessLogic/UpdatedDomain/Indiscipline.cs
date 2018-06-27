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
    public class DisciplinaryCaseDetail
    {
        public string CaseNumber { get; set; }
        public DateTime ComplaintDate { get; set; }
        public string IndisciplineTypeCode { get; set; }
        public string Discussion { get; set; }
        public string Status { get; set; }
        public virtual IndisciplineType IndisciplineType { get; set; }
    }
    public class Indiscipline
    {
    }
}