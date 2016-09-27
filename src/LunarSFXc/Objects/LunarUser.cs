using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LunarSFXc.Objects
{
    public class LunarUser : IdentityUser
    {
        public string FirstWords { get; set; }
    }
}
