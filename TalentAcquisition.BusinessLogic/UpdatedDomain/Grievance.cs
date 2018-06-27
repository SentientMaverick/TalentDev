using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TalentAcquisition.BusinessLogic.UpdatedDomain
{
    public class GrievanceType
    {
        [Key]
        public string Code { get; set; }
        public string Description { get; set; }
    }
    public class GrievanceAction
    {
        [Key]
        public string Code { get; set; }
        public string Description { get; set; }
    }
    public class GrievanceReport
    {
        [Key]
        public string No { get; set; }
        public string EmployeeNumber { get; set; }
        public string EmployeeName { get; set; }
        public string GrievanceCode { get; set; }
        public string GrievanceDescription { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateApproved { get; set; }
        public string OffenderCode { get; set; }
        public string OffenderName { get; set; }
        public string Document { get; set; }
    }
}