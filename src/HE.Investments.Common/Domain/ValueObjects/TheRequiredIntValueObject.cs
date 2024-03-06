using System.Globalization;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;

namespace HE.Investments.Common.Domain.ValueObjects;

public abstract class TheRequiredIntValueObject : ValueObject
{
    protected TheRequiredIntValueObject(
        string? value,
        string fieldName,
        string displayName,
        int minValue = int.MinValue,
        int maxValue = int.MaxValue)
    {
        if (value.IsNotProvided())
        {
            OperationResult.ThrowValidationError(fieldName, ValidationErrorMessage.MustProvideRequiredField(displayName));
        }

        if (!int.TryParse(value!, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue))
        {
            OperationResult.ThrowValidationError(fieldName, ValidationErrorMessage.MustBeWholeNumberWithExample(displayName));
        }

        if (parsedValue < minValue)
        {
            OperationResult.ThrowValidationError(fieldName, ValidationErrorMessage.MustProvideTheHigherNumber(displayName, minValue));
        }

        if (parsedValue > maxValue)
        {
            OperationResult.ThrowValidationError(fieldName, ValidationErrorMessage.MustProvideTheLowerNumber(displayName, maxValue));
        }

        Value = parsedValue;
    }

    protected TheRequiredIntValueObject(
        int value,
        string fieldName,
        string displayName,
        int minValue = int.MinValue,
        int maxValue = int.MaxValue)
    {
        if (value < minValue)
        {
            OperationResult.ThrowValidationError(fieldName, ValidationErrorMessage.MustProvideTheHigherNumber(displayName, minValue));
        }

        if (value > maxValue)
        {
            OperationResult.ThrowValidationError(fieldName, ValidationErrorMessage.MustProvideTheLowerNumber(displayName, maxValue));
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
