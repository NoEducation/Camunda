namespace Camuda.WebApi.Consts
{
    public static class ComundaProcessesNames
    {
        public const string SendEmail = "send_email",
               TestProcess = "test_process",
               BallmerPeakProcess = "BallmerPeakProcess",
               EmailIsSend = "email_is_send";

        public static IReadOnlyList<string> All => new string[]
        { 
            TestProcess,
            SendEmail,
            BallmerPeakProcess ,
            EmailIsSend
        };
    }
}
