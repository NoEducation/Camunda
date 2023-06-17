using System.Net;
using System.Net.Mail;
using Camunda.WebApi.Options;
using Microsoft.Extensions.Options;

namespace Camunda.WebApi.Infrastructure.Services;

public class SmtpClientWrapper
{
    private readonly EmailOptions _emailOptions;

    public SmtpClientWrapper(IOptions<EmailOptions> emailOptions)
    {
        _emailOptions = emailOptions.Value;
    }

    public void Send(MailMessage mailMessage)
    {
        mailMessage.To.Add(_emailOptions.To);

        var client = new SmtpClient
        {
            Host = _emailOptions.Host,
            Port = _emailOptions.Port,
            EnableSsl = _emailOptions.EnableSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_emailOptions.From, _emailOptions.Password)
        };

        client.Send(mailMessage);
    }
}