extern alias Org;

using HE.Investments.Common.Contract.Pagination;
using HE.Investments.FrontDoor.Contract.Project.Queries;
using MediatR;

using Org::HE.Investments.Organisation.LocalAuthorities.Repositories;
using LocalAuth = Org::HE.Investments.Organisation.LocalAuthorities.ValueObjects.LocalAuthority;

namespace HE.Investments.FrontDoor.Domain.LocalAuthority.QueryHandlers;

public class GetLocalAuthoritiesQueryHandler : IRequestHandler<GetLocalAuthoritiesQuery, PaginationResult<LocalAuth>>
{
    private readonly ILocalAuthorityRepository _repository;

    public GetLocalAuthoritiesQueryHandler(ILocalAuthorityRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaginationResult<LocalAuth>> Handle(GetLocalAuthoritiesQuery request, CancellationToken cancellationToken)
    {
        //TODO: pages should be indexed form 1, there is no 0 page
        var result = await _repository.Search(request.Phrase, request.PaginationRequest.Page - 1, request.PaginationRequest.ItemsPerPage, cancellationToken);

        return new PaginationResult<LocalAuth>(
                result.Items.Select(l => new LocalAuth(l.Id, l.Name)).ToList(),
                request.PaginationRequest.Page,
                request.PaginationRequest.ItemsPerPage,
                result.TotalItems);
    }
}
