using HE.InvestmentLoans.Contract.Common;

namespace HE.Investment.AHP.BusinessLogic.HomeTypes.ValueObjects;

public class HomeTypeName : ShortText
{
    public HomeTypeName(string? value) : base(value, nameof(HomeTypeName))
    {
    }
}
