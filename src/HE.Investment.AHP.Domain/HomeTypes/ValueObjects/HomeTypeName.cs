using HE.InvestmentLoans.Contract.Common;

namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class HomeTypeName : ShortText
{
    public HomeTypeName(string? value) : base(value, nameof(HomeTypeName))
    {
    }
}
