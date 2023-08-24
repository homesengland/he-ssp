using HE.InvestmentLoans.Contract.Exceptions;
using HE.InvestmentLoans.Contract.Organization;
using HE.InvestmentLoans.Contract.Organization.ValueObjects;
using HE.Investments.Organisation.CompaniesHouse.Contract;
using HE.Investments.Organisation.Services;
using MediatR;

namespace HE.InvestmentLoans.BusinessLogic.Organization.QueryHandlers;
internal class SearchOrganizationsHandler : IRequestHandler<SearchOrganizations, SearchOrganisationResponse>
{
    private readonly IOrganisationSearchService _searchService;

    public SearchOrganizationsHandler(IOrganisationSearchService searchService)
    {
        _searchService = searchService;
    }

    public async Task<SearchOrganisationResponse> Handle(SearchOrganizations request, CancellationToken cancellationToken)
    {
        var companyHousesResult = await _searchService.Search(request.SearchPhrase, new PagingQueryParams(request.PageSize, request.Page - 1), request.SearchPhrase, cancellationToken);

        if (!companyHousesResult.IsSuccessfull())
        {
            throw new ExternalServiceException();
        }

        var viewmodel = new OrganizationViewModel
        {
            Organizations = companyHousesResult.Items
            .Select(c => new OrganizationBasicDetails(c.Name, c.Street, c.City, c.PostalCode, c.CompanyNumber)),
            Name = request.SearchPhrase,
            TotalOrganizations = companyHousesResult.TotalItems,
            Page = request.Page,
        };

        return new SearchOrganisationResponse(viewmodel);
    }
}
