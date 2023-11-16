using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.InvestmentLoans.Common.Extensions;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class LandValue : ValueObject
{
    public LandValue(string value)
    {
        var (isValid, valueInt) = AmountValidator.Validate(value);
        if (!isValid || !valueInt.HasValue)
        {
            OperationResult.New()
                .AddValidationError(FinancialDetailsValidationFieldNames.LandValue, FinancialDetailsValidationErrors.InvalidLandValue)
                .CheckErrors();
        }
        else
        {
            Value = valueInt.Value;
        }
    }

    public int Value { get; }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
