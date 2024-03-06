using System.Globalization;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;

namespace HE.Investments.Common.Domain.ValueObjects;

public abstract class YourRequiredIntValueObject : ValueObject
{
    protected YourRequiredIntValueObject(
        string? value,
        string fieldName,
        string displayName,
        int minValue = int.MinValue,
        int maxValue = int.MaxValue)
    {
        if (value.IsNotProvided())
        {
            OperationResult.New()
                .AddValidationError(fieldName, ValidationErrorMessage.MustProvideYourRequiredField(displayName))
                .CheckErrors();
        }

        if (!int.TryParse(value!, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue))
        {
            OperationResult.New()
                .AddValidationError(fieldName, ValidationErrorMessage.MustProvideYourWholeNumber(displayName))
                .CheckErrors();
        }

        if (parsedValue < minValue)
        {
            OperationResult.New()
                .AddValidationError(fieldName, ValidationErrorMessage.MustProvideYourHigherNumber(displayName, minValue))
                .CheckErrors();
        }

        if (parsedValue > maxValue)
        {
            OperationResult.New()
                .AddValidationError(fieldName, ValidationErrorMessage.MustProvideYourLowerNumber(displayName, maxValue))
                .CheckErrors();
        }

        Value = parsedValue;
    }

    protected YourRequiredIntValueObject(
        int value,
        string fieldName,
        string displayName,
        int minValue = int.MinValue,
        int maxValue = int.MaxValue)
    {
        if (value < minValue)
        {
            OperationResult.New()
                .AddValidationError(fieldName, ValidationErrorMessage.MustProvideYourHigherNumber(displayName, minValue))
                .CheckErrors();
        }

        if (value > maxValue)
        {
            OperationResult.New()
                .AddValidationError(fieldName, ValidationErrorMessage.MustProvideYourLowerNumber(displayName, maxValue))
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
