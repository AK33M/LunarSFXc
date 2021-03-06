﻿using System.ComponentModel.DataAnnotations;

namespace LunarSFXc.ViewModels
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 150)]
        public string Title { get; set; }
        public string SubTitle { get; set; }
        [Required]
        [StringLength(maximumLength: 5000)]
        public string Description { get; set; }
        public CategoryViewModel Category { get; set; }
        public ImageDescriptionViewModel Image { get; set; }
        [StringLength(maximumLength: 50)]
        public string Date { get; set; }
    }
}
