using HE.Common.IntegrationModel.PortalIntegrationModel;

namespace HE.Investments.Organisation.Services;

public interface IOrganizationCrmSearchService
{
    Task<IList<OrganizationDetailsDto>> SearchOrganizationInCrmByName(string organisationNames, bool recordsWithCompanyHouseNumberIncluded);

    Task<IList<OrganizationDetailsDto>> SearchOrganizationInCrmByCompanyHouseNumber(IEnumerable<string> organisationNumbers);

    Task<OrganizationDetailsDto?> SearchOrganizationInCrmByOrganizationId(string organizationIds);
}
