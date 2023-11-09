using HE.InvestmentLoans.Contract.Common;

namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class MoreInformation : LongText
{
    public MoreInformation(string value)
        : base(value, nameof(MoreInformation))
    {
    }
}
