using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TalentAcquisition.Models.ViewModel
{
    public class OfferLetterViewModel
    {
        [Key]
        public int requisitionid { get; set; }
        public int applicationid { get; set; }
        public DateTime employmentdate { get; set; }
        public DateTime deadlinedate { get; set; }
        [AllowHtml]
        public string offermessage { get; set; }
        public string position { get; set; }
        public string salary { get; set; }
    }
}