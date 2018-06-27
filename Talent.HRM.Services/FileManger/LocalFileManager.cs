using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Talent.HRM.Services.Interfaces;

namespace Talent.HRM.Services.FileManger
{
   public class LocalFileManager:IFileHelper
    {
        public async Task<bool> CreateFolder()
        {
            try
            {
                await Task.Delay(100);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> UploadSingleFileAsync(HttpPostedFileBase file,string filename,string serverpath)
        {
            try
            {
                var fullname = Path.Combine(serverpath, filename);
                file.SaveAs(fullname);
                await Task.Delay(100);
                return true;
            }
            catch
            {
                return false;
            }
        }
     }
}
