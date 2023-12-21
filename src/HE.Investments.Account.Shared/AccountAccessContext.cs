using HE.Investments.Account.Contract.Users;

namespace HE.Investments.Account.Shared;

public class AccountAccessContext : IAccountAccessContext
{
    public const string OrganisationView = $"{nameof(UserRole.Admin)},{nameof(UserRole.Enhanced)},{nameof(UserRole.Input)},{nameof(UserRole.ViewOnly)}";

    public const string ManageUsers = $"{nameof(UserRole.Admin)}";

    public const string EditApplications = $"{nameof(UserRole.Admin)},{nameof(UserRole.Enhanced)},{nameof(UserRole.Input)},{nameof(UserRole.Limited)}";

    public const string SubmitApplication = $"{nameof(UserRole.Admin)},{nameof(UserRole.Enhanced)},{nameof(UserRole.Limited)}";

    private readonly IAccountUserContext _accountUserContext;

    public AccountAccessContext(IAccountUserContext accountUserContext)
    {
        _accountUserContext = accountUserContext;
    }

    public async Task<bool> CanManageUsers()
    {
        var account = await _accountUserContext.GetSelectedAccount();
        return account.HasOneOfRole(ToUserAccountRoles(ManageUsers));
    }

    public async Task<bool> CanAccessOrganisationView()
    {
        var account = await _accountUserContext.GetSelectedAccount();
        return account.HasOneOfRole(ToUserAccountRoles(OrganisationView));
    }

    public async Task<bool> CanSubmitApplication()
    {
        var account = await _accountUserContext.GetSelectedAccount();
        return account.HasOneOfRole(ToUserAccountRoles(SubmitApplication));
    }

    public async Task<bool> CanEditApplication()
    {
        var account = await _accountUserContext.GetSelectedAccount();
        return account.HasOneOfRole(ToUserAccountRoles(EditApplications));
    }

    private static UserRole[] ToUserAccountRoles(string roles)
    {
        return roles.Split(',').Select(x => (UserRole)Enum.Parse(typeof(UserRole), x)).ToArray();
    }
}
