using HE.Investments.Organisation.CompaniesHouse.Contract;
using HE.Investments.Organisation.Contract;

namespace HE.Investments.Organisation.Services;

public interface IOrganisationSearchService
{
    Task<OrganisationSearchResult> Search(string organisationName, PagingQueryParams pagingParams, string? companyNumber = null, CancellationToken cancellationToken = default);
}
