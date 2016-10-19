using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Extensions.Options;

namespace LunarSFXc.Services
{
    public class ImageServiceOptions
    {
        public ImageServiceOptions()
        {
                
        }

        public string ServerUploadFolder { get; set; }
        public string StorageConnectionStringImagesUploadKey1 { get; set; }
    }
}
