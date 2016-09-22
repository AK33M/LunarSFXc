using LunarSFXc.Objects;
using LunarSFXc.Repositories;
using System.Collections.Generic;

namespace LunarSFXc.ViewModels
{
    public class ListViewModel
    {
        public ListViewModel(IBlogRepository _repo, int p)
        {
            Posts = _repo.Posts(p - 1, 10);
            TotalPosts = _repo.TotalPosts();
        }
        public IList<Post> Posts { get; set; }
        public int TotalPosts { get; set; }
    }
}
