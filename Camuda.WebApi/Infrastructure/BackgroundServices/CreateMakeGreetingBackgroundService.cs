using Camuda.WebApi.Consts;
using Camuda.WebApi.Dtos;
using Camuda.WebApi.Infrastructure.Services;
using System.Text.Json;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;

namespace Camuda.WebApi.Infrastructure.BackgroundServices
{
    public class CreateMakeGreetingBackgroundService : BackgroundService
    {
        private readonly ILogger<CreateMakeGreetingBackgroundService> _logger;
        private readonly IZeebeClientService _zeebeClientService;

        public CreateMakeGreetingBackgroundService(ILogger<CreateMakeGreetingBackgroundService> logger,
            IZeebeClientService zeebeClientService)
        {
            _logger = logger;
            _zeebeClientService = zeebeClientService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _zeebeClientService.CreateWorker(TestProcessServiceTasksNames.MakeGreeting,
              async (client, job) => await HandleJob(client, job));

            return Task.CompletedTask;
        }

        private async Task HandleJob(IJobClient client, IJob job)
        {
            _logger.LogInformation($"Make Greeting Received job, for instance: {job.ElementInstanceKey}");

            var headers = JsonSerializer
                .Deserialize<MakeGreetingCustomHeadersDto>(job.CustomHeaders);

            var variables = JsonSerializer
                .Deserialize<MakeGreetingVariablesDto>(job.Variables);

            string greeting = headers!.Greeting;
            string name = variables!.Name;

            await client.NewCompleteJobCommand(job.Key)
               .Variables(JsonSerializer.Serialize(new MakeGreetingResultDto(greeting, name)))
               .Send();

            _logger.LogInformation($"Greeting Worker completed job, for instance: {job.ElementInstanceKey}");
        }
    }
}
