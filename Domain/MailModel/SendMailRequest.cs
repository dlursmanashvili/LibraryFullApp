using System.Net.Mail;

namespace Domain.MailModel
{
    public class SendMailRequest
    {
        public string userMail { get; set; }
        public string subject { get; set; }
        public string? body { get; set; } = null;
        public AlternateView? AlternateView { get; set; }
    }
}
