using HE.Investments.Account.Contract.Users;
using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.Entities;

namespace HE.Investments.Account.Shared;

public interface IAccountUserContext
{
    bool IsLogged { get; }

    UserGlobalId UserGlobalId { get; }

    string Email { get; }

    Task<UserAccount> GetSelectedAccount();

    Task<UserProfileDetails> GetProfileDetails();

    Task RefreshProfileDetails();

    Task RefreshAccounts();

    Task<bool> IsProfileCompleted();

    Task<bool> IsLinkedWithOrganisation();

    Task<bool> HasOneOfRole(UserRole[] roles);
}
