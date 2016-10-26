using System;
using System.ComponentModel.DataAnnotations;

namespace LunarSFXc.ViewModels
{
    public class TimelineEventViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        public string EndDate { get; set; }
        public ImageDescriptionViewModel Image { get; set; }
    }
}
