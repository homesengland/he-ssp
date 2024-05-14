using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Account.Shared;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Tests.TestData;
using UserAccount = HE.Investments.Account.Shared.User.UserAccount;

namespace HE.Investments.Loans.BusinessLogic.Tests.TestData;

public static class UserAccountTestData
{
    public static readonly UserAccount UserAccountOne = new(
        UserGlobalId.From("UserOne"),
        "User@one.com",
        new OrganisationBasicInfo(new OrganisationId(GuidTestData.GuidTwo.ToString()), "AccountOne", "4321", "London", false),
        [UserRole.Limited]);
}
