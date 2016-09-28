using LunarSFXc.Objects;
using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LunarSFXc.Controllers
{
    public class AuthController : Controller
    {
        private SignInManager<LunarUser> _signInManager;
        private UserManager<LunarUser> _userManager;

        public AuthController(SignInManager<LunarUser> signInManager, UserManager<LunarUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (ModelState.IsValid)
            {
                var signInResult = new Microsoft.AspNetCore.Identity.SignInResult();

                if (model.Username.Contains("@"))
                {
                    var user = await _userManager.FindByEmailAsync(model.Username);

                    if(user != null)
                    {
                        signInResult = await _signInManager.PasswordSignInAsync(
                                                        user.UserName,
                                                        model.Password,
                                                        model.RememberMe,
                                                        false);
                    }                    
                }
                else
                {
                    signInResult = await _signInManager.PasswordSignInAsync(
                                                        model.Username,
                                                        model.Password,
                                                        model.RememberMe,
                                                        false);
                }

                
                if (signInResult.Succeeded)
                {
                    if (string.IsNullOrWhiteSpace(returnUrl))
                    {
                        return RedirectToAction("Posts", "Blog");
                    }
                    else
                    {
                        return Redirect(returnUrl);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Username/Email or Password incorrect");
                }
            }

            return View("RedirectedLogin");
        }

        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
            }
            return RedirectToAction("Posts", "Blog");
        }
    }
}
