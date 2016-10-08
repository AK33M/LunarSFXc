using System.Net.Mail;
using System.Threading.Tasks;

namespace LunarSFXc.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string destEmail, MailAddress sourceEmail, string subject, string message);
    }
}