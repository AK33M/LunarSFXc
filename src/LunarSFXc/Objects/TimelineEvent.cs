using System;
using System.ComponentModel.DataAnnotations;

namespace LunarSFXc.Objects
{
    public class TimelineEvent
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        public string EndDate { get; set; }
        public ImageDescription Image { get; set; }
    }
}
