using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investments.Account.Shared.User.ValueObjects;

public class FirstName : ShortText
{
    public FirstName(string? value)
        : base(value, "FirstName", "first name")
    {
    }
}
