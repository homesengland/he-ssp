using System.Globalization;
using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;

public class ExpectedPurchasePrice : ValueObject
{
    public const string DisplayName = "The expected purchase price of the land";

    public ExpectedPurchasePrice(decimal value)
    {
        Value = PoundsValidator.Validate(value, FinancialDetailsValidationFieldNames.PurchasePrice, DisplayName);
    }

    public decimal Value { get; }

    public static ExpectedPurchasePrice From(string value)
    {
        if (value.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError(FinancialDetailsValidationFieldNames.PurchasePrice, ValidationErrorMessage.MissingRequiredField(DisplayName))
                .CheckErrors();
        }

        if (!decimal.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var parsedValue))
        {
            OperationResult.New()
                .AddValidationError(FinancialDetailsValidationFieldNames.PurchasePrice, FinancialDetailsValidationErrors.InvalidExpectedPurchasePrice)
                .CheckErrors();
        }

        return new ExpectedPurchasePrice(parsedValue);
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
