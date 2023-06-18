using System.Text.Json;
using Camunda.WebApi.Consts;
using Camunda.WebApi.Dtos;
using Camunda.WebApi.Infrastructure.Services.ZeebeEngine;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;

namespace Camunda.WebApi.Infrastructure.BackgroundServices;

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
        _zeebeClientService.CreateWorker(TestProcessServiceTasksNames.MakeGreeting, HandleJob);

        return Task.CompletedTask;
    }

    private async Task HandleJob(IJobClient client, IJob job)
    {
        _logger.LogInformation($"Make Greeting Received job, for instance: {job.ElementInstanceKey}");

        var headers = JsonSerializer
            .Deserialize<MakeGreetingCustomHeadersDto>(job.CustomHeaders);

        var variables = JsonSerializer
            .Deserialize<MakeGreetingVariablesDto>(job.Variables);

        var greeting = headers!.Greeting;
        var name = variables!.Name;

        await client.NewCompleteJobCommand(job.Key)
            .Variables(JsonSerializer.Serialize(new MakeGreetingResultDto(greeting, name)))
            .Send();

        _logger.LogInformation($"Greeting Worker completed job, for instance: {job.ElementInstanceKey}");
    }
}