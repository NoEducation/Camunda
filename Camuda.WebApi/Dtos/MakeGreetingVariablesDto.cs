namespace Camuda.WebApi.Dtos
{
    public class MakeGreetingVariablesDto
    {
        public string Name { get; set; } = default!;

        public MakeGreetingVariablesDto(string name)
        {
            Name = name;
        }
    }
}
