extern alias Org;

using HE.InvestmentLoans.Contract.Exceptions;
using HE.InvestmentLoans.Contract.Organization;
using HE.InvestmentLoans.Contract.Organization.ValueObjects;
using MediatR;
using Microsoft.PowerPlatform.Dataverse.Client;
using Org.HE.Common.IntegrationModel.PortalIntegrationModel;
using Org.HE.Investments.Organisation.CompaniesHouse.Contract;
using Org.HE.Investments.Organisation.Contract;
using Org.HE.Investments.Organisation.Services;

namespace HE.InvestmentLoans.BusinessLogic.Organization.QueryHandlers;
internal class SearchOrganizationsHandler : IRequestHandler<SearchOrganizations, SearchOrganisationResponse>
{
    private readonly IOrganisationSearchService _searchService;
    private readonly IOrganizationServiceAsync2 _organizationService;

    public SearchOrganizationsHandler(IOrganisationSearchService searchService, IOrganizationServiceAsync2 organizationService)
    {
        _searchService = searchService;
        _organizationService = organizationService;
    }

    public async Task<SearchOrganisationResponse> Handle(SearchOrganizations request, CancellationToken cancellationToken)
    {
        var (companyHousesOrganizations, totalOrganizations) = await GetOrganizationsFromCompanyHouses(request, cancellationToken);

        var organizationsFromCrm = GetMatchingOrganizationsFromCrm(companyHousesOrganizations);

        var mergedResult = MergeOrganizations(companyHousesOrganizations, organizationsFromCrm);

        var viewmodel = new OrganizationViewModel
        {
            Organizations = mergedResult
            .Select(c => new OrganizationBasicDetails(c.Name, c.Street, c.City, c.PostalCode, c.CompanyNumber)),
            Name = request.SearchPhrase,
            TotalOrganizations = totalOrganizations,
            Page = request.Page,
        };

        return new SearchOrganisationResponse(viewmodel);
    }

    private async Task<(IList<OrganisationSearchItem> Organizations, int TotalOrganizations)> GetOrganizationsFromCompanyHouses(SearchOrganizations request, CancellationToken cancellationToken)
    {
        var companyHousesResult = await _searchService.Search(request.SearchPhrase, new PagingQueryParams(request.PageSize, request.Page - 1), request.SearchPhrase, cancellationToken);

        if (!companyHousesResult.IsSuccessfull())
        {
            throw new ExternalServiceException();
        }

        return (companyHousesResult.Items, companyHousesResult.TotalItems);
    }

    private IEnumerable<OrganizationDetailsDto> GetMatchingOrganizationsFromCrm(IList<OrganisationSearchItem> companyHousesOrganizations)
    {
        var organizationCompanyNumbers = companyHousesOrganizations.Select(x => x.CompanyNumber);

        var organizationsFromCrm = _searchService.SearchOrganizationInCrm(organizationCompanyNumbers, _organizationService);

        return organizationsFromCrm;
    }

    private IEnumerable<OrganisationSearchItem> MergeOrganizations(IList<OrganisationSearchItem> companyHousesOrganizations, IEnumerable<OrganizationDetailsDto> organizationsFromCrm)
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
