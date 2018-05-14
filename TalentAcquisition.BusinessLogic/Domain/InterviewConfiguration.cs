using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TalentAcquisition.Core.Domain
{
    public class InterviewConfiguration
    {
        public InterviewConfiguration()
        {
            Interviews = new HashSet<Interview>();
        }
        public int ID { get; set; }
        public int JobApplicationID { get; set; }
        public JobApplication JobApplication { get; set; }
        public ICollection<Interview> Interviews { get; set; }
    }
}