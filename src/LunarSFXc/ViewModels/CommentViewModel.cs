using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LunarSFXc.ViewModels
{
    public class CommentViewModel
    {
        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(maximumLength: 250)]
        public string Content { get; set; }

        public int Id { get; set; }

        public LunarUserViewModel Owner { get; set; }
        public PostViewModel ParentPost { get; set; }

        public ICollection<CommentViewModel> Replies { get; set; }
    }
}
