using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TalentAcquisition.Core.Domain
{
    public class Department
    {
        public Department()
        {

        }
       // [Key]
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<OfficePosition> OfficePositions { get; set; }
    }
    public class Industry
    {
        public Industry()
        {

        }
        public int IndustryId { get; set; }
        public string Name { get; set; }

    }
}