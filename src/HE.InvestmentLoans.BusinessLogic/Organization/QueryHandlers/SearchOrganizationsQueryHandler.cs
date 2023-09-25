extern alias Org;

using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Contract.Organization;
using HE.InvestmentLoans.Contract.Organization.ValueObjects;
using MediatR;
using Org.HE.Investments.Organisation.CompaniesHouse.Contract;
using Org.HE.Investments.Organisation.Services;

namespace HE.InvestmentLoans.BusinessLogic.Organization.QueryHandlers;
internal class SearchOrganizationsQueryHandler : IRequestHandler<SearchOrganizationsQuery, SearchOrganisationsQueryResponse>
{
    private readonly IOrganisationSearchService _searchService;

    public SearchOrganizationsQueryHandler(IOrganisationSearchService searchService)
    {
        _searchService = searchService;
    }

    public async Task<SearchOrganisationsQueryResponse> Handle(SearchOrganizationsQuery request, CancellationToken cancellationToken)
    {
        var companyHousesResult = await _searchService.Search(request.SearchPhrase, new PagingQueryParams(request.PageSize, (request.Page - 1) * request.PageSize), cancellationToken);

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

        return new SearchOrganisationsQueryResponse(viewmodel);
    }
}
