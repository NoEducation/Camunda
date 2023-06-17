using Camuda.WebApi.Consts;
using Camuda.WebApi.Infrastructure.Services;
using Newtonsoft.Json;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;

namespace Camuda.WebApi.Infrastructure.BackgroundServices
{
    public class EmailWasSendEventBackgroundService : BackgroundService
    {
        private readonly IZeebeClientService _zeebeClientService;
        private readonly ILogger<EmailWasSendEventBackgroundService> _logger;

        public EmailWasSendEventBackgroundService(IZeebeClientService zeebeClientService,
            ILogger<EmailWasSendEventBackgroundService> logger)
        {
            _zeebeClientService = zeebeClientService;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _zeebeClientService.CreateWorker(EmailIsSendTaskNames.EmailWasSend,
             async (client, job) => await HandleJob(client, job));

            return Task.CompletedTask;
        }

        private async Task HandleJob(IJobClient client, IJob job)
        {
            _logger.LogInformation($"Received email was send job for instance: {job.ElementInstanceKey}");

            await client.NewCompleteJobCommand(job.Key)
                .Send();

            _logger.LogInformation($"Send email was send completed job, for instance: {job.ElementInstanceKey}");
        }
    }
}
