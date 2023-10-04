extern alias Org;
using HE.InvestmentLoans.BusinessLogic.Tests.Organization.TestData;
using Org::HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.InvestmentLoans.BusinessLogic.Tests.Organization.TestObjectBuilder;
public class OrganizationDetailsDtoTestBuilder
{
    private readonly OrganizationDetailsDto _item;

    private OrganizationDetailsDtoTestBuilder(OrganizationDetailsDto organizationDetailsDto)
    {
        _item = organizationDetailsDto;
    }

    public static OrganizationDetailsDtoTestBuilder New()
    {
        return new(new OrganizationDetailsDto()
        {
            registeredCompanyName = OrganizationDetailsDtoTestData.OrganizationDetailsDto.registeredCompanyName,
            companyRegistrationNumber = OrganizationDetailsDtoTestData.OrganizationDetailsDto.companyRegistrationNumber,
            city = OrganizationDetailsDtoTestData.OrganizationDetailsDto.city,
            country = OrganizationDetailsDtoTestData.OrganizationDetailsDto.country,
            postalcode = OrganizationDetailsDtoTestData.OrganizationDetailsDto.postalcode,
            addressLine1 = OrganizationDetailsDtoTestData.OrganizationDetailsDto.addressLine1,
            addressLine2 = OrganizationDetailsDtoTestData.OrganizationDetailsDto.addressLine2,
            addressLine3 = OrganizationDetailsDtoTestData.OrganizationDetailsDto.addressLine3,
            compayAdminContactEmail = OrganizationDetailsDtoTestData.OrganizationDetailsDto.compayAdminContactEmail,
            compayAdminContactName = OrganizationDetailsDtoTestData.OrganizationDetailsDto.compayAdminContactName,
            compayAdminContactTelephone = OrganizationDetailsDtoTestData.OrganizationDetailsDto.compayAdminContactTelephone,
            rpNumber = OrganizationDetailsDtoTestData.OrganizationDetailsDto.rpNumber,
        });
    }

    public OrganizationDetailsDto Build()
    {
        return _item;
    }
}
