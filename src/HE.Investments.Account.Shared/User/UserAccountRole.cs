namespace HE.Investments.Account.Shared.User;

public record UserAccountRole(string Role)
{
    public const string AdminRole = "Admin";

    public const string EnhancedRole = "Enhanced";

    public const string InputRole = "Input only";

    public const string ViewOnlyRole = "View only";

    public const string LimitedRole = "Limited user";

    public static UserAccountRole AnLimitedUser() => new(LimitedRole);

    public static UserAccountRole AnAdmin() => new(AdminRole);
}
