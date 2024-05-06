extern alias Org;

using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Contract.Queries;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Pagination;
using MediatR;
using IOrganisationSearchService = Org::HE.Investments.Organisation.Services.IOrganisationSearchService;
using OrganisationSearchItem = Org::HE.Investments.Organisation.Contract.OrganisationSearchItem;
using PagingQueryParams = Org::HE.Investments.Organisation.CompaniesHouse.Contract.PagingQueryParams;

namespace HE.Investments.AHP.Consortium.Domain.QueryHandlers;

public class SearchOrganisationQueryHandler : IRequestHandler<SearchOrganisationQuery, SearchOrganisationResult>
{
    private readonly IOrganisationSearchService _organisationSearchService;

    public SearchOrganisationQueryHandler(IOrganisationSearchService organisationSearchService)
    {
        _organisationSearchService = organisationSearchService;
    }

    public async Task<SearchOrganisationResult> Handle(SearchOrganisationQuery request, CancellationToken cancellationToken)
    {
        var result = await _organisationSearchService.Search(
            request.Phrase,
            new PagingQueryParams(request.PaginationRequest.ItemsPerPage, (request.PaginationRequest.Page - 1) * request.PaginationRequest.ItemsPerPage),
            cancellationToken);
        if (!result.IsSuccessful())
        {
            throw new ExternalServiceException($"Organisation Search failed with error: {result.Error}");
        }

        return new SearchOrganisationResult(
            request.Phrase,
            new PaginationResult<OrganisationDetails>(
                result.Items.Select(MapOrganisationDetails).ToList(),
                request.PaginationRequest.Page,
                request.PaginationRequest.ItemsPerPage,
                result.TotalItems));
    }

    private static OrganisationDetails MapOrganisationDetails(OrganisationSearchItem item)
    {
        return new OrganisationDetails(
            item.Name,
            item.Street,
            item.City,
            item.PostalCode,
            item.CompanyNumber,
            item.OrganisationId);
    }
}
