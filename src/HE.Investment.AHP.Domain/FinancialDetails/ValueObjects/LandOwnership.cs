using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.Investments.Common.Domain;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class LandOwnership : ValueObject
{
    public LandOwnership(string value)
    {
        Value = value == CommonResponse.Yes;
    }

    public bool Value { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
