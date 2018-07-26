using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TalentAcquisition.Core.Domain;

namespace TalentAcquisition.Models.ViewModel
{
    public class ManageGroupViewModel
    {
        public Group Group { get; set; }
        public List<GroupMember> Members { get; set; }
    }
    public class GroupMember
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
    }
}