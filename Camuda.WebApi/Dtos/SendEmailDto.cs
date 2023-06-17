namespace Camuda.WebApi.Dtos
{
    public class SendEmailDto
    {
        public string EmailBody { get; set; } = default!;
        public string EmailTitle { get; set; } = default!;
    }
}
