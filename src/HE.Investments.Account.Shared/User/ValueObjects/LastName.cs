using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investments.Account.Shared.User.ValueObjects;

public class LastName : ShortText
{
    public LastName(string? value)
        : base(value, nameof(LastName), "last name")
    {
    }
}
