using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Extensions.Options;

namespace LunarSFXc.Services
{
    public class CloudStorageService : ICouldStorageService
    {
        private IOptions<ImageServiceOptions> _options;

        public CloudStorageService(IOptions<ImageServiceOptions> optionsAccessor)
        {
            _options = optionsAccessor;
        }

        public CloudBlobContainer GetStorageContainer(string containerName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_options.Value.StorageConnectionStringImagesUploadKey1);

            // Create a blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Get a reference to a container named “mycontainer.”
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            return container;
        }
    }
}
