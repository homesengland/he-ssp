using System.Globalization;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Utils;
using HE.Investments.Common.Validators;

namespace HE.Investments.Common.Domain.ValueObjects;

public abstract class TheRequiredDecimalValueObject : ValueObject
{
    protected TheRequiredDecimalValueObject(
        string? value,
        string fieldName,
        string displayName,
        decimal minValue = decimal.MinValue,
        decimal maxValue = decimal.MaxValue,
        int precision = 2,
        MessageOptions options = MessageOptions.None)
    {
        var isMoney = options.HasFlag(MessageOptions.Money);
        var example = BuildExample(options, precision);

        Value = NumberParser.TryParseDecimal(value, minValue, maxValue, precision, out var parsedValue) switch
        {
            NumberParseResult.ValueMissing => ThrowValidationError(fieldName, GetValueMissingMessage(options)(displayName)),
            NumberParseResult.ValueNotANumber => ThrowValidationError(fieldName, ValidationErrorMessage.MustBeTheNumber(displayName, example)),
            NumberParseResult.ValueInvalidPrecision => ThrowValidationError(
                fieldName,
                isMoney ? ValidationErrorMessage.MustIncludeThePence(displayName, example) : ValidationErrorMessage.MustIncludeThePrecision(displayName, precision, example)),
            NumberParseResult.ValueTooHigh => ThrowValidationError(fieldName, ValidationErrorMessage.MustProvideTheLowerNumber(displayName, maxValue)),
            NumberParseResult.ValueTooLow => ThrowValidationError(fieldName, ValidationErrorMessage.MustProvideTheHigherNumber(displayName, minValue)),
            NumberParseResult.SuccessfullyParsed => parsedValue!.Value,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
        };
    }

    protected TheRequiredDecimalValueObject(
        decimal value,
        string fieldName,
        string displayName,
        decimal minValue = decimal.MinValue,
        decimal maxValue = decimal.MaxValue)
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

    public decimal Value { get; }

    public override string ToString()
    {
        return Value.ToString(CultureInfo.InvariantCulture);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    private static string? BuildExample(MessageOptions messageOptions, int precision)
    {
        if (messageOptions.HasFlag(MessageOptions.HideExample))
        {
            return null;
        }

        var example = "300";
        if (precision <= 0)
        {
            return example;
        }

        return $"{example}.{new string(Enumerable.Repeat('0', precision).ToArray())}";
    }

    private static decimal ThrowValidationError(string affectedField, string validationMessage) =>
        OperationResult.ThrowValidationError<decimal>(affectedField, validationMessage);

    private static Func<string, string> GetValueMissingMessage(MessageOptions options)
    {
        if (options.HasFlag(MessageOptions.Calculation))
        {
            return ValidationErrorMessage.MustBeProvidedForCalculation;
        }

        return options.HasFlag(MessageOptions.Money)
            ? ValidationErrorMessage.MustProvideRequiredFieldInPoundsAndPence
            : ValidationErrorMessage.MustProvideRequiredField;
    }
}
