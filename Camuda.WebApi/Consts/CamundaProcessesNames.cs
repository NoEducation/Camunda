namespace Camunda.WebApi.Consts;

public static class CamundaProcessesNames
{
    public const string SendEmail = "send_email",
        TestProcess = "test_process",
        BallmerPeakProcess = "BallmerPeakProcess",
        EmailIsSend = "email_is_send";

    public static IReadOnlyList<string> All => new[]
    {
        TestProcess,
        SendEmail,
        BallmerPeakProcess,
        EmailIsSend
    };
}