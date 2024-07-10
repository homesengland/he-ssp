using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Contract.Claims.Queries;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Consortium.Shared.UserContext;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[AuthorizeWithCompletedProfile]
[Route("{organisationId}/allocation/{allocationId}/claims")]
public class AllocationClaimsController : Controller
{
    private readonly IMediator _mediator;

    private readonly IConsortiumAccessContext _accountAccessContext;

    public AllocationClaimsController(IMediator mediator, IConsortiumAccessContext accountAccessContext)
    {
        _mediator = mediator;
        _accountAccessContext = accountAccessContext;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> Summary([FromQuery] int? page, string allocationId, CancellationToken cancellationToken)
    {
        var allocationClaims =
            await _mediator.Send(
                new GetAllocationClaimsQuery(AllocationId.From(allocationId), new PaginationRequest(page ?? 1, 3)),
                cancellationToken);
        return View(allocationClaims);
    }

    [HttpGet("{phaseId}/overview")]
    public async Task<IActionResult> Overview(string allocationId, string phaseId, CancellationToken cancellationToken)
    {
        var canClaimMilestone = await _accountAccessContext.CanEditApplication();
        var phase = await _mediator.Send(new GetPhaseClaimsQuery(AllocationId.From(allocationId), PhaseId.From(phaseId)), cancellationToken);

        return View((phase, canClaimMilestone));
    }
}
