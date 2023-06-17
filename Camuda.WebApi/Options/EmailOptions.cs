namespace Camunda.WebApi.Options;

public class EmailOptions
{
    public const string Key = "Email";

    public string Host { get; set; } = default!;
    public int Port { get; set; } = default!;
    public bool EnableSsl { get; set; } = default!;
    public string From { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string FromDisplayName { get; set; } = default!;
    public string To { get; set; } = default!;
}