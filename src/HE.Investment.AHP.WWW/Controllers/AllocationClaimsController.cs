using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Contract.Claims.Queries;
using HE.Investments.AHP.Allocation.Domain.UserContext;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.Consortium.Shared.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[ConsortiumAuthorize(AllocationAccessContext.ViewClaims)]
[Route("{organisationId}/allocation/{allocationId}/claims")]
public class AllocationClaimsController : Controller
{
    private readonly IMediator _mediator;

    private readonly IAllocationAccessContext _allocationAccessContext;

    public AllocationClaimsController(IMediator mediator, IAllocationAccessContext allocationAccessContext)
    {
        _mediator = mediator;
        _allocationAccessContext = allocationAccessContext;
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
    [WorkflowState(AllocationClaimWorkflowState.Overview)]
    public async Task<IActionResult> Overview(string allocationId, string phaseId, CancellationToken cancellationToken)
    {
        var canEditClaim = await _allocationAccessContext.CanEditClaim();
        var phase = await _mediator.Send(new GetPhaseClaimsQuery(AllocationId.From(allocationId), PhaseId.From(phaseId)), cancellationToken);

        return View((phase, canEditClaim));
    }
}
