extern alias Org;

using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Queries;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Contract.Validators;
using MediatR;
using Org::HE.Investments.Organisation.LocalAuthorities.Repositories;

namespace HE.Investment.AHP.Domain.Site.QueryHandlers;

public class SearchLocalAuthoritiesQueryHandler : IRequestHandler<SearchLocalAuthoritiesQuery, OperationResult<LocalAuthorities>>
{
    private readonly ILocalAuthorityRepository _localAuthorityRepository;

    public SearchLocalAuthoritiesQueryHandler(ILocalAuthorityRepository localAuthorityRepository)
    {
        _localAuthorityRepository = localAuthorityRepository;
    }

    public async Task<OperationResult<LocalAuthorities>> Handle(SearchLocalAuthoritiesQuery request, CancellationToken cancellationToken)
    {
        var (itemsFound, totalItems) =
            await _localAuthorityRepository.Search(request.Phrase, request.PaginationRequest.Page, request.PaginationRequest.ItemsPerPage, cancellationToken);

        var result = new LocalAuthorities
        {
            Page = new PaginationResult<LocalAuthority>(
                itemsFound.Select(c => new LocalAuthority { Id = c.Id.ToString(), Name = c.Name }).ToList(),
                request.PaginationRequest.Page,
                request.PaginationRequest.ItemsPerPage,
                totalItems),
            Phrase = request.Phrase,
        };

        return OperationResult.Success(result);
    }
}
