using System.Globalization;
using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;
public class PurchasePrice : ValueObject
{
    public const string DisplayName = "The purchase price of the land";

    public PurchasePrice(decimal value)
    {
        Value = PoundsValidator.Validate(value, FinancialDetailsValidationFieldNames.PurchasePrice, DisplayName);
    }

    public decimal Value { get; }

    public static PurchasePrice From(string value)
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
                .AddValidationError(FinancialDetailsValidationFieldNames.PurchasePrice, FinancialDetailsValidationErrors.InvalidActualPurchasePrice)
                .CheckErrors();
        }

        return new PurchasePrice(parsedValue);
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Value;
    }
}
