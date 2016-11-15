using System.Collections.Generic;
using LunarSFXc.Repositories;
using LunarSFXc.Services;
using AutoMapper;

namespace LunarSFXc.ViewModels
{
    public class PortfolioViewModel
    {
        public PortfolioViewModel(ICloudStorageService cloudService, IBlogRepository repo)
        {
            Projects = Mapper.Map<ICollection<ProjectViewModel>>(repo.GetAllProjects());

            foreach (var proj in Projects)
            {
                proj.Image.ImageUri = cloudService.GetImageUri(proj.Image?.ContainerName, proj.Image?.FileName);
            }
        }

        public PortfolioViewModel(ICloudStorageService cloudService, IBlogRepository repo, int id)
        {
            Projects = Mapper.Map<ICollection<ProjectViewModel>>(repo.GetProject(id));

            foreach (var proj in Projects)
            {
                proj.Image.ImageUri = cloudService.GetImageUri(proj.Image?.ContainerName, proj.Image?.FileName);
            }
        }

        public ICollection<ProjectViewModel> Projects { get; set; }
    }
}
