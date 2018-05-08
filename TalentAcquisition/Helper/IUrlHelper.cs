using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentAcquisition.Helper
{
   public interface IUrlHelper
    {
        string GetPathToImage(int applicantid);
        string GetPathToPdf(int applicantid);
        string GetPathToDocument(int applicantid);
    }
}
