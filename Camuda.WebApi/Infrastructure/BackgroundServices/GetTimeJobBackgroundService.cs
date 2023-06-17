using Camuda.WebApi.Consts;
using Camuda.WebApi.Dtos;
using Camuda.WebApi.Infrastructure.Services;
using System.Text.Json;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;

namespace Camuda.WebApi.Infrastructure.BackgroundServices
{
    public class GetTimeJobBackgroundService : BackgroundService
    {
        private readonly IZeebeClientService _zeebeClientService;
        private readonly ILogger<GetTimeJobBackgroundService> _logger;

        public GetTimeJobBackgroundService(IZeebeClientService zeebeClientService,
            ILogger<GetTimeJobBackgroundService> logger)
        {
            _zeebeClientService = zeebeClientService;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _zeebeClientService.CreateWorker(TestProcessServiceTasksNames.GetTime,
               async (client, job) => await HandleJob(client, job));

            return Task.CompletedTask;
        }

        private async Task HandleJob(IJobClient client, IJob job)
        {
            _logger.LogInformation($"Received get time job for instance: {job.ElementInstanceKey}");

            var currentTime = DateTimeOffset.UtcNow;

            await client.NewCompleteJobCommand(job.Key)
                .Variables(JsonSerializer.Serialize(new GetDateResultDto(currentTime)))
                .Send();

            _logger.LogInformation($"Get time job completed job, for instance: {job.ElementInstanceKey}");
        }
    }
}
