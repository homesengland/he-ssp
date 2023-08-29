using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Organisation.CompaniesHouse.Contract;
using HE.Investments.Organisation.Contract;
using Microsoft.PowerPlatform.Dataverse.Client;

namespace HE.Investments.Organisation.Services;

public interface IOrganisationSearchService
{
    Task<OrganisationSearchResult> Search(string organisationName, PagingQueryParams pagingParams, string? companyNumber = null, CancellationToken cancellationToken = default);

    IEnumerable<OrganizationDetailsDto> SearchOrganizationInCrm(IEnumerable<string> organisationNumbers, IOrganizationServiceAsync2 service);
}
