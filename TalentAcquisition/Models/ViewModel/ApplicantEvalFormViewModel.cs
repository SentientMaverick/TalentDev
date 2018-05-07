using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TalentAcquisition.Models.ViewModel
{
    public class ApplicantEvalFormViewModel
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string HighestQualification { get; set; }
        public string PositionAppliedFor { get; set; }
        public List<string> skills { get; set; }
    }
}