using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TalentAcquisition.Core.Domain;

namespace TalentAcquisition.BusinessLogic.Domain
{
    public class Skill
    {
        public Skill()
        {
            JobRequisitions = new HashSet<JobRequisition>();
            JobSeekers = new HashSet<JobSeeker>(); 
        }

        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int? IndustryId { get; set; }
        public virtual Industry Industry { get; set; }
        public virtual ICollection<JobRequisition> JobRequisitions { get; set; }
        public virtual ICollection<JobSeeker> JobSeekers { get; set; }
        //public virtual JobSeeker JobSeeker { get; set; }
        //public virtual JobRequisition JobRequisition { get; set; }
    }
}