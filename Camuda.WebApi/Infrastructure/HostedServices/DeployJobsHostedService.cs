using Camunda.WebApi.Infrastructure.Services;

namespace Camunda.WebApi.Infrastructure.HostedServices;

public class DeployResourcesHostedService : IHostedService
{
    private readonly CancellationToken _cancellationToken = new();
    private readonly IZeebeClientService _zeebeClientService;

    public DeployResourcesHostedService(IZeebeClientService zeebeClientService)
    {
        _zeebeClientService = zeebeClientService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return;

        await _zeebeClientService.DeployAll(_cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cancellationToken.ThrowIfCancellationRequested();
        return Task.CompletedTask;
    }
}