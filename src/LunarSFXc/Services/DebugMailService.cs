using System.Diagnostics;

namespace LunarSFXc.Services
{
    public class DebugMailService : IMailService
    {
        public void SendMail(string recipient, string from, string subject, string body)
        {
            Debug.WriteLine($"Sending Mail: To: {recipient} From: {from} Subject: {subject}");
        }
    }
}
