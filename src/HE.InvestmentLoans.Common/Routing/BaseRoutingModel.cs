namespace HE.InvestmentLoans.Common.Routing;

/// <summary>
/// Base model for all Routing requests and responses.
/// </summary>
public class BaseRoutingModel
{
    public Guid Id { get; set; }

    public string? Answer { get; set; }

    public string? Name { get; set; }

    public bool? AnswerAsBool(string answer)
    {
        return answer == "Yes" ? true : answer == "No" ? false : null;
    }

    public bool? AnswerAsBool()
    {
        return Answer == "Yes" ? true : Answer == "No" ? false : null;
    }
}
