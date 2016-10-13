using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LunarSFXc.Objects
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public LunarUser Owner { get; set; }
        public Post ParentPost { get; set; }

        public ICollection<Comment> Replies { get; set; }
    }
}
