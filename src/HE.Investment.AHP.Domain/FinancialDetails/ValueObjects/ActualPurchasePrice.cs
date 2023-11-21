using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class ActualPurchasePrice : ValueObject
{
    private ActualPurchasePrice(int value)
    {
        if (value < 0)
        {
            OperationResult.New()
                .AddValidationError(FinancialDetailsValidationFieldNames.ActualPurchasePrice, FinancialDetailsValidationErrors.InvalidActualPurchasePrice)
                .CheckErrors();
        }

        Value = value;
    }

    public int Value { get; }

    public static ActualPurchasePrice From(string value)
    {
        if (!int.TryParse(value, out var parsedValue))
        {
            OperationResult.New()
                .AddValidationError(FinancialDetailsValidationFieldNames.ActualPurchasePrice, FinancialDetailsValidationErrors.InvalidActualPurchasePrice)
                .CheckErrors();
        }

        return new ActualPurchasePrice(parsedValue);
    }

    public static ActualPurchasePrice? From(decimal? value)
    {
        if (value == null)
        {
            return null;
        }
        else
        {
            return ActualPurchasePrice.From(value.ToWholeNumberString() ?? string.Empty);
        }
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
