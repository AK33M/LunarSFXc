using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunarSFXc.Objects
{
    public class PostTag
    {
        public Post Post { get; set; }
        public int PostId { get; set; }
        public Tag Tag { get; set; }
        public int TagId { get; set; }
    }
}
