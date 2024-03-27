using System.Net;
using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investments.Organisation.CompaniesHouse;
using HE.Investments.Organisation.CompaniesHouse.Contract;
using HE.Investments.Organisation.Contract;
using Microsoft.Extensions.Logging;

namespace HE.Investments.Organisation.Services;

public class OrganisationSearchService : IOrganisationSearchService
{
    private readonly ICompaniesHouseApi _companiesHouseApi;
    private readonly IOrganizationCrmSearchService _organizationCrmSearchService;
    private readonly ILogger<OrganisationSearchService> _logger;

    public OrganisationSearchService(
        ICompaniesHouseApi companiesHouseApi,
        IOrganizationCrmSearchService organizationCrmSearchService,
        ILogger<OrganisationSearchService> logger)
    {
        _companiesHouseApi = companiesHouseApi;
        _organizationCrmSearchService = organizationCrmSearchService;
        _logger = logger;
    }

    public async Task<GetOrganizationByCompaniesHouseNumberResult> GetByOrganisationId(string organisationId, CancellationToken cancellationToken)
    {
        var result = await _organizationCrmSearchService.SearchOrganizationInCrmByOrganizationId(organisationId);
        if (result is null)
        {
            return new GetOrganizationByCompaniesHouseNumberResult(null, $"Organization with id {organisationId} not found");
        }

        return new GetOrganizationByCompaniesHouseNumberResult(
            new OrganisationSearchItem(
                result.companyRegistrationNumber,
                result.registeredCompanyName,
                result.city,
                result.addressLine1,
                result.postalcode,
                result.organisationId,
                true));
    }

    public async Task<GetOrganizationByCompaniesHouseNumberResult> GetByOrganisation(string companiesHouseNumberOrOrganisationId, CancellationToken cancellationToken)
    {
        var result = await GetByOrganisationId(companiesHouseNumberOrOrganisationId, cancellationToken);
        if (result.Item is not null)
        {
            return result;
        }

        return await GetByCompaniesHouseNumber(companiesHouseNumberOrOrganisationId, cancellationToken);
    }

    public async Task<OrganisationSearchResult> Search(string organisationName, PagingQueryParams pagingParams, CancellationToken cancellationToken = default)
    {
        var companyHousesResult = await GetOrganizationFromCompanyHousesApi(organisationName, null, pagingParams, cancellationToken);

        if (!companyHousesResult.IsSuccessful())
        {
            return companyHousesResult;
        }

        var companyHousesOrganizations = companyHousesResult.Items.ToList();

        var organizationsFromCrm = await GetMatchingOrganizationsFromCrm(companyHousesOrganizations);
        var mergedResult = MergeResults(companyHousesOrganizations, organizationsFromCrm);
        var foundSpvCompaniesCount = await AppendSpvCompanies(organisationName, pagingParams, companyHousesResult.TotalItems, mergedResult);

        return new OrganisationSearchResult(mergedResult, companyHousesResult.TotalItems + foundSpvCompaniesCount);
    }

    public async Task<GetOrganizationByCompaniesHouseNumberResult> GetByCompaniesHouseNumber(string? companiesHouseNumber, CancellationToken cancellationToken)
    {
        var companyHousesResult = await GetOrganizationFromCompanyHousesApi(null!, companiesHouseNumber, new PagingQueryParams(1, 0), cancellationToken);

        if (!companyHousesResult.IsSuccessful())
        {
            return new GetOrganizationByCompaniesHouseNumberResult(null!, companyHousesResult.Error!);
        }

        var companyHousesOrganizations = companyHousesResult.Items.ToList();

        var organizationsFromCrm = await GetMatchingOrganizationsFromCrm(companyHousesOrganizations);

        var mergedResult = MergeResults(companyHousesOrganizations, organizationsFromCrm);

        return new GetOrganizationByCompaniesHouseNumberResult(mergedResult.FirstOrDefault());
    }

    private async Task<int> AppendSpvCompanies(
        string organisationName,
        PagingQueryParams pagingParams,
        int totalItemsFromCompaniesHouseApi,
        IList<OrganisationSearchItem> mergedResult)
    {
        var result = await _organizationCrmSearchService.SearchOrganizationInCrmByName(organisationName, false);
        var displayedItems = pagingParams.StartIndex == 1 ? 0 : pagingParams.StartIndex;
        var skip = Math.Max(displayedItems - totalItemsFromCompaniesHouseApi, 0);
        foreach (var spvCompany in result.OrderBy(x => x.registeredCompanyName).Skip(skip))
        {
            if (mergedResult.Count >= pagingParams.Size)
            {
                break;
            }

            mergedResult.Add(new OrganisationSearchItem(
                spvCompany.companyRegistrationNumber,
                spvCompany.registeredCompanyName,
                spvCompany.city,
                spvCompany.addressLine1,
                spvCompany.postalcode,
                spvCompany.organisationId,
                true));
        }

        return result.Count;
    }

