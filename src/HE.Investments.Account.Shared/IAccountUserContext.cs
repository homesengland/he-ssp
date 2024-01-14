using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.Entities;
using HE.Investments.Common.Contract;

namespace HE.Investments.Account.Shared;

public interface IAccountUserContext
{
    bool IsLogged { get; }

    UserGlobalId UserGlobalId { get; }

    string Email { get; }

    Task<UserAccount> GetSelectedAccount();

    Task<UserProfileDetails> GetProfileDetails();

    Task RefreshUserData();

    Task<bool> IsProfileCompleted();

    Task<bool> IsLinkedWithOrganisation();
}
