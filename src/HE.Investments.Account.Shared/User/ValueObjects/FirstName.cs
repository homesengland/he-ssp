using HE.Investments.Common.Messages;

namespace HE.Investments.Account.Shared.User.ValueObjects;

public class FirstName : RequiredStringValueObject
{
    public FirstName(string? value, string displayName = "your first name")
        : base(value, nameof(FirstName), displayName, MaximumInputLength.ShortInput)
    {
    }
}
