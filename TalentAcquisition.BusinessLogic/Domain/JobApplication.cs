using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TalentAcquisition.Core.Domain
{
    public enum ApplicationStatus
    {
        Applied, Screened, Interview, JobOffer, Canceled
    }
    public class JobApplication
    {
        public JobApplication()
        {

        }
        public int JobApplicationID { get; set; }
        public int JobSeekerID { get; set; }
        public int JobRequisitionID { get; set; }
        public ApplicationStatus? ApplicationStatus { get; set; }
        public virtual JobSeeker JobSeeker { get; set; }
        public virtual JobRequisition JobRequisition { get; set; }

    }
}