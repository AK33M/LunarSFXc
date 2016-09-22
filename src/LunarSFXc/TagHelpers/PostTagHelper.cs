using LunarSFXc.Objects;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunarSFXc.TagHelpers
{
    public class PostTagHelper : TagHelper
    {
        public Post ForPost { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";

            var post = ForPost;
            var text = post.Title;
            var link = $"Archive/{post.PostedOn.Year}/{post.PostedOn.Month}/{post.Title}";

            output.Attributes.SetAttribute("href", link);
            output.Content.SetContent(text);
        }
    }
}
