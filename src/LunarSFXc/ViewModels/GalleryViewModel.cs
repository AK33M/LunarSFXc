using AutoMapper;
using LunarSFXc.Repositories;
using LunarSFXc.Services;
using System.Collections.Generic;

namespace LunarSFXc.ViewModels
{
    public class GalleryViewModel
    {
        public GalleryViewModel(ICloudStorageService cloudService, IBlogRepository repo, string containerName)
        {
            Images = Mapper.Map<ICollection<ImageDescriptionViewModel>>(repo.GetAllImages(containerName).Result);

            foreach (var img in Images)
            {
                img.ImageUri = cloudService.GetImageUri(img.ContainerName, img.FileName);
            }
        }

        public ICollection<ImageDescriptionViewModel> Images { get; set; }
    }
}
