using HE.Investments.Account.Contract.Users;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.User;
using HE.Investments.Account.Shared.User.ValueObjects;
using HE.Investments.Common.Contract;
using HE.Investments.Loans.Common.Tests.TestData;

namespace HE.Investments.Loans.BusinessLogic.Tests.User.TestData;

public static class UserAccountTestData
{
    public static readonly UserAccount UserAccountOne = new(UserGlobalId.From("UserOne"), "User@one.com", new OrganisationBasicInfo(new OrganisationId(GuidTestData.GuidTwo), false), "AccountOne", new[] { UserRole.Limited });
    public static readonly UserAccount AdminUserAccountOne = new(UserGlobalId.From("UserOne"), "User@one.com", new OrganisationBasicInfo(new OrganisationId(GuidTestData.GuidTwo), false), "AccountOne", new[] { UserRole.Limited, UserRole.Admin });
}
