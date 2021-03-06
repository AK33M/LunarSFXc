﻿using LunarSFXc.Objects;
using LunarSFXc.Services;
using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.Net.Http.Headers;
using LunarSFXc.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LunarSFXc.Controllers
{
    //[RequireHttps]
    public class AuthController : Controller
    {
        private SignInManager<LunarUser> _signInManager;
        private UserManager<LunarUser> _userManager;
        private IEmailService _emailService;

        private MailAddress _authMail;
        private ICloudStorageService _cloudService;

        public AuthController(SignInManager<LunarUser> signInManager, UserManager<LunarUser> userManager, IEmailService emailService, IConfigurationRoot config, ICloudStorageService cloudService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _cloudService = cloudService;
            _authMail = new MailAddress(config["mailSettings:authEmail:address"], config["mailSettings:authEmail:displayName"]);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, IFormFile profileImage, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new LunarUser { UserName = model.UserName, Email = model.Email, FirstWords = model.FirstWords };
                var image = new Image { File = profileImage };
                var fileName = ContentDispositionHeaderValue.Parse(image.File.ContentDisposition).FileName.Trim('"');
                var container = _cloudService.GetStorageContainer("profileimages");
                await image.File.SaveInAzureAsync(container, fileName);

                user.ProfileImage = new ImageDescription
                                            {
                                                ContainerName = container.Name,
                                                ContentType = image.File.ContentType,
                                                CreatedTimestamp = DateTime.Now,
                                                Description = image.Id,
                                                FileName = image.File.FileName
                                            };               

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //await _signInManager.SignInAsync(user, isPersistent: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    await _emailService.SendEmailAsync(model.Email, _authMail, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return View("RegisterConfirmation", model.Email);
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    return View("ForgotPasswordConfirmation");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Auth", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                await _emailService.SendEmailAsync(model.Email, _authMail, "Reset Password",
                    $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");

                return View("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction(nameof(AuthController.ResetPasswordConfirmation), "Auth");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(AuthController.ResetPasswordConfirmation), "Auth");
            }
            AddErrors(result);
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToLocal(returnUrl);

            }
            return View("RedirectedLogin");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var signInResult = new Microsoft.AspNetCore.Identity.SignInResult();

                if (model.Username.Contains("@"))
                {
                    var user = await _userManager.FindByEmailAsync(model.Username);

                    if (user != null)
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
                    return RedirectToLocal(returnUrl);

                }
                else
                {
                    ModelState.AddModelError("", "Username/Email or Password incorrect");
                }
            }

            return View("RedirectedLogin");
        }

        public async Task<IActionResult> Logout(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
            }

            return RedirectToLocal(returnUrl);
        }



        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(BlogController.Posts), "Blog");
        }
    }
}
