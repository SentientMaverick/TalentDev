using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TalentAcquisition.BusinessLogic.UpdatedDomain
{
    public class Branch
    {
        [Key]
        public string BranchId { get; set; }
        [Required]
        public string Location { get; set; }
        public Branch()
        {
            //Guid id = new Guid();
        }
    }
}