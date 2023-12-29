using HE.Investments.Account.Contract.Users;
using HE.Investments.Account.Domain.Tests.TestData;
using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.ValueObjects;

namespace HE.Investments.Account.Domain.Tests.User.TestData;

public static class UserAccountTestData
{
    public static readonly UserAccount UserAccountOne = new(
        UserGlobalId.From("UserOne"),
        "User@one.com",
        new OrganisationId(GuidTestData.GuidTwo),
        "AccountOne",
        new[] { UserRole.Limited });

    public static readonly UserAccount AdminUserAccountOne = new(
        UserGlobalId.From("UserOne"),
        "User@one.com",
        new OrganisationId(GuidTestData.GuidTwo),
        "AccountOne",
        new[] { UserRole.Admin, UserRole.Limited });
}
