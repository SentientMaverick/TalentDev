using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TalentAcquisition.BusinessLogic.UpdatedDomain
{
    public class LeaveType_Limit
    {
        public int ID { get; set; }
        public  string LeaveType { set; get; }
        public int Limit { get; set; }
        public bool RequiresPlan { set; get; }
        public virtual ICollection<ManageEmployeeLeave> ManageEmployeesLeave { get; set; }       
    }
}