using HE.Investments.Account.Shared.User;

namespace HE.Investments.Account.Domain.User;

public interface IAccountUserContext
{
    public UserGlobalId UserGlobalId { get; }

    public string Email { get; }

    public void RefreshUserData();
}
