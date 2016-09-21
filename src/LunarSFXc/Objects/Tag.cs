using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LunarSFXc.Objects
{
    public class Tag
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name: Field is required")]
        [StringLength(500, ErrorMessage = "Name: Length should not exceed 500 characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "UrlSlug: Field is required")]
        [StringLength(500, ErrorMessage = "UrlSlug: Length should not exceed 500 characters")]
        public string UrlSlug { get; set; }
        public string Description { get; set; }
        //[JsonIgnore]
        //public ICollection<Post> Posts { get; set; }
        public List<PostTag> PostTags { get; set; }
    }
}