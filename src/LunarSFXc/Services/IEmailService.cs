using System.Threading.Tasks;

namespace LunarSFXc.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}