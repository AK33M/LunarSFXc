using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;

namespace LunarSFXc.Objects
{
    public class LunarUser : IdentityUser
    {
        public string FirstWords { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
