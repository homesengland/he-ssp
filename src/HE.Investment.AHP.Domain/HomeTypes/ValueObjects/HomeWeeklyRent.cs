namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class HomeWeeklyRent : DecimalValueObject
{
    private const string DisplayName = "market rent";

    private const decimal MinValue = 0;

    private const decimal MaxValue = 9999.99M;

    public HomeWeeklyRent(string? value, bool isCalculation)
        : base(value, nameof(HomeWeeklyRent), DisplayName, isCalculation, MinValue, MaxValue)
    {
    }

    public HomeWeeklyRent(decimal value)
        : base(value, nameof(HomeWeeklyRent), DisplayName, MinValue, MaxValue)
    {
    }
}
