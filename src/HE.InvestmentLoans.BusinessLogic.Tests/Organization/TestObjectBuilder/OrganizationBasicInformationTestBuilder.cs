using HE.InvestmentLoans.BusinessLogic.Tests.Organization.TestData;
using HE.InvestmentLoans.Contract.Organization.ValueObjects;
using HE.Investments.Account.Contract.Organisation.Queries;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Organization.TestObjectBuilder;
public class OrganizationBasicInformationTestBuilder
{
    private readonly OrganizationBasicInformation _item;

    private OrganizationBasicInformationTestBuilder(OrganizationBasicInformation organizationBasicInformation)
    {
        _item = organizationBasicInformation;
    }

    public static OrganizationBasicInformationTestBuilder New() =>
        new(new OrganizationBasicInformation(
            OrganizationBasicInformationTestData.OrganizationBasicInformationOne.RegisteredCompanyName,
            OrganizationBasicInformationTestData.OrganizationBasicInformationOne.CompanyRegistrationNumber,
            OrganizationBasicInformationTestData.OrganizationBasicInformationOne.Address,
            OrganizationBasicInformationTestData.OrganizationBasicInformationOne.ContactInformation));

    public OrganizationBasicInformation Build()
    {
        return _item;
    }
}
