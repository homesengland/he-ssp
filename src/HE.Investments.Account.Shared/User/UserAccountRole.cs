namespace HE.Investments.Account.Shared.User;

public record UserAccountRole(string Role)
{
    public const string AdminRole = "Admin";

    public const string EnhancedRole = "Enhanced";

    public const string InputRole = "Input only";

    public const string ViewOnlyRole = "View only";

    public const string LimitedRole = "Limited user";

    public const string AccessOrganisationRoles = $"{AdminRole},{EnhancedRole},{InputRole},{ViewOnlyRole}";

    public static UserAccountRole AnLimitedUser() => new(LimitedRole);

    public static UserAccountRole AnAdmin() => new(AdminRole);

    public static UserAccountRole AnEnhancedUser() => new(EnhancedRole);

    public static UserAccountRole AnInputOnlyUser() => new(InputRole);

    public static UserAccountRole AViewOnlyUser() => new(ViewOnlyRole);

    public static UserAccountRole[] AccessOrganisation() => new[] { AnAdmin(), AnEnhancedUser(), AnInputOnlyUser(), AViewOnlyUser() };

}
