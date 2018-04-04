using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TalentAcquisition.Core.Domain;

namespace TalentAcquisition.BusinessLogic.UpdatedDomain
{
    public class MatchedApplicant
    {
        [Key]
        public int ID { get; set; }
        public int JobRequisitionID { get; set; }
        public int JobSeekerID { get; set; }
        public virtual JobSeeker JobSeeker { get; set; }
        public virtual JobRequisition JobRequisition { get; set; }
    }
}