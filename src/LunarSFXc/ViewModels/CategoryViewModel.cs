using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LunarSFXc.ViewModels
{
    public class CategoryViewModel
    {
        [Required(ErrorMessage = "Name: Field is required")]
        [StringLength(500, ErrorMessage = "Name: Length should not exceed 500 characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "UrlSlug: Field is required")]
        [StringLength(500, ErrorMessage = "UrlSlug: Length should not exceed 500 characters")]
        public string UrlSlug { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public ICollection<PostViewModel> Posts { get; set; }
    }
}
