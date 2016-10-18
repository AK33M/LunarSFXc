using System;
using System.Collections.Generic;

namespace LunarSFXc.Objects
{
    public class ImageResult
    {
        //public ICollection<string> FileNames { get; set; }
        //public string Description { get; set; }
        //public DateTime CreatedTimestamp { get; set; }
        //public DateTime UpdatedTimestamp { get; set; }
        //public ICollection<string> ContentTypes { get; set; }

        public ICollection<ImageDescription> Files { get; set; }
    }
}
