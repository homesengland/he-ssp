namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class AffordableWeeklyRent : DecimalValueObject
{
    private const string DisplayName = "prospective rent per week";

    private const decimal MinValue = 0;

    private const decimal MaxValue = 9999.99M;

    public AffordableWeeklyRent(string? value, bool isCalculation)
        : base(value, nameof(AffordableWeeklyRent), DisplayName, isCalculation, MinValue, MaxValue)
    {
    }

    public AffordableWeeklyRent(decimal value)
        : base(value, nameof(AffordableWeeklyRent), DisplayName, MinValue, MaxValue)
    {
    }
}
