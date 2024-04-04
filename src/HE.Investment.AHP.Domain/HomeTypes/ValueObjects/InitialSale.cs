using System.Globalization;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;

namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class InitialSale : ValueObject
{
    private const string DisplayName = "assumed average first tranche sale";

    private const decimal MinValue = 0.1m;

    private const decimal MaxValue = 0.75m;

    public InitialSale(string? value, bool isCalculation = false)
    {
        if (value.IsNotProvided() && !isCalculation)
        {
            OperationResult.New()
                .AddValidationError(nameof(InitialSale), ValidationErrorMessage.MustProvideRequiredField(DisplayName))
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
                .AddValidationError(nameof(InitialSale), ValidationErrorMessage.MustBeWholeNumberBetween(
                    DisplayName,
                    MinValue.ToPercentage100(),
                    MaxValue.ToPercentage100()))
                .CheckErrors();
        }

        var percentage = parsedValue / 100m;

        if (percentage is < MinValue or > MaxValue)
        {
            OperationResult.New()
                .AddValidationError(nameof(InitialSale), ValidationErrorMessage.MustBeWholeNumberBetween(
                    DisplayName,
                    MinValue.ToPercentage100(),
                    MaxValue.ToPercentage100()))
                .CheckErrors();
        }

        Value = percentage;
    }

    public InitialSale(decimal value)
    {
        if (value is < MinValue or > MaxValue)
        {
            OperationResult.New()
                .AddValidationError(nameof(InitialSale), ValidationErrorMessage.MustBeWholeNumberBetween(
                    DisplayName,
                    MinValue.ToPercentage100(),
                    MaxValue.ToPercentage100()))
                .CheckErrors();
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
}
