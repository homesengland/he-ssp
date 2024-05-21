using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.Entities;
using HE.Investments.Common.Contract;

namespace HE.Investments.Account.Shared;

public interface IUserAccountContext<TUserAccount>
    where TUserAccount : UserAccount
{
    bool IsLogged { get; }

    UserGlobalId UserGlobalId { get; }

    string Email { get; }

    Task<TUserAccount> GetSelectedAccount();

    Task<UserProfileDetails> GetProfileDetails();

    Task RefreshUserData();

    Task<bool> IsProfileCompleted();

    Task<bool> IsLinkedWithOrganisation();
}
