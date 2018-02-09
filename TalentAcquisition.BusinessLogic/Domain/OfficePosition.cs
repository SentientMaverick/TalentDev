using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TalentAcquisition.Core.Domain
{
    public class OfficePosition
    {

        public int OfficePositionID { get; set; }
     //  public int RoleID { get; set; }
       public int DepartmentID { get; set; }
        public int IndustryID { get; set; }
        public string Title { get; set; }
        public string RoleSummary { get; set; }
        [Display(Name ="Role Functions")]
        public string Reqirements { get; set; }
        //public string RoleNumber { get; set; }
        public bool IsAvailable
        { get {
                return false;
            }
            set { }
        }
       public virtual Department Department { get; set; }
       public virtual Industry Industry { get; set; }
    }
    /*
    public class Role
    {
        public int RoleID { get; set; }
       // public string Title { get; set; }
        public string RoleName { get; set; }
        public string RoleSummary { get; set; }
        public string Reqirements { get; set; }
    }*/
}