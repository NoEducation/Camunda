using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;
using Camuda.WebApi.Options;

namespace Camuda.WebApi.Infrastructure.Services
{
    public class SmtpClientWrapper 
    {
        private readonly EmailOptions emailOptions;

        public SmtpClientWrapper(IOptions<EmailOptions> emailOptions)
        {
            this.emailOptions = emailOptions.Value;
        }

        public void Send(MailMessage mailMessage)
        {
            mailMessage.To.Add(emailOptions.To);

            var client = new SmtpClient
            {
                Host = emailOptions.Host,
                Port = emailOptions.Port,
                EnableSsl = emailOptions.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(emailOptions.From, emailOptions.Password),
            };

            client.Send(mailMessage);
        }
    }
}
