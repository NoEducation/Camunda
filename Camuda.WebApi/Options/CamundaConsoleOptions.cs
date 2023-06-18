namespace Camunda.WebApi.Options;

public class CamundaConsoleOptions
{
    public static string Key => "CamundaConsole";

    public string CAMUNDA_CONSOLE_CLIENT_ID { get; set; } = default!;
    public string CAMUNDA_CONSOLE_CLIENT_SECRET { get; set; } = default!;
    public string CAMUNDA_OAUTH_URL { get; set; } = default!;
    public string CAMUNDA_CONSOLE_BASE_URL { get; set; } = default!;
    public string CAMUNDA_CONSOLE_OAUTH_AUDIENCE { get; set; } = default!;
}