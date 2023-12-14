using HE.Investments.Common.Messages;

namespace HE.Investments.Account.Shared.User.ValueObjects;

public class LastName : RequiredStringValueObject
{
    public LastName(string? value, string displayName = "your last name")
        : base(value, nameof(LastName), displayName, MaximumInputLength.ShortInput)
    {
    }
}
