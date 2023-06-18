namespace Camunda.WebApi.Infrastructure.Services.Http;

public interface ICamoundaOperateService
{
    Task<object?> GetProcessInstanceDetails(string instanceKey, CancellationToken cancellationToken);
    Task<object?> GetProcessInstanceStatistic(string instanceKey, CancellationToken cancellationToken);
}