using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LunarSFXc.Extensions
{
    public static class FormFileExtensions
    {
        private static int DefaultBufferSize = 80 * 1024;
        /// <summary>
        /// Asynchronously saves the contents of an uploaded file.
        /// </summary>
        /// <param name="formFile">The <see cref="IFormFile"/>.</param>
        /// <param name="filename">The name of the file to create.</param>
        public async static Task SaveAsAsync(
            this IFormFile formFile,
            string filename,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (formFile == null)
            {
                throw new ArgumentNullException(nameof(formFile));
            }

            using (var fileStream = new FileStream(filename, FileMode.Create))
            {
                var inputStream = formFile.OpenReadStream();
                await inputStream.CopyToAsync(fileStream, DefaultBufferSize, cancellationToken);
            }
        }

        public async static Task SaveInAzureAsync(this IFormFile formFile, CloudBlobContainer container, string filename, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (formFile == null)
            {
                throw new ArgumentNullException(nameof(formFile));
            }

            // Get a reference to a blob named "filename".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);

            // Create or overwrite the "blockBlob" blob with the contents of a local file
            // named “filename”.
            using (var inputStream = formFile.OpenReadStream())
            {
                await blockBlob.UploadFromStreamAsync(inputStream);
            }
        }
    }
}
