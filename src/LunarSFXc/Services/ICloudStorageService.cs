using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LunarSFXc.Services
{
    public interface ICloudStorageService
    {
        CloudBlobContainer GetStorageContainer(string containerName);
        Task<ICollection<Uri>> ListAllBlobs(string containerName);
        string GetImageUri(string containerName, string fileName);
    }
}