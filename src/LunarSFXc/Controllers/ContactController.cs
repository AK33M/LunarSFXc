using LunarSFXc.Services;
using LunarSFXc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LunarSFXc.Controllers
{
    public class ContactController :Controller
    {
        private IConfigurationRoot _config;
        private IEmailService _mailService;

        public ContactController(IEmailService mailService, IConfigurationRoot config)
        {
            _config = config;
            _mailService = mailService;
        }

        public IActionResult Contact()
        {
            return View("ContactConfirmation");
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactViewModel model)
        {
            var destination = _config["mailSettings:contactForm:destinationEmailAddress"];
            ViewBag.Success = false;
            var sourceMailAddress = new MailAddress(model.Email, model.Name);

            if (ModelState.IsValid)
            {
               await _mailService.SendEmailAsync(destination, sourceMailAddress,  $"Website from {model.Name}", model.ToString());
                
                ModelState.Clear();
                ViewBag.Success = true;
                ViewData["Message"] = "Success. Message Sent!";
            }
            else
            {
                ModelState.AddModelError("", "Something Happened!");
                ViewData["Message"] = "Oops!. Something Happened!";
                return View("ContactConfirmation", model);
            }

            return View("ContactConfirmation", model);
        }
    }
}
