using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.Organisation.Tests.TestObjectBuilders;

public class OrganizationDetailsDtoTestBuilder
{
    private readonly OrganizationDetailsDto _item;

    public OrganizationDetailsDtoTestBuilder(OrganizationDetailsDto item)
    {
        _item = item;
    }

    public static OrganizationDetailsDtoTestBuilder New() => new(new OrganizationDetailsDto());

    public static OrganizationDetailsDtoTestBuilder NewSpvCompany(string organisationId = "CRM1234567")
    {
        return new OrganizationDetailsDtoTestBuilder(new OrganizationDetailsDto
        {
            registeredCompanyName = "SPV Company" + organisationId,
            companyRegistrationNumber = null,
            organisationId = organisationId
        });
    }

    public OrganizationDetailsDtoTestBuilder WithCompanyHouseNumber(string? companyHouseNumber = "1234567")
    {
        _item.companyRegistrationNumber = companyHouseNumber;
        return this;
    }

    public OrganizationDetailsDto Build() => _item;
}
