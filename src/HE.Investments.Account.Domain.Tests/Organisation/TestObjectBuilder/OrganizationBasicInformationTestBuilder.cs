using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Account.Domain.Tests.Organisation.TestData;

namespace HE.Investments.Account.Domain.Tests.Organisation.TestObjectBuilder;
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
            OrganizationBasicInformationTestData.OrganizationBasicInformationOne.ContactInformation,
            OrganizationBasicInformationTestData.OrganizationBasicInformationOne.InvestmentPartnerStatus));

    public OrganizationBasicInformation Build()
    {
        return _item;
    }
}
