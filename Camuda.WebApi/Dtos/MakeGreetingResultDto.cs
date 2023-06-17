namespace Camunda.WebApi.Dtos;

public class MakeGreetingResultDto
{
    public MakeGreetingResultDto(string greeting, string name)
    {
        Greeting = greeting;
        Name = name;
    }

    public string Greeting { get; }
    public string Name { get; }
}