using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.Entities;

namespace HE.Investments.Account.Shared;

public interface IAccountUserContext
{
    public UserGlobalId UserGlobalId { get; }

    public string Email { get; }

    Task<UserAccount> GetSelectedAccount();

    Task<UserProfileDetails> GetProfileDetails();

    public Task RefreshProfileDetails();

    public Task<bool> IsProfileCompleted();

    Task<bool> IsLinkedWithOrganization();
}
