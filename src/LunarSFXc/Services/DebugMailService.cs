using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LunarSFXc.Services
{
    public class DebugMailService : IEmailService
    {
        public Task SendEmailAsync(string destEmail, MailAddress sourceEmail, string subject, string message)
        {
            throw new NotImplementedException();
        }

        //public void SendMail(string recipient, string from, string subject, string body)
        //{
        //    Debug.WriteLine($"Sending Mail: To: {recipient} From: {from} Subject: {subject}");
        //}
    }
}
