using System.Diagnostics.CodeAnalysis;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Organisation.CompaniesHouse;
using HE.Investments.Organisation.CompaniesHouse.Contract;
using HE.Investments.Organisation.Contract;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Organisation.Services;

internal class OrganisationSearchService : IOrganisationSearchService
{
    private readonly ICompaniesHouseApi _companiesHouseApi;
    private readonly IOrganizationCrmSearchService _organizationCrmSearchService;
    private readonly ILogger<OrganisationSearchService> _logger;

    public OrganisationSearchService(ICompaniesHouseApi companiesHouseApi, IOrganizationCrmSearchService organizationCrmSearchService, ILogger<OrganisationSearchService> logger)
    {
        _companiesHouseApi = companiesHouseApi;
        _organizationCrmSearchService = organizationCrmSearchService;
        _logger = logger;
    }

    public async Task<OrganisationSearchResult> Search(string organisationName, PagingQueryParams pagingParams, string? companyNumber, CancellationToken cancellationToken)
    {
        var companyHousesResult = await GetOrganizationFromCompanyHousesApi(organisationName, pagingParams, cancellationToken);

        if (!companyHousesResult.IsSuccessfull())
        {
            return companyHousesResult;
        }

        var companyHousesOrganizations = companyHousesResult.Items;

        var organizationsFromCrm = GetMatchingOrganizationsFromCrm(companyHousesOrganizations);

        var mergedResult = MergeResultes(companyHousesOrganizations, organizationsFromCrm);

        return new OrganisationSearchResult(mergedResult, companyHousesResult.TotalItems, null!);
    }

    [SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates", Justification = "We dont need high performace logging.")]
    private async Task<OrganisationSearchResult> GetOrganizationFromCompanyHousesApi(string organisationName, PagingQueryParams pagingParams, CancellationToken cancellationToken)
    {
        try
        {
            var companyHouseApiResult = await _companiesHouseApi.Search(organisationName, pagingParams, cancellationToken);

            return new OrganisationSearchResult(companyHouseApiResult.Items.Select(x => new OrganisationSearchItem(x.CompanyNumber, x.CompanyName, x.OfficeAddress.Locality!, x.OfficeAddress.AddressLine1!, x.OfficeAddress.PostalCode!)).ToList(), companyHouseApiResult.Hits, null!);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error occured while fetching organizations data from Company House API: {Message}", ex.Message);

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
