using System;

namespace TalentAcquisition.Core.Domain
{
    public class WorkExperience
    {
        public int ID { get; set; }
        public int JobSeekerID { get; set; }
        public string CompanyName { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string ReasonsForLeaving { get; set; }
        public DateTime StartingDate{ get; set; }
        public DateTime? EndingDate { get; set; }
        public virtual JobSeeker JobSeeker { get; set; }
    }
}