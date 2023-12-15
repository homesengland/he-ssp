using HE.Investments.Account.Shared.User;

namespace HE.Investments.Account.Shared;

public class AccountAccessContext : IAccountAccessContext
{
    public const string OrganisationView = $"{UserAccountRole.AdminRole},{UserAccountRole.EnhancedRole},{UserAccountRole.InputRole},{UserAccountRole.ViewOnlyRole}";

    public const string ManageUsers = $"{UserAccountRole.AdminRole}";

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

    private static UserAccountRole[] ToUserAccountRoles(string roles)
    {
        return roles.Split(',').Select(x => new UserAccountRole(x)).ToArray();
    }
}
