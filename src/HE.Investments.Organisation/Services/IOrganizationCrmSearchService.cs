using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.Organisation.Services;
internal interface IOrganizationCrmSearchService
{
    IEnumerable<OrganizationDetailsDto> SearchOrganizationInCrmByName(string organisationNames, bool recordsWithoutCopanyNumberIncluded);

    IEnumerable<OrganizationDetailsDto> SearchOrganizationInCrmByCompanyHouseNumber(IEnumerable<string> organisationNumbers);
}
