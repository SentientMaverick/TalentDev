using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TalentAcquisition.Core.Domain
{
    public class Employee:Person
    {
        public Employee()
        {

        }
        public string EmployeeNumber { get; set; }
        public DateTime EmploymentDate { get; set; }
        public int OfficePositionID { get; set; }
        public virtual ICollection<JobApplication> JobApplications {get;set;}
        public virtual OfficePosition OfficePosition { get; set; }
        public virtual ICollection<Interview> Interviews { get; set; }
        public virtual ICollection<JobRequisition> JobRequisitions { get; set; }
    }
}