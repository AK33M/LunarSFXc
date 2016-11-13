using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LunarSFXc.ViewComponents
{
    public class CommentForm : ViewComponent
    {
        public IViewComponentResult Invoke(string user, PostViewModel parentPost, int parentCommentId, int commentId, bool isEdit)
        {
            if (parentPost != null)
            {
                var model = new CommentViewModel(user,
                                               parentPost.UrlSlug,
                                               parentPost.PostedOn.Year,
                                               parentPost.PostedOn.Month);

                return View("CommentForm", model);

            }
            else if (isEdit)
            {
                var model = new CommentViewModel(user, commentId);

                return View("CommentEditForm", model);
            }
            else
            {
                var model = new CommentViewModel(user, parentCommentId);

                return View("CommentReplyForm", model);
            }
        }
    }
}
