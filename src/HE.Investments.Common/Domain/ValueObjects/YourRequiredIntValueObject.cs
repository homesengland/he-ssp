using System.Globalization;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Utils;
using HE.Investments.Common.Validators;

namespace HE.Investments.Common.Domain.ValueObjects;

public abstract class YourRequiredIntValueObject : ValueObject
{
    protected YourRequiredIntValueObject(
        string? value,
        string fieldName,
        string displayName,
        int minValue = int.MinValue,
        int maxValue = int.MaxValue,
        MessageOptions options = MessageOptions.None)
    {
        var example = options.HasFlag(MessageOptions.HideExample) ? null : "30";
        var isMoney = options.HasFlag(MessageOptions.IsMoney);

        Value = NumberParser.TryParseDecimal(value, minValue, maxValue, 0, out var parsedValue) switch
        {
            NumberParseResult.ValueMissing => ThrowValidationError(fieldName, ValidationErrorMessage.MustProvideYourRequiredField(displayName)),
            NumberParseResult.ValueNotANumber => ThrowValidationError(fieldName, ValidationErrorMessage.MustBeYourWholeNumber(displayName, example)),
            NumberParseResult.ValueInvalidPrecision => ThrowValidationError(
                fieldName,
                isMoney ? ValidationErrorMessage.MustNotIncludeYourPence(displayName, example) : ValidationErrorMessage.MustBeTheWholeNumber(displayName, example)),
            NumberParseResult.ValueTooHigh => ThrowValidationError(fieldName, ValidationErrorMessage.MustProvideYourLowerNumber(displayName, maxValue)),
            NumberParseResult.ValueTooLow => ThrowValidationError(fieldName, ValidationErrorMessage.MustProvideYourHigherNumber(displayName, minValue)),
            NumberParseResult.SuccessfullyParsed => (int)parsedValue!.Value,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
        };
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
            OperationResult.ThrowValidationError(fieldName, ValidationErrorMessage.MustProvideYourHigherNumber(displayName, minValue));
        }

        if (value > maxValue)
        {
            OperationResult.ThrowValidationError(fieldName, ValidationErrorMessage.MustProvideYourLowerNumber(displayName, maxValue));
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

    private static int ThrowValidationError(string affectedField, string validationMessage) =>
        OperationResult.ThrowValidationError<int>(affectedField, validationMessage);
}
