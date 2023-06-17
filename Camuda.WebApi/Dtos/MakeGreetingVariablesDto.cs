namespace Camunda.WebApi.Dtos;

public class MakeGreetingVariablesDto
{
    public MakeGreetingVariablesDto(string name)
    {
        Name = name;
    }

    public string Name { get; set; } = default!;
}