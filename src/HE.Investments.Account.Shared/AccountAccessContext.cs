using HE.Investments.Account.Shared.User;

namespace HE.Investments.Account.Shared;

public class AccountAccessContext : IAccountAccessContext
{
    public const string OrganisationView = $"{UserAccountRole.AdminRole},{UserAccountRole.EnhancedRole},{UserAccountRole.InputRole},{UserAccountRole.ViewOnlyRole}";

    public const string ManageUsers = $"{UserAccountRole.AdminRole}";

    public const string EditApplications = $"{UserAccountRole.AdminRole},{UserAccountRole.EnhancedRole},{UserAccountRole.InputRole},{UserAccountRole.LimitedRole}";

    public const string SubmitApplication = $"{UserAccountRole.AdminRole},{UserAccountRole.EnhancedRole},{UserAccountRole.LimitedRole}";

    private readonly IAccountUserContext _accountUserContext;

    public AccountAccessContext(IAccountUserContext accountUserContext)
    {
        _accountUserContext = accountUserContext;
    }

    public async Task<bool> CanManageUsers()
    {
        return await _accountUserContext.HasOneOfRole(ToUserAccountRoles(ManageUsers));
    }

    public async Task<bool> CanAccessOrganisationView()
    {
        return await _accountUserContext.HasOneOfRole(ToUserAccountRoles(OrganisationView));
    }

    public async Task<bool> CanSubmitApplication()
    {
        return await _accountUserContext.HasOneOfRole(ToUserAccountRoles(SubmitApplication));
    }

    public async Task<bool> CanEditApplication()
    {
        return await _accountUserContext.HasOneOfRole(ToUserAccountRoles(EditApplications));
    }

    private static UserAccountRole[] ToUserAccountRoles(string roles)
    {
        return roles.Split(',').Select(x => new UserAccountRole(x)).ToArray();
    }
}
