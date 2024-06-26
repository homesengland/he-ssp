using System.Globalization;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Utils;

namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class InitialSale : ValueObject
{
    private const string DisplayName = "assumed average first tranche sale percentage";

    private const decimal MinValue = 0.1m;

    private const decimal MaxValue = 0.75m;

    public InitialSale(string? value, bool isCalculation = false)
    {
        var minValue = MinValue.ToPercentage100();
        var maxValue = MaxValue.ToPercentage100();

        Value = NumberParser.TryParseDecimal(value, minValue, maxValue, 0, out var parsedValue) switch
        {
            NumberParseResult.ValueMissing => ThrowValidationError(
                nameof(InitialSale),
                isCalculation ? ValidationErrorMessage.MustBeProvidedForCalculationWithPercentage(DisplayName) : ValidationErrorMessage.MustProvideRequiredField(DisplayName)),
            NumberParseResult.ValueNotANumber => ThrowValidationError(nameof(InitialSale), ValidationErrorMessage.MustBeTheWholeNumber(DisplayName, null)),
            NumberParseResult.ValueInvalidPrecision => ThrowValidationError(nameof(InitialSale), ValidationErrorMessage.MustBeTheWholeNumber(DisplayName, null)),
            NumberParseResult.ValueTooHigh => ThrowValidationError(nameof(InitialSale), ValidationErrorMessage.MustProvideNumberBetween(DisplayName, minValue, maxValue)),
            NumberParseResult.ValueTooLow => ThrowValidationError(nameof(InitialSale), ValidationErrorMessage.MustProvideNumberBetween(DisplayName, minValue, maxValue)),
            NumberParseResult.SuccessfullyParsed => parsedValue!.Value / 100m,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
        };
    }

    public InitialSale(decimal value)
    {
        if (value is < MinValue or > MaxValue)
        {
            ThrowValidationError(
                nameof(InitialSale),
                ValidationErrorMessage.MustProvideNumberBetween(DisplayName, MinValue.ToPercentage100(), MaxValue.ToPercentage100()));
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

    private static int ThrowValidationError(string affectedField, string validationMessage) =>
        OperationResult.ThrowValidationError<int>(affectedField, validationMessage);
}
