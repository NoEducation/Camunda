using Camuda.WebApi.Consts;
using Camuda.WebApi.Options;
using dotenv.net.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Zeebe.Client;
using Zeebe.Client.Api.Commands;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;
using Zeebe.Client.Impl.Builder;

namespace Camuda.WebApi.Infrastructure.Services
{
    public class ZeebeClientService : IZeebeClientService, IDisposable
    {
        private const string ZEEBE_ADDRESS = "ZEEBE_ADDRESS";
        private const string ZEEBE_AUTHORIZATION_SERVER_URL = "ZEEBE_AUTHORIZATION_SERVER_URL";
        private const string ZEEBE_CLIENT_ID = "ZEEBE_CLIENT_ID";
        private const string ZEEBE_CLIENT_SECRET = "ZEEBE_CLIENT_SECRET";

        private readonly IZeebeClient _client;
        private readonly IEnvReader _envReader;
        private readonly ComundaOptions _comundaOptions;
        private readonly ILogger<ZeebeClientService> _logger;

        public ZeebeClientService(
            IEnvReader envReader,
            ILogger<ZeebeClientService> logger,
            IOptions<ComundaOptions> comundaOptions)
        {
            _envReader = envReader;
            _logger = logger;
            _comundaOptions = comundaOptions.Value;
            _client = GetClient();
        }

        public Task<ITopology> Status(CancellationToken cancellationToken) 
            => _client.TopologyRequest().Send(cancellationToken);

        public async Task DeployAll(CancellationToken cancellationToken)
        {
            var builder = GetDeployBuilder();

            if (builder == default)
                return;

            var deployment = await builder.Send(cancellationToken);

            foreach (var process in deployment.Processes)
                _logger.LogInformation("Deployed BPMN Model: " + process!.BpmnProcessId + " v." + process!.Version + " process name: " + process!.ResourceName);
        }

        public async Task<IProcessInstanceResult> RunProcessInstance(string bpmProcessId, 
            CancellationToken cancellationToken, object? payload = null)
        {
            var processCommandBuilder = GetProcessInstanceBuilder(bpmProcessId, payload);

            try
            {
                var instance = await processCommandBuilder.WithResult()
                    .Send(TimeSpan.FromMinutes(_comundaOptions.ProcessInstanceRunTimeout), cancellationToken);

                return instance;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured during running process id {bpmProcessId}, " +
                    $"with payload: {JsonSerializer.Serialize(payload)}, ex: {ex}");
                throw;
            }
        }

        public async Task<IProcessInstanceResponse> CreateProcessInstance(string bpmProcessId,
            CancellationToken cancellationToken, object? payload = null)
        {
            var processCommandBuilder = GetProcessInstanceBuilder(bpmProcessId, payload);

            try
            {
                var instanceCreatedResult = await processCommandBuilder
                    .Send(TimeSpan.FromSeconds(_comundaOptions.ProcessInstanceRunTimeout), cancellationToken);

                return instanceCreatedResult;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured during creating process id {bpmProcessId}, " +
                    $"with payload: {JsonSerializer.Serialize(payload)}, ex: {ex}");
                throw;
            }
        }

        public IJobWorker CreateWorker(string jobType, JobHandler handleJob, params string[]? fetchVariables)
        {
            var worker = _client.NewWorker()
                    .JobType(jobType)
                    .Handler(handleJob)
                    .MaxJobsActive(_comundaOptions.MaxJobsActive)
                    .Name(jobType)
                    .PollInterval(TimeSpan.FromSeconds(_comundaOptions.JobsPollInterval))
                    .PollingTimeout(TimeSpan.FromSeconds(_comundaOptions.JobsPollingTimeout))
                    .Timeout(TimeSpan.FromSeconds(_comundaOptions.JobsTimeout))
                    .FetchVariables(fetchVariables)
                    .Open();

            return worker;
        }

        public async Task SendMessage(string messageName, string correlationKey, 
            object payload, CancellationToken cancellationToken)
        {
            await _client.NewPublishMessageCommand()
                .MessageName(messageName)
                .CorrelationKey(correlationKey)
                .Variables(JsonSerializer.Serialize(payload))
                .Send(cancellationToken);
        }

        private string GetProcessPath(string processName)
            => Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "Resources", processName + ".bpmn");

        private string GetDecisionPath(string decisionName)
             => Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "Resources\\Decisions", decisionName + ".dmn");

        private IDeployProcessCommandBuilderStep2? GetDeployBuilder()
        {
            IDeployProcessCommandStep1? builder = _client.NewDeployCommand();

            IDeployProcessCommandBuilderStep2 builderStep = default!;

            foreach (var processName in ComundaProcessesNames.All)
                builderStep = builder
                    .AddResourceFile(GetProcessPath(processName));

            foreach (var decisionName in ComundaDecisionsNames.All)
                builderStep = builder
                    .AddResourceFile(GetDecisionPath(decisionName));

            return builderStep;
        }

        private ICreateProcessInstanceCommandStep3 GetProcessInstanceBuilder(string bpmProcessId, object? payload = null)
        {
            var processCommandBuilder = _client
                 .NewCreateProcessInstanceCommand()
                 .BpmnProcessId(bpmProcessId)
                 .LatestVersion();

            if (payload is not null)
                processCommandBuilder = processCommandBuilder
                    .Variables(JsonSerializer.Serialize(payload));

            return processCommandBuilder;
        }

        private IZeebeClient GetClient() => _comundaOptions.IsLocalConnection ? GetLocalClient() : GetCloudClient();

        private IZeebeClient GetLocalClient()
        {
            var zeebeUrl = _envReader.GetStringValue(ZEEBE_ADDRESS);

            var builder = ZeebeClient.Builder()
                    .UseGatewayAddress(zeebeUrl)
                    .UsePlainText()
                    .Build();

            return builder;
        }

        private IZeebeClient GetCloudClient()
        {
            var zeebeUrl = _envReader.GetStringValue(ZEEBE_ADDRESS);
            char[] port = { '4', '3', ':' };
            var audience = zeebeUrl?.TrimEnd(port);
            var authServer = _envReader.GetStringValue(ZEEBE_AUTHORIZATION_SERVER_URL);
            var clientId = _envReader.GetStringValue(ZEEBE_CLIENT_ID);
            var clientSecret = _envReader.GetStringValue(ZEEBE_CLIENT_SECRET);

            return ZeebeClient.Builder()
                    .UseGatewayAddress(zeebeUrl)
                    .UseTransportEncryption()
                    .UseAccessTokenSupplier(
                         CamundaCloudTokenProvider.Builder()
                        .UseAuthServer(authServer)
                        .UseClientId(clientId)
                        .UseClientSecret(clientSecret)
                        .UseAudience(audience)
                        .Build())
                    .Build();
        }

        public void Dispose()
        {
            if(_client is not null)
            {
                _client.Dispose();
            }
        }
    }
}
