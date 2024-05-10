using HE.Investments.Account.Api.Contract.User;

namespace HE.Investment.AHP.Domain.UserContext;

public class AhpAccessContext : IAhpAccessContext
{
    public const string ViewConsortium = $"{nameof(UserRole.Admin)},{nameof(UserRole.Enhanced)},{nameof(UserRole.Input)},{nameof(UserRole.ViewOnly)}";

    public const string ManageConsortium = $"{nameof(UserRole.Admin)},{nameof(UserRole.Enhanced)}";

    public static readonly IReadOnlyCollection<UserRole> ManageConsortiumRoles = ToUserAccountRoles(ManageConsortium);

    private readonly IAhpUserContext _ahpUserContext;

    public AhpAccessContext(IAhpUserContext ahpUserContext)
    {
        _ahpUserContext = ahpUserContext;
    }

    public async Task<bool> CanManageConsortium()
    {
        var account = await _ahpUserContext.GetSelectedAccount();
        return account.CanManageConsortium;
    }

    private static UserRole[] ToUserAccountRoles(string roles)
    {
        return roles.Split(',').Select(x => (UserRole)Enum.Parse(typeof(UserRole), x)).ToArray();
    }
}
