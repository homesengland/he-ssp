using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class LandValue : ValueObject
{
    public LandValue(string value)
    {
        var operationResult = OperationResult.New();

        var intValue = NumericValidator
            .For(value, FinancialDetailsValidationFieldNames.LandValue, operationResult)
            .IsWholeNumber(FinancialDetailsValidationErrors.InvalidLandValue)
            .IsBetween(1, 999999999, FinancialDetailsValidationErrors.InvalidLandValue);

        operationResult.CheckErrors();

        Value = intValue;
    }

    public int Value { get; }

    public static LandValue? From(decimal? value)
    {
        if (value == null)
        {
            return null;
        }
        else
        {
            return new LandValue(value.ToWholeNumberString() ?? string.Empty);
        }
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
