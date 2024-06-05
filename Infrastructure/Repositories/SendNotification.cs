using Domain.MailModel;
using Domain.MailModel.IRepository;
using Microsoft.Extensions.Configuration;
using Shared;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.Repositories
{
    public class SendNotification : ISendNotification
    {
        private readonly IConfiguration _config;
        public SendNotification(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<CommandExecutionResult> SendMail(SendMailRequest request)
        {

            string mailFrom = _config.GetSection("SendVerifyMailLinks:MailFrom").Value.ToString();
            string mailPass = _config.GetSection("SendVerifyMailLinks:MailPass").Value.ToString();
            string smtp = _config.GetSection("SendVerifyMailLinks:Smtp").Value.ToString();
            int port = Convert.ToInt32(_config.GetSection("SendVerifyMailLinks:SmtpPort").Value);

            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(mailFrom);
                    mail.To.Add(request.userMail);
                    mail.Subject = request.subject;
                    mail.IsBodyHtml = true;
                    if (request.body != null)
                    {
                        mail.Body = request.body;
                    }
                    else if (request.AlternateView != null)
                    {
                        mail.AlternateViews.Add(request.AlternateView);
                    }
                    else
                    {
                        return new CommandExecutionResult() { Success = false, ErrorMessage = "Mail Body Can't be blank" };
                    }

                    using (SmtpClient smtpClient = new SmtpClient(smtp, port))
                    {
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential(mailFrom, mailPass);
                        smtpClient.EnableSsl = false;

                        smtpClient.Send(mail);
                    }
                }

                return new CommandExecutionResult() { Success = true, ErrorMessage = null };

            }
            catch (Exception ex)
            {
                return new CommandExecutionResult()
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    Code = ex.HResult
                };
            }
        }

        public async Task<CommandExecutionResult> SendMail(string userMailTo, string subject, string htmlBody)
        {
            var mailConfig = await GetPArametersForMailSend();
            return await SendMail(mailConfig.MailFrom,
                                       mailConfig.MailPass,
                                       mailConfig.Smtp,
                                       mailConfig.Portl,
                                       userMailTo,
                                       subject,
                                       htmlBody);
        }

        public async Task<CommandExecutionResult> SendMail(string mailFrom, string mailPass, string smtp, int port, string userMailTo, string subject, string htmlBody)
        {
            try
            {
                using (MailMessage mail = new())
                {
                    mail.From = new MailAddress(mailFrom);
                    mail.To.Add(userMailTo);
                    mail.Subject = subject;
                    mail.IsBodyHtml = true;

                    if (htmlBody != null)
                    {
                        try
                        {
                            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, "text/html");
                            mail.AlternateViews.Add(alternateView);
                        }
                        catch (Exception)
                        {

                            return new CommandExecutionResult() { Success = false, ErrorMessage = "Mail Body not Valid" };
                        }

                    }
                    else
                    {
                        return new CommandExecutionResult() { Success = false, ErrorMessage = "Mail Body Can't be blank" };
                    }

                    using (SmtpClient smtpClient = new(smtp, port))
                    {
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential(mailFrom, mailPass);
                        smtpClient.EnableSsl = false;
                        smtpClient.Send(mail);
                    }
                }

                return new CommandExecutionResult() { Success = true, ErrorMessage = null };

            }
            catch (Exception ex)
            {
                return new CommandExecutionResult()
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    Code = ex.HResult
                };
            }
        }

        public async Task<(string MailFrom, string MailPass, string Smtp, int Portl)> GetPArametersForMailSend()
        {
            string mailFrom = _config.GetSection("SendVerifyMailLinks:MailFrom").Value.ToString();
            string mailPass = _config.GetSection("SendVerifyMailLinks:MailPass").Value.ToString();
            string smtp = _config.GetSection("SendVerifyMailLinks:Smtp").Value.ToString();
            int port = Convert.ToInt32(_config.GetSection("SendVerifyMailLinks:SmtpPort").Value);
            return (mailFrom, mailPass, smtp, port);
        }
    }
}
