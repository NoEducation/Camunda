namespace Camunda.WebApi.Options;

public class CamundaEnvironmentOptions
{
    public static string Key => "CamundaEnvironment";

    public string ZEEBE_ADDRESS { get; set; } = default!;
    public string ZEEBE_CLIENT_ID { get; set; } = default!;
    public string ZEEBE_CLIENT_SECRET { get; set; } = default!;
    public string ZEEBE_AUTHORIZATION_SERVER_URL { get; set; } = default!;
    public string ZEEBE_TOKEN_AUDIENCE { get; set; } = default!;
    public string CAMUNDA_CLUSTER_ID { get; set; } = default!;
    public string CAMUNDA_CLUSTER_REGION { get; set; } = default!;
    public string CAMUNDA_CREDENTIALS_SCOPES { get; set; } = default!;
    public string CAMUNDA_TASKLIST_BASE_URL { get; set; } = default!;
    public string CAMUNDA_OPTIMIZE_BASE_URL { get; set; } = default!;
    public string CAMUNDA_OPERATE_BASE_URL { get; set; } = default!;
    public string CAMUNDA_OAUTH_URL { get; set; } = default!;
}