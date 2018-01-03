using System.Diagnostics;

namespace TheWorld.Services
{
    public class DebugMailService : IEmailService
    {
        public void SendEmail(string to, string @from, string subject, string body)
        {
            Debug.WriteLine($"Sending Mail: To: {to} From: {from} Subject: {subject}");
        }
    }
}
