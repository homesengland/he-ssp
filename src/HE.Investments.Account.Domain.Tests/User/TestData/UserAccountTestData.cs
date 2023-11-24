using HE.Investments.Account.Domain.Organisation.Entities;
using HE.Investments.Account.Shared.User;
using HE.Investments.Loans.Common.Tests.TestData;

namespace HE.Investments.Account.Domain.Tests.User.TestData;

public static class UserAccountTestData
{
    public static readonly UserAccount UserAccountOne = new(UserGlobalId.From("UserOne"), "User@one.com", GuidTestData.GuidTwo, "AccountOne", new[] { UserAccountRole.AnLimitedUser() });
    public static readonly UserAccount AdminUserAccountOne = new(UserGlobalId.From("UserOne"), "User@one.com", GuidTestData.GuidTwo, "AccountOne", new[] { UserAccountRole.AnLimitedUser(), UserAccountRole.AnAdmin() });
}
