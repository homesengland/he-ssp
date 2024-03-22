using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investments.Account.Shared.User.ValueObjects;

public class YourFirstName : YourShortText
{
    public YourFirstName(string? value)
        : base(value, "FirstName", "first name")
    {
    }
}
