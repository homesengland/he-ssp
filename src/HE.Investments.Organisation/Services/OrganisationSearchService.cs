using System.Diagnostics.CodeAnalysis;
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

    public OrganisationSearchService(ICompaniesHouseApi companiesHouseApi, IOrganizationCrmSearchService organizationCrmSearchService, ILogger<OrganisationSearchService> logger)
    {
        _companiesHouseApi = companiesHouseApi;
        _organizationCrmSearchService = organizationCrmSearchService;
        _logger = logger;
    }

    public async Task<OrganisationSearchResult> Search(string organisationName, PagingQueryParams pagingParams, CancellationToken cancellationToken)
    {
        var companyHousesResult = await GetOrganizationFromCompanyHousesApi(organisationName, null, pagingParams, cancellationToken);

        if (!companyHousesResult.IsSuccessfull())
        {
            return companyHousesResult;
        }

        var companyHousesOrganizations = companyHousesResult.Items.ToList();

        var organizationsFromCrm = await GetMatchingOrganizationsFromCrm(companyHousesOrganizations);
        var mergedResult = MergeResults(companyHousesOrganizations, organizationsFromCrm);
        var foundSpvCompaniesCount = await AppendSpvCompanies(organisationName, pagingParams, mergedResult);

        return new OrganisationSearchResult(mergedResult, companyHousesResult.TotalItems + foundSpvCompaniesCount, null!);
    }

    public async Task<GetOrganizationByCompaniesHouseNumberResult> GetByCompaniesHouseNumber(string? companiesHouseNumber, CancellationToken cancellationToken)
    {
        var companyHousesResult = await GetOrganizationFromCompanyHousesApi(null!, companiesHouseNumber, new PagingQueryParams(1, 0), cancellationToken);

        if (!companyHousesResult.IsSuccessfull())
        {
            return new GetOrganizationByCompaniesHouseNumberResult(null!, companyHousesResult.Error);
        }

        var companyHousesOrganizations = companyHousesResult.Items.ToList();

        var organizationsFromCrm = await GetMatchingOrganizationsFromCrm(companyHousesOrganizations);

        var mergedResult = MergeResults(companyHousesOrganizations, organizationsFromCrm);

        return new GetOrganizationByCompaniesHouseNumberResult(mergedResult.FirstOrDefault()!, null!);
    }

    private async Task<int> AppendSpvCompanies(string organisationName, PagingQueryParams pagingParams, IList<OrganisationSearchItem> mergedResult)
    {
        var result = await _organizationCrmSearchService.SearchOrganizationInCrmByName(organisationName, false);
        foreach (var spvCompany in result.OrderBy(x => x.registeredCompanyName))
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
        string? compenirsHouseNumber,
        PagingQueryParams pagingParams,
        CancellationToken cancellationToken)
    {
        try
        {
            if (compenirsHouseNumber is not null)
            {
                var result = await _companiesHouseApi.GetByCompanyNumber(compenirsHouseNumber, cancellationToken);

                return result != null
                    ? OrganisationSearchResult.FromSingleItem(
                        new OrganisationSearchItem(
                            result!.CompanyNumber,
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
                companyHouseApiResult.Hits,
                null!);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error occured while fetching organizations data from Company House API: {Message}", ex.Message);

            return new OrganisationSearchResult(null!, 0, ex.Message);
        }
    }

    private async Task<IList<OrganizationDetailsDto>> GetMatchingOrganizationsFromCrm(IEnumerable<OrganisationSearchItem> companyHousesOrganizations)
    {
        var organizationCompanyNumbers = companyHousesOrganizations.Select(x => x.CompanyNumber);

        return await _organizationCrmSearchService.SearchOrganizationInCrmByCompanyHouseNumber(organizationCompanyNumbers);
    }

    private IList<OrganisationSearchItem> MergeResults(IList<OrganisationSearchItem> companyHousesOrganizations, IList<OrganizationDetailsDto> organizationsFromCrm)
    {
        var organizationsThatExistInCrm = companyHousesOrganizations.Join(
                organizationsFromCrm,
                c => c.CompanyNumber,
                c => c.companyRegistrationNumber,
                (ch, crm) => new OrganisationSearchItem(
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
