using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Organisation.Contract;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Organisation.Services;

public interface IOrganisationSearchService
{
    Task<OrganisationSearchResult> Search(string organisationName, string? companyNumber = null, CancellationToken cancellationToken = default);
    List<OrganizationDetailsDto> SearchOrganizationInCrm(List<string> organisationNumbers, IOrganizationServiceAsync2 service);
}
