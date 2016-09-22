using LunarSFXc.Objects;
using System.Collections.Generic;

namespace LunarSFXc.Repositories
{
    public interface IBlogRepository
    {
        IList<Post> Posts(int pageNo, int pageSize);
        int TotalPosts();
    }
}
