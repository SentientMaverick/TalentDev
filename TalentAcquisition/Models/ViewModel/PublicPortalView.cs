using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TalentAcquisition.Models.ViewModel
{
    public class PublicPortalLoginView
    {
        [Key]
        public int ID { get; set; }
    }

    public class PublicPortalCreateProfileView
    {
        [Key]
        public int ID { get; set; }
    }
    public class PublicPortalJobListingView
    {
        [Key]
        public int ID { get; set; }
        public ICollection<JobListing> JobListings;
    }
    public class JobListing
    {
        public int ID { get; set; }
    }
}