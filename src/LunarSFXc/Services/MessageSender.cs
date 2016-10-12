using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;

namespace LunarSFXc.Services
{
    public class MessageSender : IEmailService, ISmsService
    {
        private ILogger _logger;
        private EmailSenderOptions _options;

        public MessageSender(IOptions<EmailSenderOptions> optionsAccessor, ILogger logger)
        {
            _logger = logger;
            _options = optionsAccessor.Value;
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
                var credentials = new NetworkCredential(
                    _options.SendGridUser,
                    _options.SendGridPassword);

                var transportWeb = new SendGrid.Web(credentials);

                return transportWeb.DeliverAsync(myMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending email message", ex);
                return null;
            }           
        }
    }
}
