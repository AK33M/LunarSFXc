using LunarSFXc.Objects;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;

namespace LunarSFXc.TagHelpers
{
    public class PostTagHelper : TagHelper
    {
        public Post ForPost { get; set; }
        public string PostTitle { get; set; } = string.Empty;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var post = ForPost;
            var text = post.Title;
            if (!string.IsNullOrWhiteSpace(PostTitle))
            {
                text = PostTitle;
            }
            var link = $"Archive/{post.PostedOn.Year}/{post.PostedOn.Month}/{post.UrlSlug}";

            output.TagName = "a";

            output.Attributes.SetAttribute("href", link);
            output.Content.SetContent(text);
        }
    }

    public class CategoryTagHelper : TagHelper
    {
        public Category ForCategory { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var category = ForCategory;
            var link = $"Category/{category.UrlSlug}";

            output.TagName = "a";
            output.Attributes.SetAttribute("href", link);
            output.Attributes.SetAttribute("title", $"See all posts in {category.Name}");
            output.Content.SetContent(category.Name);
        }
    }

    public class TagsTagHelper : TagHelper
    {
        public ICollection<PostTag> ForTags { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var tags = ForTags;
            var html = string.Empty;
            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    html = html + $"<a href='Tag/{tag.Tag.Name}' title='See all posts with {tag.Tag.Name} tag'>{tag.Tag.Name}</a> ";
                }
            }
            
            output.TagName = "div";
            output.Attributes.Add("class", "postTags");
            output.Content.SetHtmlContent(html);
        }
    }
}
