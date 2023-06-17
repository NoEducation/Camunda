namespace Camuda.WebApi.Dtos
{
    public class GetDateResultDto
    {
        public string Time { get; private set; }
        public int Hour { get; private set; }
        public DayOfWeek DayOfWeek { get; private set; }

        public GetDateResultDto(DateTimeOffset time)
        {
            Time = time.ToString("o");
            Hour = time.ToLocalTime().Hour;
            DayOfWeek = time.DayOfWeek;
        }
    }
}
