using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class NumberOfStoreys : TheRequiredIntValueObject
{
    private const string DisplayName = "number of storeys";

    private const int MinValue = 0;

    private const int MaxValue = 99;

    public NumberOfStoreys(string? value)
        : base(value, nameof(NumberOfStoreys), DisplayName, MinValue, MaxValue)
    {
    }

    public NumberOfStoreys(int value)
        : base(value, nameof(NumberOfStoreys), DisplayName, MinValue, MaxValue)
    {
    }
}
