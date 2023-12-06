using System.Globalization;
using HE.Investment.AHP.Domain.FinancialDetails.Constants;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.FinancialDetails.ValueObjects;

public class CurrentLandValue : ValueObject
{
    public const string DisplayName = "Current Land Value";

    public CurrentLandValue(decimal landValue)
    {
        Value = PoundsValidator.Validate(landValue, FinancialDetailsValidationFieldNames.LandValue, DisplayName);
    }

    public decimal Value { get; }

    public static CurrentLandValue From(string value)
    {
        if (value.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError(FinancialDetailsValidationFieldNames.LandValue, ValidationErrorMessage.MissingRequiredField(DisplayName))
                .CheckErrors();
        }

        if (!decimal.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var parsedValue))
        {
            OperationResult.New()
                .AddValidationError(FinancialDetailsValidationFieldNames.LandValue, FinancialDetailsValidationErrors.InvalidLandValue)
                .CheckErrors();
        }

        return new CurrentLandValue(parsedValue);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
