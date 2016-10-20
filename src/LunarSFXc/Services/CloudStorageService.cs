using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace LunarSFXc.Services
{
    public class CloudStorageService : ICouldStorageService
    {
        private CloudStorageAccount _storageAccount;
        private IOptions<ImageServiceOptions> _options;

        public CloudStorageService(IOptions<ImageServiceOptions> optionsAccessor)
        {
            _options = optionsAccessor;
            _storageAccount = CloudStorageAccount.Parse(_options.Value.StorageConnectionStringImagesUploadKey1);

        }

        public CloudBlobContainer GetStorageContainer(string containerName)
        {

            // Create a blob client.
            CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();

            // Get a reference to a container named “mycontainer.”
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            return container;
        }

        public void ListAllBlobs(string containerName)
        {
            // Create the blob client.
            CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            var foo = ListBlobsSegmentedInFlatListing(container);

            //// Loop over items within the container and output the length and URI.
            //foreach (IListBlobItem item in container.ListBlobsSegmentedAsync( )
            //{
            //    if (item.GetType() == typeof(CloudBlockBlob))
            //    {
            //        CloudBlockBlob blob = (CloudBlockBlob)item;

            //        Console.WriteLine("Block blob of length {0}: {1}", blob.Properties.Length, blob.Uri);

            //    }
            //    else if (item.GetType() == typeof(CloudPageBlob))
            //    {
            //        CloudPageBlob pageBlob = (CloudPageBlob)item;

            //        Console.WriteLine("Page blob of length {0}: {1}", pageBlob.Properties.Length, pageBlob.Uri);

            //    }
            //    else if (item.GetType() == typeof(CloudBlobDirectory))
            //    {
            //        CloudBlobDirectory directory = (CloudBlobDirectory)item;

            //        Console.WriteLine("Directory: {0}", directory.Uri);
            //    }
            //}
        }

        async public static Task ListBlobsSegmentedInFlatListing(CloudBlobContainer container)
        {
            //List blobs to the console window, with paging.
           // Console.WriteLine("List blobs in pages:");

            int i = 0;
            BlobContinuationToken continuationToken = null;
            BlobResultSegment resultSegment = null;

            //Call ListBlobsSegmentedAsync and enumerate the result segment returned, while the continuation token is non-null.
            //When the continuation token is null, the last page has been returned and execution can exit the loop.
            do
            {
                //This overload allows control of the page size. You can return all remaining results by passing null for the maxResults parameter,
                //or by calling a different overload.
                resultSegment = await container.ListBlobsSegmentedAsync("", true, BlobListingDetails.All, 10, continuationToken, null, null);
                if (resultSegment.Results.Count<IListBlobItem>() > 0)
                {
                    var bar = ++i;
                    //Console.WriteLine("Page {0}:", ++i);
                }
                foreach (var blobItem in resultSegment.Results)
                {
                    var foo = blobItem.StorageUri.PrimaryUri;
                    //Console.WriteLine("\t{0}", blobItem.StorageUri.PrimaryUri);
                }
                //Console.WriteLine();

                //Get the continuation token.
                continuationToken = resultSegment.ContinuationToken;
            }
            while (continuationToken != null);
        }
    }
}
