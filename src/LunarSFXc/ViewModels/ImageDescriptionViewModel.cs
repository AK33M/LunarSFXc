using System;

namespace LunarSFXc.ViewModels
{
    public class ImageDescriptionViewModel
    {
        public string FileName { get; set; }
        public string Description { get; set; }
        public string ContainerName { get; set; }
        public string ContentType { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime UpdatedTimestamp { get; set; }
        public string ImageUri { get; set; }

    }
}
