using Camunda.WebApi.Consts;
using Camunda.WebApi.Dtos;
using Camunda.WebApi.Infrastructure.Services.Http;
using Camunda.WebApi.Infrastructure.Services.ZeebeEngine;
using Microsoft.AspNetCore.Mvc;
using Zeebe.Client.Api.Responses;

namespace Camunda.WebApi.Controllers;

[ApiController]
[Route("process")]
public class ProcessController : ControllerBase
{
    private readonly ICamoundaOperateService _camoundaOperateService;
    private readonly IZeebeClientService _zeebeClientService;

    public ProcessController(IZeebeClientService zeebeClientService,
        ICamoundaOperateService camoundaOperateService)
    {
        _zeebeClientService = zeebeClientService;
        _camoundaOperateService = camoundaOperateService;
    }

    [HttpGet("engine/topology")]
    public async Task<ITopology> GetTopology(CancellationToken cancellationToken)
    {
        return await _zeebeClientService.Status(cancellationToken);
    }

    [HttpGet("process-instance/details")]
    public async Task<object?> GetProcessInstanceDetails([FromQuery] string instanceKey,
        CancellationToken cancellationToken)
    {
        return await _camoundaOperateService.GetProcessInstanceDetails(instanceKey, cancellationToken);
    }

    [HttpGet("process-instance/statistic")]
    public async Task<object?> GetProcessInstGetProcessInstanceStatisticDetails([FromQuery] string instanceKey,
        CancellationToken cancellationToken)
    {
        return await _camoundaOperateService.GetProcessInstanceStatistic(instanceKey, cancellationToken);
    }

    [HttpPost("run-process/send-email")]
    public async Task<IProcessInstanceResult> RunProcessInstanceSendEmail(CancellationToken cancellationToken)
    {
        return await _zeebeClientService
            .RunProcessInstance(CamundaProcessesNames.SendEmail, cancellationToken);
    }

    [HttpPost("run-process/test-process")]
    public async Task<IProcessInstanceResult> RunProcessInstanceTestProcess([FromBody] string name,
        CancellationToken cancellationToken)
    {
        return await _zeebeClientService.RunProcessInstance(
            CamundaProcessesNames.TestProcess,
            cancellationToken,
            new MakeGreetingVariablesDto(name));
    }
}