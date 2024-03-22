namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class RentPerWeek : DecimalValueObject
{
    private const string DisplayName = "per week";

    private const decimal MinValue = 0;

    private const decimal MaxValue = 9999.99M;

    public RentPerWeek(string? value, bool isCalculation, string? rentType = null)
        : base(value, nameof(RentPerWeek), string.Join(" ", rentType ?? "rent", DisplayName), isCalculation, MinValue, MaxValue)
    {
    }

    public RentPerWeek(decimal value)
        : base(value, nameof(RentPerWeek), DisplayName, MinValue, MaxValue)
    {
    }
}
