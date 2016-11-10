using System;
using System.Collections.Generic;

namespace LunarSFXc.Objects
{
    public class ImageDescription
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public string ContainerName { get; set; }
        public DateTime CreatedTimestamp { get; set; }
        public DateTime UpdatedTimestamp { get; set; }
        public string ContentType { get; set; }
    }

    class ImageDescriptionComparer : IEqualityComparer<ImageDescription>
    {
        public bool Equals(ImageDescription x, ImageDescription y)
        {
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                return false;

            return x.Id == y.Id && x.Description == y.Description;
        }

        public int GetHashCode(ImageDescription obj)
        {
            if (ReferenceEquals(obj, null)) return 0;

            int hashId = obj.Id.GetHashCode();

            int hashDescriptionId = obj.Description.GetHashCode();

            return hashId ^ hashDescriptionId;
        }
    }
}
