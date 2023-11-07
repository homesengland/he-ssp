using HE.Investments.Account.Domain.User.ValueObjects;
using HE.Investments.Account.Shared.User;

namespace HE.Investments.Account.Domain.User;

public class AccountUserContext : IAccountUserContext
{
    public UserGlobalId UserGlobalId { get; }

    public string Email { get; }

    public void RefreshUserData()
    {
        throw new NotImplementedException();
    }
}
