using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investment.AHP.Domain.HomeTypes.ValueObjects;

public class MaximumOccupancy : TheRequiredIntValueObject
{
    private const string DisplayName = "maximum occupancy of each home";

    private const int MinValue = 0;

    private const int MaxValue = 999;

    public MaximumOccupancy(string? value)
        : base(value, nameof(MaximumOccupancy), DisplayName, MinValue, MaxValue)
    {
    }

    public MaximumOccupancy(int value)
        : base(value, nameof(MaximumOccupancy), DisplayName, MinValue, MaxValue)
    {
    }
}
