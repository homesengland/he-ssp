using HE.Investments.Account.Api.Contract.User;

namespace HE.Investments.Consortium.Shared.UserContext;

public class ConsortiumAccessContext : IConsortiumAccessContext
{
    public const string Edit = $"{nameof(UserRole.Admin)},{nameof(UserRole.Enhanced)},{nameof(UserRole.Input)},{nameof(UserRole.Limited)}";

    public const string Submit = $"{nameof(UserRole.Admin)},{nameof(UserRole.Enhanced)},{nameof(UserRole.Limited)}";

    public const string ViewConsortium = $"{nameof(UserRole.Admin)},{nameof(UserRole.Enhanced)},{nameof(UserRole.Input)},{nameof(UserRole.ViewOnly)}";

    public const string ManageConsortium = $"{nameof(UserRole.Admin)},{nameof(UserRole.Enhanced)}";

    public static readonly IReadOnlyCollection<UserRole> ManageConsortiumRoles = ToUserAccountRoles(ManageConsortium);

    private readonly IConsortiumUserContext _consortiumUserContext;

    public ConsortiumAccessContext(IConsortiumUserContext consortiumUserContext)
    {
        _consortiumUserContext = consortiumUserContext;
    }

    public async Task<bool> CanManageConsortium()
    {
        var account = await _consortiumUserContext.GetSelectedAccount();
        return account is { CanManageConsortium: true };
    }

    public async Task<bool> IsConsortiumLeadPartner()
    {
        var account = await _consortiumUserContext.GetSelectedAccount();
        return account is { Consortium.IsLeadPartner: true };
    }

    public async Task<bool> CanEditApplication()
    {
        var account = await _consortiumUserContext.GetSelectedAccount();
        return account.CanEditApplication && (account.Consortium.IsLeadPartner || account.Consortium.HasNoConsortium);
    }

    private static UserRole[] ToUserAccountRoles(string roles)
    {
        return roles.Split(',').Select(x => (UserRole)Enum.Parse(typeof(UserRole), x)).ToArray();
    }
}
