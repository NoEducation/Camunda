using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Camunda.WebApi.Options;
using Microsoft.Extensions.Options;

namespace Camunda.WebApi.Infrastructure.Services.Http;

public class CamundaHttpService : ICamundaHttpService
{
    private readonly CamundaEnvironmentOptions _camundaEnvironmentOptions;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<CamundaHttpService> _logger;

    private Token? _token;

    public CamundaHttpService(IOptions<CamundaEnvironmentOptions> camundaEnvironmentOptions,
        IHttpClientFactory httpClientFactory,
        ILogger<CamundaHttpService> logger)
    {
        _camundaEnvironmentOptions = camundaEnvironmentOptions.Value;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<TResponse?> Get<TResponse>(Uri uri, CancellationToken cancellationToken)
    {
        using var client = await PrepareClient(cancellationToken);

        var response = await client.GetFromJsonAsync<TResponse>(uri, cancellationToken);

        return response;
    }

    public async Task<TResponse?> Post<TResponse>(Uri uri, CancellationToken cancellationToken, object? payload = null)
    {
        using var client = await PrepareClient(cancellationToken);

        var response = await client.PostAsJsonAsync(uri, payload, cancellationToken);

        var result = await response.Content
            .ReadFromJsonAsync<TResponse>(JsonSerializerOptions.Default, cancellationToken);

        return result;
    }

    private async Task<HttpClient> PrepareClient(CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient();

        await CheckToken(client, cancellationToken);

        client.DefaultRequestHeaders.Accept
            .Add(new MediaTypeWithQualityHeaderValue("application/json"));

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", $"{_token!.AccessToken}");

        return client;
    }

    private async Task CheckToken(HttpClient client, CancellationToken cancellationToken)
    {
        if (_token is null || string.IsNullOrWhiteSpace(_token.AccessToken))
            await SetAccessToken(client, cancellationToken);

        if (_token!.ExpiresAt > DateTimeOffset.Now.AddMinutes(-1))
        {
            _logger.LogInformation("Access token needs to be refreshed");
            await SetAccessToken(client, cancellationToken);
        }
    }

    private async Task SetAccessToken(HttpClient client, CancellationToken cancellationToken)
    {
        var result = await client.PostAsJsonAsync(
            new Uri(_camundaEnvironmentOptions.ZEEBE_AUTHORIZATION_SERVER_URL),
            new
            {
                client_id = _camundaEnvironmentOptions.ZEEBE_CLIENT_ID,
                client_secret = _camundaEnvironmentOptions.ZEEBE_CLIENT_SECRET,
                audience = "operate.camunda.io",
                grant_type = "client_credentials"
            }, cancellationToken);

        _token = await result.Content.ReadFromJsonAsync<Token>(JsonSerializerOptions.Default, cancellationToken);
    }

    private class Token
    {
        public Token(string accessToken, int expiresIn)
        {
            AccessToken = accessToken;
            ExpiresIn = expiresIn;
        }

        [JsonPropertyName("access_token")] public string AccessToken { get; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn
        {
            set => ExpiresAt = DateTimeOffset.Now.AddSeconds(value);
        }

        public DateTimeOffset ExpiresAt { get; private set; }
    }
}