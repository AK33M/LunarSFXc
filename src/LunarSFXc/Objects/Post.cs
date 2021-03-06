﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LunarSFXc.Objects
{
    public class Post
    {
        [Required(ErrorMessage = "Id: Field is required")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Title: Field is required")]
        [StringLength(500, ErrorMessage = "Title: Length should not exceed 500 characters")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Short Description: Field is required")]
        public string ShortDescription { get; set; }
        [Required(ErrorMessage = "Description: Field is required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Meta: Field is required")]
        [StringLength(1000, ErrorMessage = "Meta: Length should not exceed 1000 characters")]
        public string Meta { get; set; }
        [Required(ErrorMessage = "UrlSlug: Field is required")]
        [StringLength(50, ErrorMessage = "UrlSlug: Length should not exceed 50 characters")]
        public string UrlSlug { get; set; }
        public bool Published { get; set; }
        [Required(ErrorMessage = "PostedOn: Field is required")]
        public DateTime PostedOn { get; set; }
        public DateTime? Modified { get; set; }
        public Category Category { get; set; }
       // public ICollection<Tag> Tags { get; set; }
        public int CategoryId { get; set; }

        public LunarUser PostedBy { get; set; }

        public ICollection<PostTag> PostTags { get; set; }
        public ICollection<ImageDescription> Images { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
