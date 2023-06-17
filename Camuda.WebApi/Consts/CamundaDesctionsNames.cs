namespace Camunda.WebApi.Consts;

public class CamundaDecisionsNames
{
    public const string SaySomethingNiceDecision = "say_something_nice_decision";

    public static IReadOnlyList<string> All => new[]
    {
        SaySomethingNiceDecision
    };
}