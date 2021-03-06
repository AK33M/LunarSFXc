﻿using LunarSFXc.Objects;
using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;

namespace LunarSFXc.TagHelpers
{
    public class PostTagHelper : TagHelper
    {
        public PostViewModel ForPost { get; set; }
        public string PostTitle { get; set; } = string.Empty;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var post = ForPost;
            var text = post.Title;
            if (!string.IsNullOrWhiteSpace(PostTitle))
            {
                text = PostTitle;
            }
            var link = $"/Archive/{post.PostedOn.Year}/{post.PostedOn.Month}/{post.UrlSlug}";

            output.TagName = "a";

            output.Attributes.SetAttribute("href", link);
            output.Content.SetContent(text);
        }
    }

    public class CategoryTagHelper : TagHelper
    {
        public CategoryViewModel ForCategory { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var category = ForCategory;
            var link = $"/Category/{category.UrlSlug}";

            output.TagName = "span";
            //output.Attributes.SetAttribute("href", link);

            //output.Attributes.SetAttribute("title", $"See all posts in {category.Name}");
            //output.Content.SetContent(category.Name);

            var html = $"<a href='{link}' title='See all posts in {category.Name}'><i class='fa fa-paperclip'></i>{category.Name}</a>";
            output.Content.SetHtmlContent(html);
        }
    }

    //[HtmlTargetElement("tags", Attributes = "post-tags")]
    //public class PostTagsTagHelper : TagHelper
    //{
    //    public ICollection<PostTag> ForTags { get; set; }

    //    public override void Process(TagHelperContext context, TagHelperOutput output)
    //    {
    //        var tags = ForTags;
    //        var html = string.Empty;
    //        if (tags != null)
    //        {
    //            foreach (var tag in tags)
    //            {
    //                html = html + $"<li><a href='/Tag/{tag.Tag.UrlSlug}' title='See all posts with {tag.Tag.Name} tag'>#{tag.Tag.Name}</a></li>";
    //            }
    //        }

    //        output.TagName = "ul";
    //        output.Attributes.Add("class", "postTags list-inline pull-right");
    //        output.Content.SetHtmlContent(html);
    //    }
    //}

    [HtmlTargetElement("tags", Attributes = "tags")]
    public class TagsTagHelper : TagHelper
    {
        public ICollection<TagViewModel> ForTags { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var tags = ForTags;
            var html = string.Empty;
            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    html = html + $"<li><a href='/Tag/{tag.UrlSlug}' title='See all posts with {tag.Name} tag'>#{tag.Name}</a></li>";
                }
            }

            output.TagName = "ul";
            output.Attributes.Add("class", "postTags list-inline");
            output.Content.SetHtmlContent(html);
        }
    }
}
