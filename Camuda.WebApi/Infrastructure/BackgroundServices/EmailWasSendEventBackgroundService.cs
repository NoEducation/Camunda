﻿using Camunda.WebApi.Consts;
using Camunda.WebApi.Infrastructure.Services;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;

namespace Camunda.WebApi.Infrastructure.BackgroundServices;

public class EmailWasSendEventBackgroundService : BackgroundService
{
    private readonly ILogger<EmailWasSendEventBackgroundService> _logger;
    private readonly IZeebeClientService _zeebeClientService;

    public EmailWasSendEventBackgroundService(IZeebeClientService zeebeClientService,
        ILogger<EmailWasSendEventBackgroundService> logger)
    {
        _zeebeClientService = zeebeClientService;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _zeebeClientService.CreateWorker(EmailIsSendTaskNames.EmailWasSend, HandleJob);

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