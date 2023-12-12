namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class MarketRent : DecimalValueObject
{
    private const string DisplayName = "market rent";

    private const decimal MinValue = 0;

    private const decimal MaxValue = 9999.99M;

    public MarketRent(string? value, bool isCalculation)
        : base(value, nameof(MarketRent), DisplayName, isCalculation, MinValue, MaxValue)
    {
    }

    public MarketRent(decimal value)
        : base(value, nameof(MarketRent), DisplayName, MinValue, MaxValue)
    {
    }
}
