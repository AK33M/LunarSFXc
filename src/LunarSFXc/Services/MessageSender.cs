using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace LunarSFXc.Services
{
    public class MessageSender : IEmailService, ISmsService
    {
        private IConfigurationRoot _config;
        private ILogger<MessageSender> _logger;
        private EmailSenderOptions _options;

        public MessageSender(IOptions<EmailSenderOptions> optionsAccessor, ILogger<MessageSender> logger, IConfigurationRoot config)
        {
            _logger = logger;
            _options = optionsAccessor.Value;
            _config = config;
        }
        public Task SendEmailAsync(string destEmail, MailAddress sourceEmail, string subject, string message)
        {
            try
            {
                var myMessage = new SendGrid.SendGridMessage();
                myMessage.AddTo(destEmail);
                myMessage.From = sourceEmail;
                myMessage.Subject = subject;
                myMessage.Text = message;
                myMessage.Html = message;
                myMessage.AddBcc(_config["mailSettings:contactForm:destinationEmailAddress"]);
                var credentials = new NetworkCredential(
                    _options.SendGridUser,
                    _options.SendGridPassword);

                var transportWeb = new SendGrid.Web(credentials);

                return transportWeb.DeliverAsync(myMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Message Sender: SendEmailAsync", ex);
                throw ex;
            }
        }
    }
}
