namespace Camunda.WebApi.Options;

public class CamundaOptions
{
    public static string Key => "Camunda";
    public string LocalConnectionAddress { get; set; } = default!;
    public bool IsLocalConnection { get; set; } = default!;
    public int ProcessInstanceRunTimeout { get; set; } = default!;
    public int MaxJobsActive { get; set; } = default!;
    public int JobsPollInterval { get; set; } = default!;
    public int JobsPollingTimeout { get; set; } = default!;
    public int JobsTimeout { get; set; } = default!;
}