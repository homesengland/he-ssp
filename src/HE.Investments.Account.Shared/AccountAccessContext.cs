using HE.Investments.Account.Api.Contract.User;

namespace HE.Investments.Account.Shared;

public class AccountAccessContext : IAccountAccessContext
{
    public const string OrganisationView = $"{nameof(UserRole.Admin)},{nameof(UserRole.Enhanced)},{nameof(UserRole.Input)},{nameof(UserRole.ViewOnly)}";

    public const string ManageUsers = $"{nameof(UserRole.Admin)}";

    public const string EditApplications = $"{nameof(UserRole.Admin)},{nameof(UserRole.Enhanced)},{nameof(UserRole.Input)},{nameof(UserRole.Limited)}";

    public const string SubmitApplication = $"{nameof(UserRole.Admin)},{nameof(UserRole.Enhanced)},{nameof(UserRole.Limited)}";

    public static readonly IReadOnlyCollection<UserRole> OrganisationViewRoles = ToUserAccountRoles(OrganisationView);

    public static readonly IReadOnlyCollection<UserRole> ManageUsersRoles = ToUserAccountRoles(ManageUsers);

    public static readonly IReadOnlyCollection<UserRole> EditApplicationRoles = ToUserAccountRoles(EditApplications);

    public static readonly IReadOnlyCollection<UserRole> SubmitApplicationRoles = ToUserAccountRoles(SubmitApplication);

    private readonly IAccountUserContext _accountUserContext;

    public AccountAccessContext(IAccountUserContext accountUserContext)
    {
        _accountUserContext = accountUserContext;
    }

    public async Task<bool> CanManageUsers()
    {
        var account = await _accountUserContext.GetSelectedAccount();
        return account.CanManageUsers;
    }

    public async Task<bool> CanAccessOrganisationView()
    {
        var account = await _accountUserContext.GetSelectedAccount();
        return account.CanAccessOrganisationView;
    }

    public async Task<bool> CanSubmitApplication()
    {
        var account = await _accountUserContext.GetSelectedAccount();
        return account.CanSubmitApplication;
    }

    public async Task<bool> CanEditApplication()
    {
        var account = await _accountUserContext.GetSelectedAccount();
        return account.CanEditApplication;
    }

    private static UserRole[] ToUserAccountRoles(string roles)
    {
        return roles.Split(',').Select(x => (UserRole)Enum.Parse(typeof(UserRole), x)).ToArray();
    }
}
