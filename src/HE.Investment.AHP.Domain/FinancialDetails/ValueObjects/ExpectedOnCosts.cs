using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class ExpectedOnCosts : ValueObject
{
    public ExpectedOnCosts(string value)
    {
        var operationResult = OperationResult.New();

        var intValue = NumericValidator
            .For(value, FinancialDetailsValidationFieldNames.ExpectedOnCosts, operationResult)
            .IsWholeNumber(FinancialDetailsValidationErrors.InvalidExpectedOnCosts)
            .IsBetween(1, 999999999, FinancialDetailsValidationErrors.InvalidExpectedOnCosts);

        operationResult.CheckErrors();

        Value = intValue;
    }

    public decimal Value { get; }

    public static ExpectedOnCosts? From(decimal? value)
    {
        if (value == null)
        {
            return null;
        }

        return new ExpectedOnCosts(value.ToWholeNumberString() ?? string.Empty);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
