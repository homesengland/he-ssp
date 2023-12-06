using System.Globalization;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public abstract class DecimalValueObject : ValueObject
{
    protected DecimalValueObject(
        string? value,
        string fieldName,
        string displayName,
        bool isCalculation,
        decimal minValue = decimal.MinValue,
        decimal maxValue = decimal.MaxValue)
    {
        if (value.IsNotProvided() && !isCalculation)
        {
            return;
        }

        if (value.IsNotProvided() && isCalculation)
        {
            OperationResult.New()
                .AddValidationError(fieldName, ValidationErrorMessage.MustBeProvidedForCalculation(displayName))
                .CheckErrors();
        }

        if (value.IsProvided())
        {
            if (!decimal.TryParse(value!, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var parsedValue))
            {
                OperationResult.New()
                    .AddValidationError(fieldName, ValidationErrorMessage.MustBeNumber(displayName))
                    .CheckErrors();
            }

            if (decimal.Round(parsedValue, 2) != parsedValue)
            {
                OperationResult.New()
                    .AddValidationError(fieldName, ValidationErrorMessage.MustBeNumber(displayName))
                    .CheckErrors();
            }

            if (parsedValue < minValue || parsedValue > maxValue)
            {
                OperationResult.New()
                    .AddValidationError(fieldName, ValidationErrorMessage.MustBeDecimalNumberBetween(displayName, minValue, maxValue))
                    .CheckErrors();
            }

            Value = parsedValue;
        }
    }

    protected DecimalValueObject(
        decimal value,
        string fieldName,
        string displayName,
        decimal minValue = decimal.MinValue,
        decimal maxValue = decimal.MaxValue)
    {
        if (value < minValue || value > maxValue)
        {
            OperationResult.New()
                .AddValidationError(fieldName, ValidationErrorMessage.MustBeDecimalNumberBetween(displayName, minValue, maxValue))
                .CheckErrors();
        }

        Value = decimal.Round(value, 2);
    }

    public decimal Value { get; }

    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
