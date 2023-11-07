using HE.InvestmentLoans.Contract.Organization.ValueObjects;
using HE.Investments.Account.Contract.Organisation.Queries;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Organization.TestData;
public static class OrganizationBasicInformationTestData
{
    public static readonly OrganizationBasicInformation OrganizationBasicInformationOne = new(
        "Test company",
        "112233",
        new("Aleje Jerozolimskie", "100", "12", "Warsaw", "123456", "Poland"),
        new("858993004", "example@email.com"));
}
