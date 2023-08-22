using HE.Investments.Organisation.Contract;

namespace HE.Investments.Organisation.Services;

public interface IOrganisationSearchService
{
    Task<OrganisationSearchResult> Search(string organisationName, string? companyNumber = null, CancellationToken cancellationToken = default);
}
