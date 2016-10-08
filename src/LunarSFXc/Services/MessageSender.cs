using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System;

namespace LunarSFXc.Services
{
    public class MessageSender : IEmailService, ISmsService
    {
        private EmailSenderOptions _options;

        public MessageSender(IOptions<EmailSenderOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }
        public Task SendEmailAsync(string destEmail, MailAddress sourceEmail, string subject, string message)
        {
            var myMessage = new SendGrid.SendGridMessage();
            myMessage.AddTo(destEmail);
            myMessage.From = sourceEmail;
            myMessage.Subject = subject;
            myMessage.Text = message;
            myMessage.Html = message;
            var credentials = new NetworkCredential(
                _options.SendGridUser,
                _options.SendGridPassword);

            var transportWeb = new SendGrid.Web(credentials);

            var result = transportWeb.DeliverAsync(myMessage);
            return result;
        }
    }
}
