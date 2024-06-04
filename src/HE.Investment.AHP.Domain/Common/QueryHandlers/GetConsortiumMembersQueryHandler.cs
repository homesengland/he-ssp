using HE.Investment.AHP.Contract.Common.Queries;
using HE.Investment.AHP.Domain.UserContext;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Contract.Enums;
using HE.Investments.AHP.Consortium.Contract.Queries;
using HE.Investments.AHP.Consortium.Shared.UserContext;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Pagination;
using MediatR;

namespace HE.Investment.AHP.Domain.Common.QueryHandlers;

public class GetConsortiumMembersQueryHandler : IRequestHandler<GetConsortiumMembersQuery, PaginationResult<ConsortiumMemberDetails>>
{
    private readonly IMediator _mediator;

    private readonly IConsortiumUserContext _consortiumUserContext;

    public GetConsortiumMembersQueryHandler(IMediator mediator, IConsortiumUserContext consortiumUserContext)
    {
        _mediator = mediator;
        _consortiumUserContext = consortiumUserContext;
    }

    public async Task<PaginationResult<ConsortiumMemberDetails>> Handle(GetConsortiumMembersQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _consortiumUserContext.GetSelectedAccount();
        if (userAccount.Consortium.HasNoConsortium)
        {
            throw new NotFoundException(nameof(userAccount.Consortium));
        }

        var consortiumDetails = await _mediator.Send(new GetConsortiumDetailsQuery(userAccount.Consortium.ConsortiumId, true), cancellationToken);
        var members = new[] { consortiumDetails.LeadPartner }.Concat(consortiumDetails.Members.Where(x => x.Status == ConsortiumMemberStatus.Active))
            .OrderBy(x => x.Details.Name)
            .ToList();

        return new PaginationResult<ConsortiumMemberDetails>(
            members.Skip((request.PaginationRequest.Page - 1) * request.PaginationRequest.ItemsPerPage).Take(request.PaginationRequest.ItemsPerPage).ToList(),
            request.PaginationRequest.Page,
            request.PaginationRequest.ItemsPerPage,
            members.Count);
    }
}
