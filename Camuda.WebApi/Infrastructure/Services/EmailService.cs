using Camuda.WebApi.Options;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace Camuda.WebApi.Infrastructure.Services
{
    public class EmailService
    {
        private readonly SmtpClientWrapper _smtpClient;
        private readonly EmailOptions _emailOptions;

        public EmailService(SmtpClientWrapper smtpClient,
            IOptions<EmailOptions> emailOptions)
        {
            _smtpClient = smtpClient;
            _emailOptions = emailOptions.Value;
        }

        public void SendEmail(string subject, string body)
        {
            using var message = new MailMessage();

            message.IsBodyHtml = true;
            message.From = new MailAddress(_emailOptions.From, _emailOptions.FromDisplayName);
            message.Subject = subject;
            message.Body = body;

            _smtpClient.Send(message);
        }
    }
}
