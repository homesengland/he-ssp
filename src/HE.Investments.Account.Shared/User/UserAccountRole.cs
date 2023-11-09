namespace HE.Investments.Account.Shared.User;

public record UserAccountRole(string Role)
{
    public const string LimitedUser = "Limited user";

    public const string Admin = "Account administrator";

    public static UserAccountRole AnLimitedUser() => new(LimitedUser);

    public static UserAccountRole AnAdmin() => new(Admin);
}
