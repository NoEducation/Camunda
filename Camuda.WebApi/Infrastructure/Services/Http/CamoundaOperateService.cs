using Camunda.WebApi.Options;
using Microsoft.Extensions.Options;

namespace Camunda.WebApi.Infrastructure.Services.Http;

/// <summary>
///     Details https://docs.camunda.io/docs/8.0/apis-tools/operate-api/
/// </summary>
public class CamoundaOperateService : ICamoundaOperateService
{
    private readonly CamundaEnvironmentOptions _camundaEnvironmentOptions;
    private readonly ICamundaHttpService _camundaHttpService;

    public CamoundaOperateService(
        IOptions<CamundaEnvironmentOptions> camundaEnvironmentOptions,
        ICamundaHttpService camundaHttpService)
    {
        _camundaHttpService = camundaHttpService;
        _camundaEnvironmentOptions = camundaEnvironmentOptions.Value;
    }

    public async Task<object?> GetProcessInstanceDetails(string instanceKey, CancellationToken cancellationToken)
    {
        Uri uri = new($"{_camundaEnvironmentOptions.CAMUNDA_OPERATE_BASE_URL}/v1/process-instances/{instanceKey}");

        var processInstanceDetails = await _camundaHttpService.Get<object>(uri, cancellationToken);

        return processInstanceDetails;
    }

    public async Task<object?> GetProcessInstanceStatistic(string instanceKey, CancellationToken cancellationToken)
    {
        Uri uri = new(
            $"{_camundaEnvironmentOptions.CAMUNDA_OPERATE_BASE_URL}/v1/process-instances/{instanceKey}/statistics");

        var processInstanceDetails = await _camundaHttpService.Get<object>(uri, cancellationToken);

        return processInstanceDetails;
    }
}