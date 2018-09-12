using System;
using System.IO;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace FaceGraphTask.Infrastructure.Service
{
    public class StorageService
    {
        private readonly CloudBlobContainer _blobContainer;

        public StorageService(string containerName)
        {
            // Check if Container Name is null or empty  
            if (string.IsNullOrEmpty(containerName))
            {
                throw new ArgumentNullException("containerName", "Container Name can't be empty");
            }
            try
            {
                // Get azure table storage connection string.  
                var connectionString = "Your Azure Storage Connection String goes here";
                var storageAccount = CloudStorageAccount.Parse(connectionString);

                var cloudBlobClient = storageAccount.CreateCloudBlobClient();
                _blobContainer = cloudBlobClient.GetContainerReference(containerName);

                // Create the container and set the permission  
                if (_blobContainer.CreateIfNotExists())
                {
                    _blobContainer.SetPermissions(
                        new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Blob
                        }
                        );
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string UploadFile(HttpPostedFileBase fileToUpload)
        {
            // Check HttpPostedFileBase is null or not  
            if (fileToUpload == null || fileToUpload.ContentLength == 0)
                return null;
            var fileName = Path.GetFileName(fileToUpload.FileName);
            // Create a block blob  
            var blockBlob = _blobContainer.GetBlockBlobReference(fileName);
            // Set the object's content type  
            blockBlob.Properties.ContentType = fileToUpload.ContentType;
            // upload to blob  
            blockBlob.UploadFromStream(fileToUpload.InputStream);

            // get file uri  
            var absoluteUri = blockBlob.Uri.AbsoluteUri;
            return absoluteUri;
        }

        public bool DeleteBlob(string absoluteUri)
        {
            try
            {
                var uriObj = new Uri(absoluteUri);
                var blobName = Path.GetFileName(uriObj.LocalPath);

                // get block blob refarence  
                var blockBlob = _blobContainer.GetBlockBlobReference(blobName);

                // delete blob from container      
                blockBlob.Delete();
                return true;
            }
            catch (Exception exceptionObj)
            {
                throw exceptionObj;
            }
        }
    }
}
