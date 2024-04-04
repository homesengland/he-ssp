extern alias Org;

using HE.Investments.Common.Contract.Pagination;
using HE.Investments.FrontDoor.Contract.LocalAuthority.Queries;
using MediatR;

using Org::HE.Investments.Organisation.LocalAuthorities.Repositories;

namespace HE.Investments.FrontDoor.Domain.LocalAuthority.QueryHandlers;

public class GetLocalAuthoritiesQueryHandler : IRequestHandler<GetLocalAuthoritiesQuery, PaginationResult<Investments.Common.Contract.LocalAuthority>>
{
    private readonly ILocalAuthorityRepository _repository;

    public GetLocalAuthoritiesQueryHandler(ILocalAuthorityRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaginationResult<Investments.Common.Contract.LocalAuthority>> Handle(GetLocalAuthoritiesQuery request, CancellationToken cancellationToken)
    {
        var (items, totalItems) = await _repository.Search(request.Phrase, request.PaginationRequest.Page - 1, request.PaginationRequest.ItemsPerPage, cancellationToken);

        return new PaginationResult<Investments.Common.Contract.LocalAuthority>(
                items.Select(l => new Investments.Common.Contract.LocalAuthority(l.Code.Value, l.Name)).ToList(),
                request.PaginationRequest.Page,
                request.PaginationRequest.ItemsPerPage,
                totalItems);
    }
}
