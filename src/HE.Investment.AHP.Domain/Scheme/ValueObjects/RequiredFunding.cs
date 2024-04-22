using HE.Investments.Common.Domain.ValueObjects;
using HE.Investments.Common.Validators;

namespace HE.Investment.AHP.Domain.Scheme.ValueObjects;

public class RequiredFunding : TheRequiredIntValueObject
{
    private const string DisplayName = "total of funding you are requesting";

    private const int MinValue = 1;

    private const int MaxValue = 999999999;

    public RequiredFunding(string? value)
        : base(value, nameof(RequiredFunding), DisplayName, MinValue, MaxValue, MessageOptions.HideExample | MessageOptions.Money)
    {
    }

    public RequiredFunding(int value)
        : base(value, nameof(RequiredFunding), DisplayName, MinValue, MaxValue)
    {
    }
}
