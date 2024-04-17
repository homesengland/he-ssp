extern alias Org;

using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Queries;
using HE.Investment.AHP.Domain.Site.Mappers;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Contract.Validators;
using MediatR;
using Org::HE.Investments.Organisation.LocalAuthorities.Repositories;

namespace HE.Investment.AHP.Domain.Site.QueryHandlers;

public class SearchLocalAuthoritiesQueryHandler : IRequestHandler<SearchLocalAuthoritiesQuery, OperationResult<LocalAuthorities>>
{
    private readonly ILocalAuthorityRepository _repository;

    public SearchLocalAuthoritiesQueryHandler(ILocalAuthorityRepository repository)
    {
        _repository = repository;
    }

    public async Task<OperationResult<LocalAuthorities>> Handle(SearchLocalAuthoritiesQuery request, CancellationToken cancellationToken)
    {
        var (items, totalItems) = await _repository.Search(
            request.Phrase,
            request.PaginationRequest.Page - 1,
            request.PaginationRequest.ItemsPerPage,
            cancellationToken);

        var result = new LocalAuthorities
        {
            Page = new PaginationResult<LocalAuthority>(
                items.Select(x => LocalAuthorityMapper.Map(x)!).ToList(),
                request.PaginationRequest.Page,
                request.PaginationRequest.ItemsPerPage,
                totalItems),
            Phrase = request.Phrase,
        };

        return OperationResult.Success(result);
    }
}
