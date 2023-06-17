using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;

namespace Camuda.WebApi.Infrastructure.Services
{
    public interface IZeebeClientService
    {
        IJobWorker CreateWorker(string jobType, JobHandler handleJob, params string[]? fetchVariables);
        Task DeployAll(CancellationToken cancellationToken);
        Task<IProcessInstanceResult> RunProcessInstance(string bpmProcessId,
            CancellationToken cancellationToken, object? payload = null);
        Task<IProcessInstanceResponse> CreateProcessInstance(string bpmProcessId,
            CancellationToken cancellationToken, object? payload = null);
        Task SendMessage(string messageName, string correlationKey, object payload, CancellationToken cancellationToken);
        Task<ITopology> Status(CancellationToken cancellationToken);
    }
}