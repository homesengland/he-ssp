using System.Globalization;
using HE.Investments.Common.Domain;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class HomeMarketValue : ValueObject
{
    private const string DisplayName = "market value of each property";

    private const int MinValue = 0;

    private const int MaxValue = 99999999;

    public HomeMarketValue(string? value, bool isCalculation)
    {
        if (value.IsNotProvided() && !isCalculation)
        {
            return;
        }

        if (value.IsNotProvided() && isCalculation)
        {
            OperationResult.New()
                .AddValidationError(nameof(HomeMarketValue), ValidationErrorMessage.MustBeProvidedForCalculation(DisplayName))
                .CheckErrors();
        }

        if (!int.TryParse(value!, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue))
        {
            OperationResult.New()
                .AddValidationError(nameof(HomeMarketValue), ValidationErrorMessage.MustBeNumber(DisplayName))
                .CheckErrors();
        }

        if (parsedValue < MinValue || parsedValue > MaxValue)
        {
            OperationResult.New()
                .AddValidationError(nameof(HomeMarketValue), ValidationErrorMessage.MustBeNumberBetween(DisplayName, MinValue, MaxValue))
                .CheckErrors();
        }

        Value = parsedValue;
    }

    public HomeMarketValue(int value)
    {
        if (value < MinValue || value > MaxValue)
        {
            OperationResult.New()
                .AddValidationError(nameof(HomeMarketValue), ValidationErrorMessage.MustBeNumberBetween(DisplayName, MinValue, MaxValue))
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
