using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain.ValueObjects;

namespace HE.Investments.Account.Domain.Users.ValueObjects;

public record UserId : StringIdValueObject
{
    public UserId(string id)
        : base(id)
    {
    }
}
