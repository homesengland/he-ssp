using HE.Investments.Account.Domain.User.ValueObjects;

namespace HE.Investments.Account.Domain.User;

public interface IAccountUserContext
{
    public UserGlobalId UserGlobalId { get; }

    public string Email { get; }

    public void RefreshUserData();
}
