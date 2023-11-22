using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class ExpectedPurchasePrice : ValueObject
{
    private ExpectedPurchasePrice(int value)
    {
        if (value < 0)
        {
            OperationResult.New()
                .AddValidationError(FinancialDetailsValidationFieldNames.ExpectedPurchasePrice, FinancialDetailsValidationErrors.InvalidExpectedPurchasePrice)
                .CheckErrors();
        }

        Value = value;
    }

    public int Value { get; }

    public static ExpectedPurchasePrice From(string value)
    {
        if (!int.TryParse(value, out var parsedValue))
        {
            OperationResult.New()
                .AddValidationError(FinancialDetailsValidationFieldNames.ExpectedPurchasePrice, FinancialDetailsValidationErrors.InvalidExpectedPurchasePrice)
                .CheckErrors();
        }

        return new ExpectedPurchasePrice(parsedValue);
    }

    public static ExpectedPurchasePrice? From(decimal? value)
    {
        if (value == null)
        {
            return null;
        }
        else
        {
            return ExpectedPurchasePrice.From(value.ToWholeNumberString() ?? string.Empty);
        }
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
