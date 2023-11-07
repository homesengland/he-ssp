extern alias Org;
using HE.InvestmentLoans.Common.Exceptions;
using HE.InvestmentLoans.Contract.Organization;
using HE.InvestmentLoans.Contract.Organization.ValueObjects;
using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Organisation.CompaniesHouse.Contract;
using HE.Investments.Organisation.Services;
using MediatR;

namespace HE.Investments.Account.Domain.Organisation.QueryHandlers;

internal class SearchOrganisationsQueryHandler : IRequestHandler<SearchOrganizationsQuery, SearchOrganisationsQueryResponse>
{
    private readonly IOrganisationSearchService _searchService;

    public SearchOrganisationsQueryHandler(IOrganisationSearchService searchService)
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

        var viewmodel = new OrganisationSearchModel
        {
            Organizations = companyHousesResult.Items
            .Select(c => new OrganizationBasicDetails(c.Name, c.Street, c.City, c.PostalCode, c.CompanyNumber, c.OrganisationId)),
            Name = request.SearchPhrase,
            TotalOrganizations = companyHousesResult.TotalItems,
            Page = request.Page,
            ItemsPerPage = request.PageSize,
        };

        return new SearchOrganisationsQueryResponse(viewmodel);
    }
}
