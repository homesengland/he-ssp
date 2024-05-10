using HE.Investments.Account.Api.Contract.User;

namespace HE.Investment.AHP.Domain.UserContext;

public static class AhpAccessContext
{
    public const string ViewConsortium = $"{nameof(UserRole.Admin)},{nameof(UserRole.Enhanced)},{nameof(UserRole.Input)},{nameof(UserRole.ViewOnly)}";

    public const string ManageConsortium = $"{nameof(UserRole.Admin)},{nameof(UserRole.Enhanced)}";

    public static readonly IReadOnlyCollection<UserRole> ManageConsortiumRoles = ToUserAccountRoles(ManageConsortium);

    public static bool CanManageConsortium(IEnumerable<UserRole> roles)
    {
        return roles.Any(x => ManageConsortiumRoles.Contains(x));
    }

    private static UserRole[] ToUserAccountRoles(string roles)
    {
        return roles.Split(',').Select(x => (UserRole)Enum.Parse(typeof(UserRole), x)).ToArray();
    }
}
