using Camunda.WebApi.Consts;
using Camunda.WebApi.Dtos;
using Camunda.WebApi.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Zeebe.Client.Api.Responses;

namespace Camunda.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    private readonly IZeebeClientService _zeebeClientService;

    public HomeController(IZeebeClientService zeebeClientService)
    {
        _zeebeClientService = zeebeClientService;
    }

    [HttpGet("GetTopology")]
    public async Task<ITopology> GetTopology(CancellationToken cancellationToken)
    {
        return await _zeebeClientService.Status(cancellationToken);
    }

    [HttpPost("Run/SendEmail")]
    public async Task<IProcessInstanceResult> RunProcessInstanceSendEmail(CancellationToken cancellationToken)
    {
        return await _zeebeClientService
            .RunProcessInstance(CamundaProcessesNames.SendEmail, cancellationToken);
    }

    [HttpPost("Run/TestProcess")]
    public async Task<IProcessInstanceResult> RunProcessInstanceTestProcess([FromBody] string name,
        CancellationToken cancellationToken)
    {
        return await _zeebeClientService.RunProcessInstance(
            CamundaProcessesNames.TestProcess,
            cancellationToken,
            new MakeGreetingVariablesDto(name));
    }
}