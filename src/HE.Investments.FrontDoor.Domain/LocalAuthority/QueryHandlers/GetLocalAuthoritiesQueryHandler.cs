extern alias Org;

using HE.Investments.Common.Contract.Pagination;
using HE.Investments.FrontDoor.Contract.LocalAuthority.Queries;
using MediatR;

using Org::HE.Investments.Organisation.LocalAuthorities.Repositories;

namespace HE.Investments.FrontDoor.Domain.LocalAuthority.QueryHandlers;

public class GetLocalAuthoritiesQueryHandler : IRequestHandler<GetLocalAuthoritiesQuery, PaginationResult<Common.Contract.LocalAuthority>>
{
    private readonly ILocalAuthorityRepository _repository;

    public GetLocalAuthoritiesQueryHandler(ILocalAuthorityRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaginationResult<Common.Contract.LocalAuthority>> Handle(GetLocalAuthoritiesQuery request, CancellationToken cancellationToken)
    {
        //TODO: #92119 pages should be indexed form 1, there is no 0 page
        var result = await _repository.Search(request.Phrase, request.PaginationRequest.Page - 1, request.PaginationRequest.ItemsPerPage, cancellationToken);

        return new PaginationResult<Common.Contract.LocalAuthority>(
                result.Items.Select(l => new Common.Contract.LocalAuthority(l.Id.Value, l.Name)).ToList(),
                request.PaginationRequest.Page,
                request.PaginationRequest.ItemsPerPage,
                result.TotalItems);
    }
}
