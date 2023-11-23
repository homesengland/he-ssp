namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class NumberOfStoreys : RequiredIntValueObject
{
    private const string DisplayName = "How many storeys in this home type";

    private const int MinValue = 0;

    private const int MaxValue = 999;

    public NumberOfStoreys(string? value)
        : base(value, nameof(NumberOfStoreys), DisplayName, MinValue, MaxValue)
    {
    }

    public NumberOfStoreys(int value)
        : base(value, nameof(NumberOfStoreys), DisplayName, MinValue, MaxValue)
    {
    }
}
