namespace HE.InvestmentLoans.BusinessLogic.User.Entities;

public record UserAccountRole(string Role)
{
    public const string LimitedUser = "Limited user";

    public const string Admin = "Admin";

    public static UserAccountRole AnLimitedUser() => new(LimitedUser);

    public static UserAccountRole AnAdmin() => new(Admin);
}
