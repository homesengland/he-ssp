using HE.Investment.AHP.WWW.Models.AllocationClaim;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Contract.Claims.Queries;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.Consortium.Shared.Authorization;
using HE.Investments.Consortium.Shared.UserContext;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[AuthorizeWithCompletedProfile]
[ConsortiumAuthorize(ConsortiumAccessContext.Submit)]
[Route("{organisationId}/allocation/{allocationId}/claims/{phaseId}/{claimType}")]
public class AllocationClaimController : WorkflowController<AllocationClaimWorkflowState>
{
    private readonly IMediator _mediator;

    public AllocationClaimController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("back")]
    public async Task<IActionResult> Back(
        [FromRoute] string organisationId,
        [FromRoute] string allocationId,
        [FromRoute] string phaseId,
        [FromRoute] MilestoneType claimType,
        AllocationClaimWorkflowState currentPage)
    {
        return await Back(currentPage, new { organisationId, allocationId, phaseId, claimType });
    }

    [HttpGet("continue-answering")]
    public IActionResult ContinueAnswering(
        [FromRoute] string organisationId,
        [FromRoute] string allocationId,
        [FromRoute] string phaseId,
        [FromRoute] MilestoneType claimType)
    {
        // TODO: AB#103021 Continue section answering
        return RedirectToAction(claimType == MilestoneType.Acquisition ? "CostsIncurred" : "MilestoneDate", new { organisationId, allocationId, phaseId, claimType });
    }

    [HttpGet("costs-incurred")]
    [WorkflowState(AllocationClaimWorkflowState.CostsIncurred)]
    public async Task<IActionResult> CostsIncurred(
        [FromRoute] string allocationId,
        [FromRoute] string phaseId,
        [FromRoute] MilestoneType claimType,
        CancellationToken cancellationToken)
    {
        return View(await GetClaimModel(allocationId, phaseId, claimType, cancellationToken));
    }

    [HttpPost("costs-incurred")]
    [WorkflowState(AllocationClaimWorkflowState.CostsIncurred)]
    public async Task<IActionResult> CostsIncurred(
        [FromRoute] string organisationId,
        [FromRoute] string allocationId,
        [FromRoute] string phaseId,
        [FromRoute] MilestoneType claimType)
    {
        return await ContinueWithWorkflow(new { organisationId, allocationId, phaseId, claimType });
    }

    [HttpGet("milestone-date")]
    [WorkflowState(AllocationClaimWorkflowState.MilestoneDate)]
    public async Task<IActionResult> MilestoneDate(
        [FromRoute] string allocationId,
        [FromRoute] string phaseId,
        [FromRoute] MilestoneType claimType,
        CancellationToken cancellationToken)
    {
        return View(await GetClaimModel(allocationId, phaseId, claimType, cancellationToken));
    }

    [HttpPost("milestone-date")]
    [WorkflowState(AllocationClaimWorkflowState.MilestoneDate)]
    public async Task<IActionResult> MilestoneDate(
        [FromRoute] string organisationId,
        [FromRoute] string allocationId,
        [FromRoute] string phaseId,
        [FromRoute] MilestoneType claimType)
    {
        return await ContinueWithWorkflow(new { organisationId, allocationId, phaseId, claimType });
    }

    [HttpGet("confirmation")]
    [WorkflowState(AllocationClaimWorkflowState.Confirmation)]
    public async Task<IActionResult> Confirmation(
        [FromRoute] string allocationId,
        [FromRoute] string phaseId,
        [FromRoute] MilestoneType claimType,
        CancellationToken cancellationToken)
    {
        return View(await GetClaimModel(allocationId, phaseId, claimType, cancellationToken));
    }

    [HttpPost("confirmation")]
    [WorkflowState(AllocationClaimWorkflowState.Confirmation)]
    public async Task<IActionResult> Confirmation(
        [FromRoute] string organisationId,
        [FromRoute] string allocationId,
        [FromRoute] string phaseId,
        [FromRoute] MilestoneType claimType)
    {
        return await ContinueWithWorkflow(new { organisationId, allocationId, phaseId, claimType });
    }

    [HttpGet("check-answers")]
    [WorkflowState(AllocationClaimWorkflowState.CheckAnswers)]
    public async Task<IActionResult> CheckAnswers(
        [FromRoute] string allocationId,
        [FromRoute] string phaseId,
        [FromRoute] MilestoneType claimType,
        CancellationToken cancellationToken)
    {
        return View(await GetClaimModel(allocationId, phaseId, claimType, cancellationToken));
    }

    [HttpPost("check-answers")]
    [WorkflowState(AllocationClaimWorkflowState.CheckAnswers)]
    public IActionResult CheckAnswers(
        [FromRoute] string organisationId,
        [FromRoute] string allocationId,
        [FromRoute] string phaseId)
    {
        return RedirectToAction("Overview", "AllocationClaims", new { organisationId, allocationId, phaseId });
    }

    protected override async Task<IStateRouting<AllocationClaimWorkflowState>> Routing(AllocationClaimWorkflowState currentState, object? routeData = null)
    {
        var allocationId = Request.GetRouteValue("allocationId")
                     ?? routeData?.GetPropertyValue<string>("allocationId")
                     ?? throw new NotFoundException("Cannot find AllocationId");
        var phaseId = Request.GetRouteValue("phaseId")
                           ?? routeData?.GetPropertyValue<string>("phaseId")
                           ?? throw new NotFoundException("Cannot find PhaseId");
        var claimType = Request.GetRouteValue("claimType")
                        ?? routeData?.GetPropertyValue<string>("claimType")
                        ?? throw new NotFoundException("Cannot find ClaimType");

        var phaseClaims = await GetPhaseClaims(allocationId, phaseId, CancellationToken.None);

        return new AllocationClaimWorkflow(currentState, phaseClaims.MilestoneClaims.Single(x => x.Type == Enum.Parse<MilestoneType>(claimType, true)));
    }

    private async Task<Phase> GetPhaseClaims(string allocationId, string phaseId, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetPhaseClaimsQuery(AllocationId.From(allocationId), PhaseId.From(phaseId)), cancellationToken);
    }

    private async Task<PhaseClaimModel> GetClaimModel(
        string allocationId,
        string phaseId,
        MilestoneType claimType,
        CancellationToken cancellationToken)
    {
        var phaseClaims = await GetPhaseClaims(allocationId, phaseId, cancellationToken);

        return new PhaseClaimModel(
            phaseClaims.Id.Value,
            phaseClaims.Name,
            phaseClaims.Allocation,
            phaseClaims.MilestoneClaims.Single(x => x.Type == claimType));
    }
}
