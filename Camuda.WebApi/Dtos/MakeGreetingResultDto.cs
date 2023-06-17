namespace Camuda.WebApi.Dtos
{
    public class MakeGreetingResultDto
    {
        public string Greeting { get; private set; } 
        public string Name { get; private set; }

        public MakeGreetingResultDto(string greeting, string name)
        {
            Greeting = greeting;
            Name = name;
        }
    }
}
