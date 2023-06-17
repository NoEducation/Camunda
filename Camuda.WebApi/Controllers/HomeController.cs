using Camuda.WebApi.Consts;
using Camuda.WebApi.Dtos;
using Camuda.WebApi.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Zeebe.Client.Api.Responses;

namespace Camuda.WebApi.Controllers
{
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
                .RunProcessInstance(ComundaProcessesNames.SendEmail, cancellationToken);
        }

        [HttpPost("Run/TestProcess")]
        public async Task<IProcessInstanceResult> RunProcessInstanceTestProcess([FromBody] string name, CancellationToken cancellationToken)
        {
            return await _zeebeClientService.RunProcessInstance(
                ComundaProcessesNames.TestProcess,
                cancellationToken,
                new MakeGreetingVariablesDto(name));
        }
    }
}
