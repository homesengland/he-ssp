using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Organisation.CompaniesHouse;
using HE.Investments.Organisation.CompaniesHouse.Contract;
using HE.Investments.Organisation.Contract;

namespace HE.Investments.Organisation.Services;

internal class OrganisationSearchService : IOrganisationSearchService
{
    private readonly ICompaniesHouseApi _companiesHouseApi;
    private readonly IOrganizationCrmSearchService _organizationCrmSearchService;

    public OrganisationSearchService(ICompaniesHouseApi companiesHouseApi, IOrganizationCrmSearchService organizationCrmSearchService)
    {
        _companiesHouseApi = companiesHouseApi;
        _organizationCrmSearchService = organizationCrmSearchService;
    }

    public async Task<OrganisationSearchResult> Search(string organisationName, PagingQueryParams pagingParams, string? companyNumber, CancellationToken cancellationToken)
    {
        var companyHousesResult = await GetOrganizationFromCompenyHousesApi(organisationName, pagingParams, cancellationToken);

        if (!companyHousesResult.IsSuccessfull())
        {
            return companyHousesResult;
        }

        var companyHousesOrganizations = companyHousesResult.Items;

        var organizationsFromCrm = GetMatchingOrganizationsFromCrm(companyHousesOrganizations);

        var mergedResult = MergeResultes(companyHousesOrganizations, organizationsFromCrm);

        return new OrganisationSearchResult(mergedResult, companyHousesResult.TotalItems, null!);
    }

    private async Task<OrganisationSearchResult> GetOrganizationFromCompenyHousesApi(string organisationName, PagingQueryParams pagingParams, CancellationToken cancellationToken)
    {
        try
        {
            var companyHouseApiResult = await _companiesHouseApi.Search(organisationName, pagingParams, cancellationToken);

            return new OrganisationSearchResult(companyHouseApiResult.Items.Select(x => new OrganisationSearchItem(x.CompanyNumber, x.CompanyName, x.OfficeAddress.Locality!, x.OfficeAddress.AddressLine1!, x.OfficeAddress.PostalCode!)).ToList(), companyHouseApiResult.Hits, null!);
        }
        catch (HttpRequestException ex)
        {
            return new OrganisationSearchResult(null!, 0, ex.Message);
        }
    }

    private IEnumerable<OrganizationDetailsDto> GetMatchingOrganizationsFromCrm(IEnumerable<OrganisationSearchItem> companyHousesOrganizations)
    {
        var organizationCompanyNumbers = companyHousesOrganizations.Select(x => x.CompanyNumber);

        return _organizationCrmSearchService.SearchOrganizationInCrm(organizationCompanyNumbers);
    }

    private IEnumerable<OrganisationSearchItem> MergeResultes(IEnumerable<OrganisationSearchItem> companyHousesOrganizations, IEnumerable<OrganizationDetailsDto> organizationsFromCrm)
    {
        var organizationsThatExistInCrm = companyHousesOrganizations.Join(
            organizationsFromCrm,
            c => c.CompanyNumber,
            c => c.companyRegistrationNumber,
            (ch, crm) => new OrganisationSearchItem(crm.companyRegistrationNumber, crm.registeredCompanyName, crm.city, crm.addressLine1, crm.postalcode));

        var organizationNumbersThatExistInCrm = organizationsThatExistInCrm.Select(c => c.CompanyNumber);

        var organizationsThatNotExistInCrm = companyHousesOrganizations.Where(ch => !organizationNumbersThatExistInCrm.Contains(ch.CompanyNumber));

        return organizationsThatNotExistInCrm.Concat(organizationsThatExistInCrm);
    }
}
