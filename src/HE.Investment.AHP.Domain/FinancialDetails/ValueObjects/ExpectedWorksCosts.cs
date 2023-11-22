using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class ExpectedWorksCosts : ValueObject
{
    public ExpectedWorksCosts(string value)
    {
        var operationResult = OperationResult.New();

        var intValue = NumericValidator
            .For(value, FinancialDetailsValidationFieldNames.ExpectedWorksCosts, operationResult)
            .IsWholeNumber(FinancialDetailsValidationErrors.InvalidExpectedWorksCosts)
            .IsBetween(1, 999999999, FinancialDetailsValidationErrors.InvalidExpectedWorksCosts);

        operationResult.CheckErrors();

        Value = intValue;
    }

    public int Value { get; }

    public static ExpectedWorksCosts? From(decimal? value)
    {
        if (value == null)
        {
            return null;
        }

        return new ExpectedWorksCosts(value.ToWholeNumberString() ?? string.Empty);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
