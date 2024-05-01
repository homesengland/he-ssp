using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using UserAccount = HE.Investments.Account.Shared.User.UserAccount;

namespace HE.Investments.Common.Tests.TestData;

public static class UserAccountTestData
{
    public static readonly UserAccount UserAccountOne = new(
        UserGlobalId.From("UserOne"),
        "User@one.com",
        new OrganisationBasicInfo(new OrganisationId(GuidTestData.GuidTwo.ToString()), "AccountOne", "1234", "London", false),
        new[] { UserRole.Limited });

    public static readonly UserAccount AdminUserAccountOne = new(
        UserGlobalId.From("UserOne"),
        "User@one.com",
        new OrganisationBasicInfo(new OrganisationId(GuidTestData.GuidTwo.ToString()), "AccountOne", "1234", "London", false),
        new[] { UserRole.Admin, UserRole.Limited });
}
