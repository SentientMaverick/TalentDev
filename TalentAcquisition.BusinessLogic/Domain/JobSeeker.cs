using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TalentAcquisition.BusinessLogic.Domain;
using TalentAcquisition.BusinessLogic.UpdatedDomain;

namespace TalentAcquisition.Core.Domain
{
    public class JobSeeker:Person
    {
        public JobSeeker()
        {
            JobApplications = new HashSet<JobApplication>();
            Schools = new HashSet<School>();
            WorkExperiences = new HashSet<WorkExperience>();
            Certifications = new HashSet<Certification>();
            Skills = new HashSet<Skill>();
            MatchedApplicants = new HashSet<MatchedApplicant>();

        }
        [Display(Name ="Username")]
        public string ApplicantNumber { get; set; }
        [Display(Name = "Date Of Registration")]
        public DateTime RegistrationDate { get; set; }
        public string  UploadedCVAddress { get; set; }
        public string UploadedPassportAddress { get; set; }
        [Display(Name = "Industry of Choice or Specialization")]
        public int? IndustryID { get; set; }
        public GradeObtained? HighestQualification { get; set; }
        public virtual Industry Industry { get; set; }
        public virtual ICollection<JobApplication> JobApplications { get; set; }
        public virtual ICollection<School> Schools { get; set; }
        public virtual ICollection<WorkExperience> WorkExperiences { get; set; }
        public virtual ICollection<Certification> Certifications { get; set; }
        public virtual ICollection<Skill> Skills { get; set; }
        public virtual ICollection<MatchedApplicant> MatchedApplicants { get; set; }
        // public virtual ICollection<Certification> Uploads { get; set; }

    }
}