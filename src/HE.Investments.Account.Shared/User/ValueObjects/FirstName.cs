using HE.Investments.Common.Messages;

namespace HE.Investments.Account.Shared.User.ValueObjects;

public class FirstName : RequiredStringValueObject
{
    public FirstName(string? value)
        : base(value, nameof(FirstName), "your first name", MaximumInputLength.ShortInput)
    {
    }
}
