using System;
using System.Collections.Generic;

namespace LunarSFXc.Objects
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public LunarUser Owner { get; set; }
        public Post ParentPost { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public ICollection<Comment> Replies { get; set; }
    }
}
