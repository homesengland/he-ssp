using System.Globalization;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public abstract class RequiredIntValueObject : ValueObject
{
    protected RequiredIntValueObject(
        string? value,
        string fieldName,
        string displayName,
        int minValue = int.MinValue,
        int maxValue = int.MaxValue)
    {
        if (value.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError(fieldName, ValidationErrorMessage.MissingRequiredField(displayName))
                .CheckErrors();
        }

        if (!int.TryParse(value!, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue))
        {
            OperationResult.New()
                .AddValidationError(fieldName, ValidationErrorMessage.MustBeNumber(displayName, minValue, maxValue))
                .CheckErrors();
        }

        if (parsedValue < minValue || parsedValue > maxValue)
        {
            OperationResult.New()
                .AddValidationError(fieldName, ValidationErrorMessage.MustBeNumber(displayName, minValue, maxValue))
                .CheckErrors();
        }

        Value = parsedValue;
    }

    protected RequiredIntValueObject(
        int value,
        string fieldName,
        string displayName,
        int minValue = int.MinValue,
        int maxValue = int.MaxValue)
    {
        if (value < minValue || value > maxValue)
        {
            OperationResult.New()
                .AddValidationError(fieldName, ValidationErrorMessage.MustBeNumber(displayName, minValue, maxValue))
                .CheckErrors();
        }

        Value = value;
    }

    public int Value { get; }

    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
