namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class MarketRentPerWeek : DecimalValueObject
{
    private const string DisplayName = "market rent per week";

    private const decimal MinValue = 0;

    private const decimal MaxValue = 9999.99M;

    public MarketRentPerWeek(string? value, bool isCalculation)
        : base(value, nameof(MarketRentPerWeek), DisplayName, isCalculation, MinValue, MaxValue)
    {
    }

    public MarketRentPerWeek(decimal value)
        : base(value, nameof(MarketRentPerWeek), DisplayName, MinValue, MaxValue)
    {
    }
}
