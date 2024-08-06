using HE.Investment.AHP.WWW.Extensions;
using HE.Investment.AHP.WWW.Models.AllocationClaim;
using HE.Investment.AHP.WWW.Models.AllocationClaim.Factories;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.AHP.Allocation.Contract;
using HE.Investments.AHP.Allocation.Contract.Claims;
using HE.Investments.AHP.Allocation.Contract.Claims.Commands;
using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.AHP.Allocation.Contract.Claims.Queries;
using HE.Investments.AHP.Allocation.Domain.UserContext;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.WWW.Controllers;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.Consortium.Shared.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[ConsortiumAuthorize(AllocationAccessContext.FulfillClaims)]
[Route("{organisationId}/allocation/{allocationId}/claims/{phaseId}/{claimType}")]
public class AllocationClaimController : WorkflowController<AllocationClaimWorkflowState>
{
    private readonly IMediator _mediator;

    private readonly IAllocationAccessContext _allocationAccessContext;

    private readonly IAllocationClaimCheckAnswersViewModelFactory _allocationClaimCheckAnswersViewModelFactory;

    public AllocationClaimController(
        IMediator mediator,
        IAllocationClaimCheckAnswersViewModelFactory allocationClaimCheckAnswersViewModelFactory,
        IAllocationAccessContext allocationAccessContext)
    {
        _mediator = mediator;
        _allocationAccessContext = allocationAccessContext;
        _allocationClaimCheckAnswersViewModelFactory = allocationClaimCheckAnswersViewModelFactory;
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
    public async Task<IActionResult> ContinueAnswering(
        [FromRoute] string allocationId,
        [FromRoute] string phaseId,
        [FromRoute] MilestoneType claimType)
    {
        var summary = await GetAllocationClaimAndCreateSummary(
            AllocationId.From(allocationId),
            PhaseId.From(phaseId),
            claimType,
            CancellationToken.None);

        return this.ContinueSectionAnswering(
            summary,
            () => this.OrganisationRedirectToAction("CheckAnswers", routeValues: new { allocationId, phaseId, claimType }));
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
            phaseClaim => phaseClaim with { Claim = phaseClaim.Claim with { AchievementDate = achievementDate } });
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
            cancellationToken,
            phaseClaim => phaseClaim with { Claim = phaseClaim.Claim with { IsConfirmed = isConfirmed == "checked" } });
    }

    [HttpGet("check-answers")]
    [WorkflowState(AllocationClaimWorkflowState.CheckAnswers)]
    public async Task<IActionResult> CheckAnswers(
        [FromRoute] string allocationId,
        [FromRoute] string phaseId,
        [FromRoute] MilestoneType claimType,
        CancellationToken cancellationToken)
    {
        return View(await GetAllocationClaimAndCreateSummary(
            AllocationId.From(allocationId),
            PhaseId.From(phaseId),
            claimType,
            cancellationToken));
    }

    [ConsortiumAuthorize(AllocationAccessContext.SubmitClaims)]
    [HttpPost("submit")]
    [WorkflowState(AllocationClaimWorkflowState.CheckAnswers)]
    public async Task<IActionResult> Submit(
        [FromRoute] string allocationId,
        [FromRoute] string phaseId,
        [FromRoute] MilestoneType claimType,
        CancellationToken cancellationToken)
    {
        var command = new SubmitClaimCommand(AllocationId.From(allocationId), PhaseId.From(phaseId), claimType);

        if (Request.IsCancelAndReturnAction())
        {
            return await HandleCancelAndReturnAction(command, cancellationToken);
        }

        return await this.ExecuteCommand<AllocationClaimSummaryViewModel>(
            _mediator,
            command,
            async () => await Continue(
                new
                {
                    organisationId = Request.GetOrganisationIdFromRoute()?.Value,
                    allocationId,
                    phaseId,
                }),
            async () =>
            {
                var summary = await GetAllocationClaimAndCreateSummary(
                    AllocationId.From(allocationId),
                    PhaseId.From(phaseId),
                    claimType,
                    cancellationToken);

                return View(nameof(CheckAnswers), summary);
            },
            cancellationToken);
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

    private async Task<IActionResult> ExecuteClaimCommand<TCommand>(
        TCommand command,
        string viewName,
        CancellationToken cancellationToken,
        Func<PhaseClaimModel, PhaseClaimModel>? createViewModelForError = null)
            where TCommand : IProvideClaimDetailsCommand
    {
        if (Request.IsCancelAndReturnAction())
        {
            return await HandleCancelAndReturnAction(command, cancellationToken);
        }

        return await this.ExecuteCommand<TCommand>(
            _mediator,
            command,
            async () => await ContinueWithRedirect(
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
                var modelWithError = createViewModelForError != null ? createViewModelForError(model) : model;
                return View(viewName, modelWithError);
            },
            cancellationToken);
    }

    private async Task<AllocationClaimSummaryViewModel> GetAllocationClaimAndCreateSummary(
        AllocationId allocationId,
        PhaseId phaseId,
        MilestoneType claimType,
        CancellationToken cancellationToken)
    {
        var phaseClaim = await GetClaimModel(allocationId.Value, phaseId.Value, claimType, cancellationToken);
        var isEditable = phaseClaim.Claim.IsEditable && await _allocationAccessContext.CanEditClaim();
        var claimSection = _allocationClaimCheckAnswersViewModelFactory.CreateSummary(
            allocationId,
            phaseId,
            phaseClaim.Claim,
            Url,
            isEditable);

        return new AllocationClaimSummaryViewModel(
            allocationId,
            phaseId,
            phaseClaim.Allocation.Name,
            claimType,
            [claimSection],
            isEditable,
            await _allocationAccessContext.CanSubmitClaim());
    }

    private async Task<IActionResult> HandleCancelAndReturnAction<TCommand>(TCommand command, CancellationToken cancellationToken)
        where TCommand : IProvideClaimDetailsCommand
    {
        await _mediator.Send(new CancelClaimCommand(command.AllocationId, command.PhaseId, command.MilestoneType), cancellationToken);

        return RedirectToAction(
            "Overview",
            "AllocationClaims",
            new { allocationId = command.AllocationId.Value, phaseId = command.PhaseId.Value, organisationId = Request.GetOrganisationIdFromRoute() });
    }
}
