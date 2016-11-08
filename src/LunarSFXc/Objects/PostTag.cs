using System;
using System.Collections.Generic;

namespace LunarSFXc.Objects
{
    public class PostTag
    {
        public Post Post { get; set; }
        public int PostId { get; set; }
        public Tag Tag { get; set; }
        public int TagId { get; set; }
    }

    class PostTagComparer : IEqualityComparer<PostTag>
    {
        public bool Equals(PostTag x, PostTag y)
        {
            if (Object.ReferenceEquals(x, null) ||
            Object.ReferenceEquals(y, null))
                return false;

            return x.PostId == y.PostId && x.TagId == y.TagId;
        }

        public int GetHashCode(PostTag obj)
        {
            if (Object.ReferenceEquals(obj, null)) return 0;

            int hashTagId = obj.TagId.GetHashCode();

            int hashPostId = obj.PostId.GetHashCode();

            return hashTagId ^ hashPostId;
        }


    }
}
