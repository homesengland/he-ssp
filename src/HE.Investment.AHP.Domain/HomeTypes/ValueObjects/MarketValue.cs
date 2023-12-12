namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class MarketValue : RequiredIntForCalculationValueObject
{
    private const string DisplayName = "market value of each property";

    private const int MinValue = 0;

    private const int MaxValue = 99999999;

    public MarketValue(string? value, bool isCalculation)
        : base(value, nameof(MarketValue), DisplayName, MinValue, MaxValue, isCalculation)
    {
    }

    public MarketValue(int value)
        : base(value, nameof(MarketValue), DisplayName, MinValue, MaxValue)
    {
    }
}
