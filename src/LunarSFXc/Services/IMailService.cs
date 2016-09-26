namespace LunarSFXc.Services
{
    public interface IMailService
    {
        void SendMail(string recipient, string from, string subject, string body);
    }
}