using System.ComponentModel.DataAnnotations;

namespace LunarSFXc.Objects
{
    public class Project
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 150)]
        public string  Title { get; set; }
        public string SubTitle { get; set; }
        [Required]
        [StringLength(maximumLength: 5000)]
        public string Description { get; set; }
        public Category Category { get; set; }
        public ImageDescription Image { get; set; }
    }
}
