using LunarSFXc.Services;

namespace LunarSFXc.ViewModels
{
    public class ImageDescriptionViewModel
    {
        private ICloudStorageService _cloudService;

        //public ImageDescriptionViewModel(ICloudStorageService cloudService)
        //{
        //    _cloudService = cloudService;
        //}

        public string FileName { get; set; }
        public string Description { get; set; }
        public string ContainerName { get; set; }
        public string ContentType { get; set; }




        public string ImageUri
        {
            get
            {
                return "string";
                    //_cloudService.GetImageUri(ContainerName, FileName);
            }
        }

    }
}
