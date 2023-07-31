using HE.InvestmentLoans.Common.Utils.Constants.FormOption;

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
        return answer == CommonResponse.Yes ? true : answer == CommonResponse.No ? false : null;
    }

    public bool? AnswerAsBool()
    {
        return Answer == CommonResponse.Yes ? true : Answer == CommonResponse.No ? false : null;
    }
}
