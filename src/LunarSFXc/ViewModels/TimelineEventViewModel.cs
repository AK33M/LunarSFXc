using System;
using System.ComponentModel.DataAnnotations;

namespace LunarSFXc.ViewModels
{
    public class TimelineEventViewModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        [StringLength(maximumLength: 250, MinimumLength = 50)]
        public string Description { get; set; }
        [Required]
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        [Required(ErrorMessage ="Please add an Image to go along with this event.")]
        public ImageDescriptionViewModel Image { get; set; }
    }
}
