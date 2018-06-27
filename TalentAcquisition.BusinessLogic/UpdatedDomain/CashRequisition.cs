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
            Id = Guid.NewGuid();
        }
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
    public class CashRequisition
    {
        public CashRequisition()
        {

        }
        [Key]
        public string No { get; set; }
        public string Type { get; set; }
        public int RequestCreator { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string BranchCode { get; set; }
        public string DeptCode { get; set; }
        public string Status { get; set; }
    }
}