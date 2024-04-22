using System.Globalization;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Utils;
using HE.Investments.Common.Validators;

namespace HE.Investments.Common.Domain.ValueObjects;

public abstract class TheRequiredIntValueObject : ValueObject
{
    protected TheRequiredIntValueObject(
        string? value,
        string fieldName,
        string displayName,
        int minValue = int.MinValue,
        int maxValue = int.MaxValue,
        MessageOptions options = MessageOptions.None)
    {
        var isCalculation = options.HasFlag(MessageOptions.Calculation);
        var isMoney = options.HasFlag(MessageOptions.Money);
        var example = options.HasFlag(MessageOptions.HideExample) ? null : "300";

        Value = NumberParser.TryParseDecimal(value, minValue, maxValue, 0, out var parsedValue) switch
        {
            NumberParseResult.ValueMissing => ThrowValidationError(
                fieldName,
                isCalculation ? ValidationErrorMessage.MustBeProvidedForCalculation(displayName) : ValidationErrorMessage.MustProvideRequiredField(displayName)),
            NumberParseResult.ValueNotANumber => ThrowValidationError(fieldName, ValidationErrorMessage.MustBeTheWholeNumber(displayName, example)),
            NumberParseResult.ValueInvalidPrecision => ThrowValidationError(
                fieldName,
                isMoney ? ValidationErrorMessage.MustNotIncludeThePence(displayName, example) : ValidationErrorMessage.MustBeTheWholeNumber(displayName, example)),
            NumberParseResult.ValueTooHigh => ThrowValidationError(fieldName, ValidationErrorMessage.MustProvideTheLowerNumber(displayName, maxValue)),
            NumberParseResult.ValueTooLow => ThrowValidationError(fieldName, ValidationErrorMessage.MustProvideTheHigherNumber(displayName, minValue)),
            NumberParseResult.SuccessfullyParsed => (int)parsedValue!.Value,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
        };
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

    private static int ThrowValidationError(string affectedField, string validationMessage) =>
        OperationResult.ThrowValidationError<int>(affectedField, validationMessage);
}
