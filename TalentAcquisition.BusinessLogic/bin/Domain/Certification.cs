using System;
using System.ComponentModel.DataAnnotations;

namespace TalentAcquisition.Core.Domain
{
    public class Certification
    {
        public int ID { get; set; }
        public int JobSeekerID { get; set; }
        [Required]
       // [DataType(DataType.)]
        public string Title { get; set; }
        [Required]
        public DateTime Year { get; set; }
        public virtual JobSeeker JobSeeker { get; set; }
    }
    public class ProfessionalCertification
    {
        public int ID { get; set; }
        public int EmployeeID { get; set; }
        [Required]
        // [DataType(DataType.)]
        public string Title { get; set; }
        [Required]
        public DateTime Year { get; set; }
        public virtual Employee Employee { get; set; }
    }
}