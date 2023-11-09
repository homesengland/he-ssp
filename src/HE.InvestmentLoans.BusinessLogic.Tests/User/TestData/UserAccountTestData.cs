using HE.InvestmentLoans.Common.Tests.TestData;
using HE.Investments.Account.Shared.User;

namespace HE.InvestmentLoans.BusinessLogic.Tests.User.TestData;

public static class UserAccountTestData
{
    public static readonly UserAccount UserAccountOne = new(UserGlobalId.From("UserOne"), "User@one.com", GuidTestData.GuidTwo, "AccountOne", new[] { UserAccountRole.AnLimitedUser() });
    public static readonly UserAccount AdminUserAccountOne = new(UserGlobalId.From("UserOne"), "User@one.com", GuidTestData.GuidTwo, "AccountOne", new[] { UserAccountRole.AnLimitedUser(), UserAccountRole.AnAdmin() });
}
