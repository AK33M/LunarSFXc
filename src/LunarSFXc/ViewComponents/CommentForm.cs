using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LunarSFXc.ViewComponents
{
    public class CommentForm : ViewComponent
    {
        public IViewComponentResult Invoke(string user, PostViewModel parentPost, int commentId)
        {
            if(parentPost != null)
            {
                var model = new CommentViewModel(user,
                                               parentPost.UrlSlug,
                                               parentPost.PostedOn.Year,
                                               parentPost.PostedOn.Month);

                return View("CommentForm", model);

            }
            else
            {
                var model = new CommentViewModel(user, commentId);

                return View("CommentReplyForm", model);
            }            
        }
    }
}
