namespace Camunda.WebApi.Infrastructure.Services.Http;

public interface ICamundaHttpService
{
    Task<TResponse?> Get<TResponse>(Uri uri, CancellationToken cancellationToken);
    Task<TResponse?> Post<TResponse>(Uri uri, CancellationToken cancellationToken, object? payload = null);
}