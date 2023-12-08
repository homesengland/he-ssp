using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investments.Account.Domain.Users.ValueObjects;

public class UserId : StringIdValueObject
{
    public UserId(string id)
        : base(id)
    {
    }
}
