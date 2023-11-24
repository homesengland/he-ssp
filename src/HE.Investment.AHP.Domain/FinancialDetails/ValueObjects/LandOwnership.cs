using HE.Investments.Common.Domain;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;

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
