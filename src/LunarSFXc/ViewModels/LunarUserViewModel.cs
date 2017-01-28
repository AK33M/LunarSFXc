using LunarSFXc.Objects;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace LunarSFXc.ViewModels
{
    public class LunarUserViewModel
    {
        public string UserName { get; set; }

        public string FirstWords { get; set; }

        public ICollection<string> Roles { get; set; }

        public ImageDescriptionViewModel ProfileImage { get; set; }

        public ICollection<CommentViewModel> Comments { get; set; }
        
        public int CommentCount
        {
            get { return Comments.Count; }
        }

        public string RolesString
        {
            get { return GetRolesString(); }
        }

        private string GetRolesString()
        {
            var result = string.Empty;
            if (Roles != null)
            {

                foreach (var r in Roles)
                {
                    result = string.Join(", ", r, result);
                }
            }


            return result.EndsWith(", ") ? result.Remove(result.LastIndexOf(',')) : result;
        }

    }
}
