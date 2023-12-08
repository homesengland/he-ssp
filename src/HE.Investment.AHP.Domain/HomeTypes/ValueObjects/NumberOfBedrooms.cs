namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class NumberOfBedrooms : RequiredIntValueObject
{
    private const string DisplayName = "number of bedrooms in each home";

    private const int MinValue = 0;

    private const int MaxValue = 999;

    public NumberOfBedrooms(string? value)
        : base(value, nameof(NumberOfBedrooms), DisplayName, MinValue, MaxValue)
    {
    }

    public NumberOfBedrooms(int value)
        : base(value, nameof(NumberOfBedrooms), DisplayName, MinValue, MaxValue)
    {
    }
}
