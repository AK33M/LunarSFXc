﻿using LunarSFXc.Objects;
using LunarSFXc.Repositories;
using System.Collections.Generic;

namespace LunarSFXc.ViewModels
{
    public class WidgetViewModel
    {
        public WidgetViewModel(IBlogRepository repo)
        {
            Categories = repo.Categories();
            Tags = repo.Tags();
        }

        public ICollection<Category> Categories { get; set; }
        public ICollection<Tag> Tags { get; set; }
    }
}
