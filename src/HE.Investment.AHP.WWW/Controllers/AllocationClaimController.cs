using HE.Investment.AHP.WWW.Models.AllocationClaim;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Contract.Claims.Commands;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Contract.Claims.Queries;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.WWW.Controllers;
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
        [FromRoute] string allocationId,
        [FromRoute] string phaseId,
        [FromRoute] MilestoneType claimType,
        [FromForm] bool? costsIncurred,
        CancellationToken cancellationToken)
    {
        return await ExecuteClaimCommand(
            new ProvideCostsIncurredCommand(AllocationId.From(allocationId), PhaseId.From(phaseId), claimType, costsIncurred),
            nameof(CostsIncurred),
            cancellationToken);
    }

    [HttpGet("achievement-date")]
    [WorkflowState(AllocationClaimWorkflowState.AchievementDate)]
    public async Task<IActionResult> AchievementDate(
        [FromRoute] string allocationId,
        [FromRoute] string phaseId,
        [FromRoute] MilestoneType claimType,
        CancellationToken cancellationToken)
    {
        return View(await GetClaimModel(allocationId, phaseId, claimType, cancellationToken));
    }

    [HttpPost("achievement-date")]
    [WorkflowState(AllocationClaimWorkflowState.AchievementDate)]
    public async Task<IActionResult> AchievementDate(
        [FromRoute] string allocationId,
        [FromRoute] string phaseId,
        [FromRoute] MilestoneType claimType,
        [FromForm] DateDetails? achievementDate,
        CancellationToken cancellationToken)
    {
        return await ExecuteClaimCommand(
            new ProvideClaimAchievementDateCommand(AllocationId.From(allocationId), PhaseId.From(phaseId), claimType, achievementDate),
            nameof(AchievementDate),
            cancellationToken,
            achievementDate);
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
        [FromRoute] string allocationId,
        [FromRoute] string phaseId,
        [FromRoute] MilestoneType claimType,
        [FromForm] string isConfirmed,
        CancellationToken cancellationToken)
    {
        return await ExecuteClaimCommand(
            new ProvideClaimConfirmationCommand(AllocationId.From(allocationId), PhaseId.From(phaseId), claimType, isConfirmed == "checked"),
            nameof(Confirmation),
            cancellationToken);
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

        var milestoneClaim = phaseClaims.MilestoneClaims.Single(x => x.Type == claimType);

        var milestoneClaimModel = new MilestoneClaimModel(
            milestoneClaim.Type,
            milestoneClaim.Status,
            milestoneClaim.AmountOfGrantApportioned,
            milestoneClaim.PercentageOfGrantApportioned,
            milestoneClaim.ForecastClaimDate,
            milestoneClaim.AchievementDate,
            milestoneClaim.SubmissionDate,
            milestoneClaim.CanBeClaimed,
            milestoneClaim.CostsIncurred,
            milestoneClaim.IsConfirmed);

        return new PhaseClaimModel(
            phaseClaims.Id.Value,
            phaseClaims.Name,
            phaseClaims.Allocation,
            milestoneClaimModel);
    }

    private async Task<IActionResult> ExecuteClaimCommand<TCommand>(
        TCommand command,
        string viewName,
        CancellationToken cancellationToken,
        DateDetails? achievementDate = null)
            where TCommand : IProvideClaimDetailsCommand
    {
        if (Request.IsCancelAndReturnAction())
        {
            await _mediator.Send(new CancelClaimCommand(command.AllocationId, command.PhaseId, command.MilestoneType), cancellationToken);

            return RedirectToAction(
                "Overview",
                "AllocationClaims",
                new { allocationId = command.AllocationId.Value, phaseId = command.PhaseId.Value, organisationId = Request.GetOrganisationIdFromRoute() });
        }

        return await this.ExecuteCommand<TCommand>(
            _mediator,
            command,
            async () => await ContinueWithWorkflow(
                new
                {
                    organisationId = Request.GetOrganisationIdFromRoute()?.Value,
                    allocationId = command.AllocationId.Value,
                    phaseId = command.PhaseId.Value,
                    claimType = command.MilestoneType,
                }),
            async () =>
            {
                var model = await GetClaimModel(command.AllocationId.Value, command.PhaseId.Value, command.MilestoneType, cancellationToken);
                var modelWithError = new PhaseClaimModel(model.Id, model.Name, model.Allocation, model.Claim) { Claim = { AchievementDate = achievementDate } };
                return View(viewName, modelWithError);
            },
            cancellationToken);
    }
}
