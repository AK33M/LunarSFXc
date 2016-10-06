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

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new LunarUser { UserName = model.UserName, Email = model.Email, FirstWords = model.FirstWords };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Posts", "Blog");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
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

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}
