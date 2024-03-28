using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investments.Account.Shared.User.ValueObjects;

public class YourLastName : YourShortText
{
    public YourLastName(string? value)
        : base(value, "LastName", "last name")
    {
    }
}
