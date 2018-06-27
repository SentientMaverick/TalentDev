using System;
using System.Collections.Generic;
using System.Web;
using System.Threading.Tasks;
using System.IO;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Azure;

using Talent.HRM.Services.Interfaces;

namespace Talent.HRM.Services.FileManger
{
   public class AzureFileHelper : IFileHelper
    {
        static CloudBlobClient blobClient;
        const string blobContainerName = "webappstoragedotnet-imagecontainer";
        public string[] containernames = new string[] { "Images", "Documents", "ActivityUploads", "Uploads" };
        Dictionary<string, CloudBlobContainer> containers = new Dictionary<string, CloudBlobContainer>();
        static CloudBlobContainer blobContainer;
        public  AzureFileHelper()
        {
            if (blobContainer == null)
            {
                string StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=codelabstorage;AccountKey=ZhDqmcMh0hvBojC+BX0o0s2OYUuC+50VttPcbldMcYCUyhXwHNrM0rvPg/9d2PTMtXyOqHrhvWPxG5wijjADMw==;EndpointSuffix=core.windows.net";
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageConnectionString);
                blobClient = storageAccount.CreateCloudBlobClient();
                blobContainer = blobClient.GetContainerReference(blobContainerName);
                blobContainer.CreateIfNotExistsAsync().Wait();
            }
        }
        private string GetRandomBlobName(string filename)
        {
            string ext = Path.GetExtension(filename);
            return string.Format("{0:10}_{1}{2}", DateTime.Now.Ticks, Guid.NewGuid(), ext);
        }
        public async Task<bool> CreateFolder()
        {
            string StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=codelabstorage;AccountKey=ZhDqmcMh0hvBojC+BX0o0s2OYUuC+50VttPcbldMcYCUyhXwHNrM0rvPg/9d2PTMtXyOqHrhvWPxG5wijjADMw==;EndpointSuffix=core.windows.net";
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(StorageConnectionString);
                // Create a blob client for interacting with the blob service.
                blobClient = storageAccount.CreateCloudBlobClient();
                blobContainer = blobClient.GetContainerReference(blobContainerName);
                await blobContainer.CreateIfNotExistsAsync();

                // To view the uploaded blob in a browser, you have two options. The first option is to use a Shared Access Signature (SAS) token to delegate  
                // access to the resource. See the documentation links at the top for more information on SAS. The second approach is to set permissions  
                // to allow public access to blobs in this container. Comment the line below to not use this approach and to use SAS. Then you can view the image  
                // using: https://[InsertYourStorageAccountNameHere].blob.core.windows.net/webappstoragedotnet-imagecontainer/FileName 
                await blobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

                // Gets all Cloud Block Blobs in the blobContainerName and passes them to teh view
                List<Uri> allBlobs = new List<Uri>();
                foreach (IListBlobItem blob in blobContainer.ListBlobs())
                {
                    if (blob.GetType() == typeof(CloudBlockBlob))
                        allBlobs.Add(blob.Uri);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> UploadSingleFileAsync(HttpPostedFileBase file,string filename, string serverpath="")
        {
            //string tempDirectory=Environment.CurrentDirectory.ToString();
            //string tempDirectory = @"C:/";
            try
            {
                var path = Path.Combine(serverpath, filename);
                file.SaveAs(path);

                CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(filename);
                using (var fileStream = System.IO.File.OpenRead(path))
                {
                    //blockBlob.UploadFromStream(fileStream);
                    await blockBlob.UploadFromStreamAsync(fileStream);
                }

               // CloudBlockBlob blob = blobContainer.GetBlockBlobReference(file.FileName);
               /// await blob.UploadFromFileAsync(file.FileName);

                //int fileCount = files.Count;

                //if (fileCount > 0)
                //{
                //    for (int i = 0; i < fileCount; i++)
                //    {
                //        CloudBlockBlob blob = blobContainer.GetBlockBlobReference(GetRandomBlobName(files[i].FileName));
                //        await blob.UploadFromFileAsync(files[i].FileName);
                //    }
                //}
               // File.Delete(path);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> DeleteImage(string name)
        {
            try
            {
                Uri uri = new Uri(name);
                string filename = Path.GetFileName(uri.LocalPath);

                var blob = blobContainer.GetBlockBlobReference(filename);
                await blob.DeleteIfExistsAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
