using HE.InvestmentLoans.Contract.Organization.ValueObjects;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Organization.TestData;
public static class OrganizationBasicInformationTestData
{
    public static readonly OrganizationBasicInformation OrganizationBasicInformationOne = new(
        "Test company",
        "112233",
        new("Aleje Jerozolimskie", "100", "12", "Warsaw", "123456", "Poland"));
}
