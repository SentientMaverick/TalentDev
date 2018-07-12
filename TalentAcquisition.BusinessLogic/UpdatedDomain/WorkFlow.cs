using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TalentAcquisition.BusinessLogic.UpdatedDomain
{
    public class DocumentWorkFlow
    {
        public DocumentWorkFlow()
        {
            NoOfApprovals = 1;
        }
        public int Id { get; set; }
        public int Sender { get; set; }
        [Display(Name ="Process Name")]
        public string ProcessName { get; set; }
        [Display(Name = "No Of Approvals")]
        public int NoOfApprovals { get; set; }
        [Required]
        [Display(Name = "Approver 1")]
        public string Approver1Id { get; set; }
        [Display(Name = "Approver 2")]
        public string Approver2Id { get; set; }
        [Display(Name = "Approver 3")]
        public string Approver3Id { get; set; }
        [Display(Name = "Approver 4")]
        public string Approver4Id { get; set; }
        [Display(Name = "Approver 5")]
        public string Approver5Id { get; set; }
        [Display(Name = "Enabled")]
        public bool IsEnabled { get; set; }
    }
    public class ApprovalEntry
    {
        [Key]
        public int No { get; set; }
        public string ProcessNo { get; set; }
        public string Sender { get; set; }
        public string Approver { get; set; }
        public int Sequence { get; set; }
        public string Status { get; set; }
        public string ProcessType { get; set; }
    }
}