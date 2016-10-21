using LunarSFXc.Services;
using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LunarSFXc.ViewComponents
{
    public class PhotoGallery : ViewComponent
    {
        private ICouldStorageService _cloudService;

        public PhotoGallery(ICouldStorageService cloudService)
        {
            _cloudService = cloudService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var uris = await _cloudService.ListAllBlobs("imagesupload");

            var model = new GalleryViewModel
            {                
                 ImageUris = uris
            };

            return View("PhotoGallery", model);
        }
    }
}
