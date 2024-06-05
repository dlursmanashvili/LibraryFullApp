using Domain.MailModel;
using Shared;
using System.Net.Mail;
using System.Net;

namespace Domain.MailModel.IRepository
{
    public interface ISendNotification
    {
        Task<CommandExecutionResult> SendMail(SendMailRequest request);
        Task<CommandExecutionResult> SendMail(string mailFrom, string mailPass, string smtp, int port, string userMail, string subject, string htmlBody);
        Task<(string MailFrom, string MailPass, string Smtp, int Portl)> GetPArametersForMailSend();
        Task<CommandExecutionResult> SendMail(string userMailTo, string subject, string htmlBody);
    }
}


