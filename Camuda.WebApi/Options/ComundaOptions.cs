namespace Camuda.WebApi.Options
{
    public class ComundaOptions
    {
        public static string Key => "Comunda";
        public bool IsLocalConnection { get; set; } = default!;
        public int ProcessInstanceRunTimeout { get; set; } = default!;
        public int MaxJobsActive { get; set; } = default!;
        public int JobsPollInterval { get; set; } = default!;
        public int JobsPollingTimeout { get; set; } = default!;
        public int JobsTimeout { get; set; } = default!;
    }
}
