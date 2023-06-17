using Camuda.WebApi.Consts;
using Camuda.WebApi.Dtos;
using Camuda.WebApi.Infrastructure.Services;
using System.Text.Json;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;

namespace Camuda.WebApi.Infrastructure.BackgroundServices
{
    public class SendEmailBackgroundService : BackgroundService
    {
        private readonly IZeebeClientService _zeebeClientService;
        private readonly ILogger<SendEmailBackgroundService> _logger;
        private readonly EmailService _emailService;

        public SendEmailBackgroundService(IZeebeClientService zeebeClientService,
            ILogger<SendEmailBackgroundService> logger,
            EmailService emailService)
        {
            _zeebeClientService = zeebeClientService;
            _logger = logger;
            _emailService = emailService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
           _zeebeClientService.CreateWorker(SendEmailServiceTaskNames.SendEmail,
        async (client, job) => await HandleJob(client, job));

            return Task.CompletedTask;
        }

        private async Task HandleJob(IJobClient client, IJob job)
        {
            _logger.LogInformation($"Received send email job for instance: {job.ElementInstanceKey}");

            var headers = JsonSerializer
              .Deserialize<SendEmailDto>(job.Variables);

            _emailService.SendEmail(headers!.EmailTitle, headers.EmailBody);

            var messageGuid = Guid.NewGuid().ToString();

            await _zeebeClientService.SendMessage(
                MessagesNames.EmailSendMessage,
                "MessageGuid",
                new { MessageGuid = messageGuid },
                CancellationToken.None);

            await client.NewCompleteJobCommand(job.Key)
                .Variables(JsonSerializer.Serialize(new { MessageGuid = messageGuid }))
                .Send();

            _logger.LogInformation($"Send email completed job for instance: {job.ElementInstanceKey}");
        }
    }
}
