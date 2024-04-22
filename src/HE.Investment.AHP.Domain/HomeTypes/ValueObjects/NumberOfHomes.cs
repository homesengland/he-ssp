using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class NumberOfHomes : TheRequiredIntValueObject
{
    private const string DisplayName = "number of homes you are delivering";

    private const int MinValue = 0;

    private const int MaxValue = 999;

    public NumberOfHomes(string? value)
        : base(value, nameof(NumberOfHomes), DisplayName, MinValue, MaxValue)
    {
    }

    public NumberOfHomes(int value)
        : base(value, nameof(NumberOfHomes), DisplayName, MinValue, MaxValue)
    {
    }
}
