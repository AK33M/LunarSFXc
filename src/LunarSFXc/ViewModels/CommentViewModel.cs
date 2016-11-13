using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LunarSFXc.ViewModels
{
    public class CommentViewModel
    {
        public CommentViewModel()
        {

        }

        public CommentViewModel(string user, string postTitle, int year, int month)
        {
            User = user;
            PostTitle = postTitle;
            Year = year;
            Month = month;
        }
        public CommentViewModel(string user, int parentCommentId)
        {
            User = user;
            ParentCommentId = parentCommentId;
        }

        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(maximumLength: 250)]
        public string Content { get; set; }

        public int Id { get; set; }

        [HiddenInput]
        public string User { get; set; }
        [HiddenInput]
        public string PostTitle { get; set; }
        [HiddenInput]
        public int Year { get; set; }
        [HiddenInput]
        public int Month { get; set; }
        [HiddenInput]
        public int ParentCommentId { get; set; }
        [HiddenInput]
        public int CommentId { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public ICollection<CommentViewModel> Replies { get; set; }
    }
}
