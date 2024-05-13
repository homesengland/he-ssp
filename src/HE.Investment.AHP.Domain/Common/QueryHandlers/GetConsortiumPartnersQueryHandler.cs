using HE.Investment.AHP.Contract.Common.Queries;
using HE.Investment.AHP.Domain.UserContext;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Contract.Enums;
using HE.Investments.AHP.Consortium.Contract.Queries;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Pagination;
using MediatR;

namespace HE.Investment.AHP.Domain.Common.QueryHandlers;

public class GetConsortiumPartnersQueryHandler : IRequestHandler<GetConsortiumPartnersQuery, PaginationResult<ConsortiumMemberDetails>>
{
    private readonly IMediator _mediator;

    private readonly IAhpUserContext _ahpUserContext;

    public GetConsortiumPartnersQueryHandler(IMediator mediator, IAhpUserContext ahpUserContext)
    {
        _mediator = mediator;
        _ahpUserContext = ahpUserContext;
    }

    public async Task<PaginationResult<ConsortiumMemberDetails>> Handle(GetConsortiumPartnersQuery request, CancellationToken cancellationToken)
    {
        var userAccount = await _ahpUserContext.GetSelectedAccount();
        if (userAccount.Consortium.HasNoConsortium)
        {
            throw new NotFoundException(nameof(userAccount.Consortium));
        }

        var consortiumDetails = await _mediator.Send(new GetConsortiumDetailsQuery(userAccount.Consortium.ConsortiumId, true), cancellationToken);
        var partners = new[] { consortiumDetails.LeadPartner }.Concat(consortiumDetails.Members.Where(x => x.Status == ConsortiumMemberStatus.Active))
            .OrderBy(x => x.Details.Name)
            .ToList();

        return new PaginationResult<ConsortiumMemberDetails>(
            partners.Skip((request.PaginationRequest.Page - 1) * request.PaginationRequest.ItemsPerPage).Take(request.PaginationRequest.ItemsPerPage).ToList(),
            request.PaginationRequest.Page,
            request.PaginationRequest.ItemsPerPage,
            partners.Count);
    }
}
