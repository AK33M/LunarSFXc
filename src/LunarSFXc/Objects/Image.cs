using Microsoft.AspNetCore.Http;
using System;

namespace LunarSFXc.Objects
{
    public class Image
    {
        public Image()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public IFormFile File { get; set; }
    }
}

