using System.Text.Json;
using Camunda.WebApi.Consts;
using Camunda.WebApi.Dtos;
using Camunda.WebApi.Infrastructure.Services.ZeebeEngine;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;

namespace Camunda.WebApi.Infrastructure.BackgroundServices;

public class GetTimeJobBackgroundService : BackgroundService
{
    private readonly ILogger<GetTimeJobBackgroundService> _logger;
    private readonly IZeebeClientService _zeebeClientService;

    public GetTimeJobBackgroundService(ILogger<GetTimeJobBackgroundService> logger,
        IZeebeClientService zeebeClientService)
    {
        _logger = logger;
        _zeebeClientService = zeebeClientService;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _zeebeClientService.CreateWorker(TestProcessServiceTasksNames.GetTime, HandleJob);

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