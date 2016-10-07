using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LunarSFXc.Services
{
    public class AuthEmailService : IEmailService, ISmsService
    {
        private AuthMessageSenderOptions _options;

        public AuthEmailService(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var myMessage = new SendGrid.SendGridMessage();
            myMessage.AddTo(email);
            myMessage.From = new MailAddress("this@akeemtaiwo.com", "Akeem");
            myMessage.Subject = subject;
            myMessage.Text = message;
            myMessage.Html = message;
            var credentials = new NetworkCredential(
                _options.SendGridUser,
                _options.SendGridPassword);

            var transportWeb = new SendGrid.Web(credentials);
            return transportWeb.DeliverAsync(myMessage);
        }
    }
}
