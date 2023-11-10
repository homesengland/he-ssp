using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.Entities;

namespace HE.Investments.Account.Shared;

public interface IAccountUserContext
{
    UserGlobalId UserGlobalId { get; }

    string Email { get; }

    Task<UserAccount> GetSelectedAccount();

    Task<UserProfileDetails> GetProfileDetails();

    Task RefreshProfileDetails();

    Task RefreshAccounts();

    Task<bool> IsProfileCompleted();

    Task<bool> IsLinkedWithOrganization();
}
