using HE.InvestmentLoans.BusinessLogic.User.Entities;
using HE.InvestmentLoans.Common.Tests.TestData;
using HE.InvestmentLoans.Contract.User;
using HE.InvestmentLoans.Contract.User.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Tests.User.TestData;

public static class UserAccountTestData
{
    public static readonly UserAccount UserAccountOne = new(UserGlobalId.From("UserOne"), "User@one.com", GuidTestData.GuidTwo, "AccountOne", new[] { UserAccountRole.AnLimitedUser() });
}
