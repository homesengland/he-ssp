using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.Organisation.Services;
internal interface IOrganizationCrmSearchService
{
    IEnumerable<OrganizationDetailsDto> SearchOrganizationInCrm(IEnumerable<string> organisationNumbers);
}
