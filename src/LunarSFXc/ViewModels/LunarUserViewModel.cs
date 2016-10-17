using System.Collections.Generic;

namespace LunarSFXc.ViewModels
{
    public class LunarUserViewModel
    {
        public string UserName { get; set; }

        public string FirstWords { get; set; }

        public ICollection<CommentViewModel> Comments { get; set; }
    }
}
