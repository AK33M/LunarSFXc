using Microsoft.WindowsAzure.Storage.Blob;

namespace LunarSFXc.Services
{
    public interface ICouldStorageService
    {
        CloudBlobContainer GetStorageContainer(string containerName);
    }
}