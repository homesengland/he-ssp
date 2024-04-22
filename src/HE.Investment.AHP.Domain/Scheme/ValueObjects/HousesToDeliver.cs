using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.Scheme.ValueObjects;

public class HousesToDeliver : TheRequiredIntValueObject
{
    private const string DisplayName = "number of homes this scheme will deliver";

    private const int MinValue = 1;

    private const int MaxValue = 999999;

    public HousesToDeliver(string? value)
        : base(value, nameof(HousesToDeliver), DisplayName, MinValue, MaxValue, MessageOptions.HideExample)
    {
    }

    public HousesToDeliver(int value)
        : base(value, nameof(RequiredFunding), DisplayName, MinValue, MaxValue)
    {
    }
}
