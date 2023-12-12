namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class ProspectiveRent : DecimalValueObject
{
    private const string DisplayName = "prospective rent per week";

    private const decimal MinValue = 0;

    private const decimal MaxValue = 9999.99M;

    public ProspectiveRent(string? value, bool isCalculation)
        : base(value, nameof(ProspectiveRent), DisplayName, isCalculation, MinValue, MaxValue)
    {
    }

    public ProspectiveRent(decimal value)
        : base(value, nameof(ProspectiveRent), DisplayName, MinValue, MaxValue)
    {
    }
}
