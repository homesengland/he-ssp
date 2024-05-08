using HE.Investments.Account.Api.Contract.User;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.User;
using HE.Investments.Common.Contract;

namespace HE.Investments.AHP.Consortium.Domain.Tests.TestData;

public static class UserAccountInvestmentsOrganisationTestData
{
    public static readonly UserAccount JjCompanyEmployee = new(
        UserGlobalId.From("MasterJ"),
        "User@company.com",
        new OrganisationBasicInfo(InvestmentsOrganisationTestData.JjCompany.Id, "AccountOne", "1234", "London", false),
        [UserRole.Admin, UserRole.Limited]);
}
