using Camuda.WebApi.Infrastructure.Services;

namespace Camuda.WebApi.Infrastructure.HostedServices
{
    public class DeployResourcesHostedService : IHostedService
    {
        private readonly IZeebeClientService _zeebeClientService;
        private readonly CancellationToken _cancellationToken = new CancellationToken();

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
}
