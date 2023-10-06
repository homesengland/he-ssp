using HE.Investments.Organisation.CompaniesHouse.Contract;
using HE.Investments.Organisation.Contract;

namespace HE.Investments.Organisation.Services;

public interface IOrganisationSearchService
{
    Task<GetOrganizationByCompaniesHouseNumberResult> GetByCompaniesHouseNumber(string? companiesHouseNumber, CancellationToken cancellationToken);

    Task<GetOrganizationByCompaniesHouseNumberResult> GetByOrganisationId(string organisationId, CancellationToken cancellationToken);

    Task<GetOrganizationByCompaniesHouseNumberResult> GetByOrganisation(string companiesHouseNumberOrOrganisationId, CancellationToken cancellationToken);

    Task<OrganisationSearchResult> Search(string organisationName, PagingQueryParams pagingParams, CancellationToken cancellationToken = default);
}
