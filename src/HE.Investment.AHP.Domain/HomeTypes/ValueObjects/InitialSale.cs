using System.Globalization;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class InitialSale : ValueObject
{
    private const string DisplayName = "assumed average first tranche sale";

    private const int MinValue = 10;

    private const int MaxValue = 75;

    public InitialSale(string? value, bool isCalculation = false)
    {
        if (value.IsNotProvided() && !isCalculation)
        {
            OperationResult.New()
                .AddValidationError(nameof(InitialSale), ValidationErrorMessage.MissingRequiredField(DisplayName))
                .CheckErrors();
        }

        if (value.IsNotProvided() && isCalculation)
        {
            OperationResult.New()
                .AddValidationError(nameof(InitialSale), ValidationErrorMessage.MustBeProvidedForCalculation(DisplayName))
                .CheckErrors();
        }

        if (!int.TryParse(value!, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue))
        {
            OperationResult.New()
                .AddValidationError(nameof(InitialSale), ValidationErrorMessage.MustBeNumberBetween(DisplayName, MinValue, MaxValue))
                .CheckErrors();
        }

        if (parsedValue is < MinValue or > MaxValue)
        {
            OperationResult.New()
                .AddValidationError(nameof(InitialSale), ValidationErrorMessage.MustBeNumberBetween(DisplayName, MinValue, MaxValue))
                .CheckErrors();
        }

        Value = parsedValue;
    }

    public InitialSale(int value)
    {
        if (value is < MinValue or > MaxValue)
        {
            OperationResult.New()
                .AddValidationError(nameof(InitialSale), ValidationErrorMessage.MustBeNumberBetween(DisplayName, MinValue, MaxValue))
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
