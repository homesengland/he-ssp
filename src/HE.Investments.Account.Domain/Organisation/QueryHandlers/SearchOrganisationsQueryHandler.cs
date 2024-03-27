using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Organisation.CompaniesHouse.Contract;
using HE.Investments.Organisation.Services;
using MediatR;

namespace HE.Investments.Account.Domain.Organisation.QueryHandlers;

internal class SearchOrganisationsQueryHandler : IRequestHandler<SearchOrganisationsQuery, SearchOrganisationsQueryResponse>
{
    private readonly IOrganisationSearchService _searchService;

    public SearchOrganisationsQueryHandler(IOrganisationSearchService searchService)
    {
        _searchService = searchService;
    }

    public async Task<SearchOrganisationsQueryResponse> Handle(SearchOrganisationsQuery request, CancellationToken cancellationToken)
    {
        var companyHousesResult = await _searchService.Search(request.SearchPhrase, new PagingQueryParams(request.PageSize, (request.Page - 1) * request.PageSize), cancellationToken);

        if (!companyHousesResult.IsSuccessful())
        {
            throw new ExternalServiceException();
        }

        var viewmodel = new OrganisationSearchModel
        {
            Organisations = companyHousesResult.Items
                .Select(c => new OrganisationBasicDetails(c.Name, c.Street, c.City, c.PostalCode, c.CompanyNumber, c.OrganisationId))
                .ToList(),
            Name = request.SearchPhrase,
            TotalOrganisations = companyHousesResult.TotalItems,
            Page = request.Page,
            ItemsPerPage = request.PageSize,
        };

        return new SearchOrganisationsQueryResponse(viewmodel);
    }
}
