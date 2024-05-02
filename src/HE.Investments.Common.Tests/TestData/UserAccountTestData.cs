using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using OrganisationId = HE.Investments.Account.Shared.User.ValueObjects.OrganisationId;
using UserAccount = HE.Investments.Account.Shared.User.UserAccount;

namespace HE.Investments.Common.Tests.TestData;

public static class UserAccountTestData
{
    public static readonly UserAccount UserAccountOne = new(
        UserGlobalId.From("UserOne"),
        "User@one.com",
        new OrganisationBasicInfo(new OrganisationId(GuidTestData.GuidTwo), "AccountOne", "1234", "London", false),
        [UserRole.Limited]);

    public static readonly UserAccount AdminUserAccountOne = new(
        UserGlobalId.From("UserOne"),
        "User@one.com",
        new OrganisationBasicInfo(new OrganisationId(GuidTestData.GuidTwo), "AccountOne", "1234", "London", false),
        [UserRole.Admin, UserRole.Limited]);
}
