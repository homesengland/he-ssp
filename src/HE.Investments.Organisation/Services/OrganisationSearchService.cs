using HE.Investments.Organisation.CompaniesHouse;
using HE.Investments.Organisation.Contract;

namespace HE.Investments.Organisation.Services;

public class OrganisationSearchService : IOrganisationSearchService
{
    private readonly ICompaniesHouseApi _companiesHouseApi;

    public OrganisationSearchService(ICompaniesHouseApi companiesHouseApi)
    {
        _companiesHouseApi = companiesHouseApi;
    }

    public async Task<OrganisationSearchResult> Search(string organisationName, string? companyNumber, CancellationToken cancellationToken)
    {
        var companyHouseApiResult = await _companiesHouseApi.Search(organisationName, cancellationToken);
        return new OrganisationSearchResult(companyHouseApiResult.Items.Select(x => new OrganisationSearchItem(x.CompanyNumber, x.Title)).ToList());
    }
}
