using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class MarketRentPerWeek : TheRequiredDecimalValueObject
{
    private const string DisplayName = "market rent per week";

    private const decimal MinValue = 0;

    private const decimal MaxValue = 9999.99M;

    public MarketRentPerWeek(string? value, bool isCalculation)
        : base(
            value,
            nameof(MarketRentPerWeek),
            DisplayName,
            MinValue,
            MaxValue,
            precision: 2,
            MessageOptions.IsMoney.WithCalculation(isCalculation))
    {
    }

    public MarketRentPerWeek(decimal value)
        : base(value, nameof(MarketRentPerWeek), DisplayName, MinValue, MaxValue)
    {
    }
}
