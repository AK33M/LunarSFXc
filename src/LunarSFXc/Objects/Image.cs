namespace LunarSFXc.Objects
{
    public class Image
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set;}
        public byte[] Content { get; set; }

        public ImagePostPosition Postion { get; set; }

    }

    public enum ImagePostPosition
    {
        InPost,
        HeaderBackground,
        PostPreview
    }
}

