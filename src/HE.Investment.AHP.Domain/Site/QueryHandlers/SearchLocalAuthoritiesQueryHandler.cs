using HE.Common.IntegrationModel.PortalIntegrationModel;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Queries;
using HE.Investment.AHP.Domain.Site.Repositories;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Contract.Validators;
using MediatR;

namespace HE.Investment.AHP.Domain.Site.QueryHandlers;

public class SearchLocalAuthoritiesQueryHandler : IRequestHandler<SearchLocalAuthoritiesQuery, OperationResult<LocalAuthorities>>
{
    private readonly IAhgLocalAuthorityRepository _repository;

    public SearchLocalAuthoritiesQueryHandler(IAhgLocalAuthorityRepository repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<LocalAuthorities>> Handle(SearchLocalAuthoritiesQuery request, CancellationToken cancellationToken)
    {
        var paging = new PagingRequestDto { pageNumber = request.PaginationRequest.Page, pageSize = request.PaginationRequest.ItemsPerPage };
        var localAuthorities = await _repository.Get(paging, request.Phrase, cancellationToken);

        var result = new LocalAuthorities
        {
            Page = new PaginationResult<LocalAuthority>(
                localAuthorities.items.Select(c => new LocalAuthority { Id = c.id, Name = c.name }).ToList(),
                request.PaginationRequest.Page,
                request.PaginationRequest.ItemsPerPage,
                localAuthorities.totalItemsCount),
            Phrase = request.Phrase,
        };

        return OperationResult.Success(result);
    }
}
