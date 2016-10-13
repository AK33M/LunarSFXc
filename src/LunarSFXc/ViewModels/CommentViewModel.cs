using System.ComponentModel.DataAnnotations;

namespace LunarSFXc.ViewModels
{
    public class CommentViewModel
    {
        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(maximumLength: 250)]
        public string Comment { get; set; }
    }
}
