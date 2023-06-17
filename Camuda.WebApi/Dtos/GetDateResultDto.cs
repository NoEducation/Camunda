namespace Camunda.WebApi.Dtos;

public class GetDateResultDto
{
    public GetDateResultDto(DateTimeOffset time)
    {
        Time = time.ToString("o");
        Hour = time.ToLocalTime().Hour;
        DayOfWeek = time.DayOfWeek;
    }

    public string Time { get; }
    public int Hour { get; }
    public DayOfWeek DayOfWeek { get; }
}