    private async Task<OrganisationSearchResult> GetOrganizationFromCompanyHousesApi(
        string organisationName,
        string? companiesHouseNumber,
        PagingQueryParams pagingParams,
        CancellationToken cancellationToken)
    {
        try
        {
            if (companiesHouseNumber is not null)
            {
                var result = await _companiesHouseApi.GetByCompanyNumber(companiesHouseNumber, cancellationToken);

                return result != null
                    ? OrganisationSearchResult.FromSingleItem(
                        new OrganisationSearchItem(
                            result.CompanyNumber,
                            result.CompanyName,
                            result.OfficeAddress.Locality!,
                            result.OfficeAddress.AddressLine1!,
                            result.OfficeAddress.PostalCode!,
                            null))
                    : OrganisationSearchResult.Empty();
            }

            var companyHouseApiResult = await _companiesHouseApi.Search(organisationName, pagingParams, cancellationToken);

            return new OrganisationSearchResult(
                companyHouseApiResult.Items.Select(x =>
                        new OrganisationSearchItem(
                            x.CompanyNumber,
                            x.CompanyName,
                            x.OfficeAddress.Locality!,
                            x.OfficeAddress.AddressLine1!,
                            x.OfficeAddress.PostalCode!,
                            null))
                    .ToList(),
                companyHouseApiResult.Hits);
        }
        catch (HttpRequestException ex)
        {
            if (pagingParams.StartIndex > 1 && ex.StatusCode == HttpStatusCode.InternalServerError)
            {
                // Company House API returns 500 error when we try to fetch page that does not exist
                // But we need TotalItems (Hits) to be able to calculate paging.
                return await GetTotalItemsResult(organisationName, pagingParams, cancellationToken);
            }

            _logger.LogError(ex, "Error occured while fetching organizations data from Company House API: {Message}", ex.Message);
            return new OrganisationSearchResult(Array.Empty<OrganisationSearchItem>(), 0, ex.Message);
        }
    }

    private async Task<OrganisationSearchResult> GetTotalItemsResult(string organisationName, PagingQueryParams pagingParams, CancellationToken cancellationToken)
    {
        try
        {
            var companyHouseApiResult = await _companiesHouseApi.Search(organisationName, pagingParams with { StartIndex = 1 }, cancellationToken);
            return new OrganisationSearchResult(Array.Empty<OrganisationSearchItem>(), companyHouseApiResult.Hits);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error occured while fetching organizations data from Company House API: {Message}", ex.Message);
            return new OrganisationSearchResult(Array.Empty<OrganisationSearchItem>(), 0, ex.Message);
        }
    }

    private async Task<IList<OrganizationDetailsDto>> GetMatchingOrganizationsFromCrm(IEnumerable<OrganisationSearchItem> companyHousesOrganizations)
    {
        var organizationCompanyNumbers = companyHousesOrganizations.Where(x => !string.IsNullOrEmpty(x.CompanyNumber)).Select(x => x.CompanyNumber!);

        return await _organizationCrmSearchService.SearchOrganizationInCrmByCompanyHouseNumber(organizationCompanyNumbers);
    }

    private IList<OrganisationSearchItem> MergeResults(
        IList<OrganisationSearchItem> companyHousesOrganizations,
        IList<OrganizationDetailsDto> organizationsFromCrm)
    {
        var organizationsThatExistInCrm = companyHousesOrganizations.Join(
                organizationsFromCrm,
                c => c.CompanyNumber,
                c => c.companyRegistrationNumber,
                (_, crm) => new OrganisationSearchItem(
                    crm.companyRegistrationNumber,
                    crm.registeredCompanyName,
                    crm.city,
                    crm.addressLine1,
                    crm.postalcode,
                    crm.organisationId,
                    true))
            .ToList();

        var organizationNumbersThatExistInCrm = organizationsThatExistInCrm.Select(c => c.CompanyNumber);

        var organizationsThatNotExistInCrm = companyHousesOrganizations.Where(ch => !organizationNumbersThatExistInCrm.Contains(ch.CompanyNumber));

        return organizationsThatNotExistInCrm.Concat(organizationsThatExistInCrm).ToList();
    }
}
