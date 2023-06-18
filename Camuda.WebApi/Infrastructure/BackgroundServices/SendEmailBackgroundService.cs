using System.Text.Json;
using Camunda.WebApi.Consts;
using Camunda.WebApi.Dtos;
using Camunda.WebApi.Infrastructure.Services.Email;
using Camunda.WebApi.Infrastructure.Services.ZeebeEngine;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;

namespace Camunda.WebApi.Infrastructure.BackgroundServices;

public class SendEmailBackgroundService : BackgroundService
{
    private readonly ILogger<SendEmailBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IZeebeClientService _zeebeClientService;

    public SendEmailBackgroundService(
        ILogger<SendEmailBackgroundService> logger,
        IServiceProvider serviceProvider,
        IZeebeClientService zeebeClientService)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _zeebeClientService = zeebeClientService;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _zeebeClientService.CreateWorker(SendEmailServiceTaskNames.SendEmail, HandleJob);

        return Task.CompletedTask;
    }

    private async Task HandleJob(IJobClient client, IJob job)
    {
        using var scope = _serviceProvider.CreateScope();

        _logger.LogInformation($"Received send email job for instance: {job.ElementInstanceKey}");

        var headers = JsonSerializer
            .Deserialize<SendEmailDto>(job.Variables);

        SendEmail(scope, headers!);

        var messageGuid = Guid.NewGuid().ToString();

        await SendMessage(scope, messageGuid);

        await client.NewCompleteJobCommand(job.Key)
            .Variables(JsonSerializer.Serialize(new { MessageGuid = messageGuid }))
            .Send();

        _logger.LogInformation($"Send email completed job for instance: {job.ElementInstanceKey}");
    }

    private async Task SendMessage(IServiceScope scope, string messageGuid)
    {
        var zeebeClientService = scope.ServiceProvider.GetService<IZeebeClientService>();

        await zeebeClientService!.SendMessage(
            MessagesNames.EmailSendMessage,
            "MessageGuid",
            new { MessageGuid = messageGuid },
            CancellationToken.None);
    }

    private void SendEmail(IServiceScope scope, SendEmailDto sendEmailDto)
    {
        var emailService = scope.ServiceProvider.GetService<EmailService>();

        emailService!.SendEmail(sendEmailDto!.EmailTitle, sendEmailDto.EmailBody);
    }
}