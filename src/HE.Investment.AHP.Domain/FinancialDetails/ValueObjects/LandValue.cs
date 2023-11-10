using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.InvestmentLoans.Common.Extensions;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class LandValue : ValueObject
{
    public LandValue(string value)
    {
        if (!int.TryParse(value, out var valueInt) || valueInt <= 0)
        {
            OperationResult.New()
            .AddValidationError(FinancialDetailsValidationFieldNames.LandValue, FinancialDetailsValidationErrors.InvalidLandValue)
            .CheckErrors();
        }

        Value = valueInt;
    }

    public int Value { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
