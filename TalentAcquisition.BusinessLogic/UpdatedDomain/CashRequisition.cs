using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TalentAcquisition.BusinessLogic.UpdatedDomain
{

    public class CashRequisitionType
    {
        public CashRequisitionType()
        {
        }
        [Key]
        public string Code { get; set; }
        public string Description { get; set; }
    }
    public class CashRequisition
    {
        public CashRequisition()
        {

        }
        [Key]
        [Display(Name = "Requisition No")]
        public string No { get; set; }
        [Display(Name ="Type")]
        public string CashRequisitionTypeCode { get; set; }
        [Display(Name = "Requester")]
        public int RequestCreator { get; set; }
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
        [Display(Name = "Date Modified")]
        public DateTime? DateModified { get; set; }
        [Display(Name = "Branch Code")]
        public string BranchCode { get; set; }
        [Display(Name = "Department Code")]
        public string DepartmentId{ get; set; }
        [Display(Name = "Total Amount")]
        public string TotalAmount { get; set; }
        public string Status { get; set; }
        public  virtual CashRequisitionType CashRequisitionType { get; set; }
        public virtual Core.Domain.Department Department { get; set; }
        public virtual Branch Branch { get; set; }
        public virtual ICollection<CashLineItem> Items { get; set; }
    }

    public class CashLineItem
    {
        public CashLineItem()
        {

        }
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public string CashRequisitionNo { get; set; }
        public virtual CashRequisition CashRequisition { get; set; }
    }
}