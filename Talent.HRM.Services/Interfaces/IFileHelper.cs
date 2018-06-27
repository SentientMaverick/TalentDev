using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Talent.HRM.Services.Interfaces
{
   public interface IFileHelper
    {
        Task<bool> CreateFolder();
        Task<bool> UploadSingleFileAsync(HttpPostedFileBase file,string filename,string serverpath);
    }
}
