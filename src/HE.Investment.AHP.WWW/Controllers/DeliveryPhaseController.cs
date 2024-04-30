using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Queries;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Contract.Delivery.MilestonePayments.Commands;
using HE.Investment.AHP.Contract.Delivery.Queries;
using HE.Investment.AHP.WWW.Extensions;
using HE.Investment.AHP.WWW.Models.Delivery;
using HE.Investment.AHP.WWW.Models.Delivery.Factories;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using HE.Investments.Common.Workflow;
using HE.Investments.Common.WWW.Controllers;
using HE.Investments.Common.WWW.Routing;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[AuthorizeWithCompletedProfile]
[Route("application/{applicationId}/delivery-phase")]
public class DeliveryPhaseController : WorkflowController<DeliveryPhaseWorkflowState>
{
    private readonly IMediator _mediator;
    private readonly IDeliveryPhaseCheckAnswersViewModelFactory _deliveryPhaseCheckAnswersViewModelFactory;

    public DeliveryPhaseController(
        IMediator mediator,
        IDeliveryPhaseCheckAnswersViewModelFactory deliveryPhaseCheckAnswersViewModelFactory)
    {
        _mediator = mediator;
        _deliveryPhaseCheckAnswersViewModelFactory = deliveryPhaseCheckAnswersViewModelFactory;
    }

    [HttpGet("{deliveryPhaseId}/back")]
    public async Task<IActionResult> Back([FromRoute] string applicationId, [FromRoute] string deliveryPhaseId, DeliveryPhaseWorkflowState currentPage)
    {
        return await Back(currentPage, new { applicationId, deliveryPhaseId });
    }

    [HttpGet("{deliveryPhaseId}/start")]
    public async Task<IActionResult> Start([FromRoute] string applicationId, [FromRoute] string deliveryPhaseId, CancellationToken cancellationToken)
    {
        var deliveryPhaseSummary = await GetDeliveryPhaseAndCreateSummary(false, cancellationToken);
        return this.ContinueSectionAnswering(deliveryPhaseSummary, () => RedirectToAction("CheckAnswers", new { applicationId, deliveryPhaseId }));
    }

    [HttpGet("create")]
    [WorkflowState(DeliveryPhaseWorkflowState.Create)]
    public async Task<IActionResult> Create([FromRoute] string applicationId)
    {
        var application = await _mediator.Send(new GetApplicationQuery(AhpApplicationId.From(applicationId)));

        var model = new DeliveryPhaseNameViewModel(applicationId, null, application.Name, null, nameof(this.Create));
        return View("Name", model);
    }

    [HttpPost("create")]
    [WorkflowState(DeliveryPhaseWorkflowState.Create)]
    public async Task<IActionResult> Create([FromRoute] string applicationId, DeliveryPhaseNameViewModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new CreateDeliveryPhaseCommand(AhpApplicationId.From(applicationId), model.DeliveryPhaseName),
            cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("Name", model);
        }

        return await this.ReturnToTaskListOrContinue(
            async () => await ContinueWithRedirect(new { applicationId, deliveryPhaseId = result.ReturnedData?.Value }));
    }

    [HttpGet("{deliveryPhaseId}/name")]
    [WorkflowState(DeliveryPhaseWorkflowState.Name)]
    public async Task<IActionResult> Name([FromRoute] string applicationId, [FromRoute] string deliveryPhaseId, CancellationToken cancellationToken)
    {
        var deliveryPhase = await GetDeliveryPhaseDetails(applicationId, deliveryPhaseId, cancellationToken);
        var model = new DeliveryPhaseNameViewModel(applicationId, deliveryPhaseId, deliveryPhase.Application.Name, deliveryPhase.Name, nameof(this.Name));
        return View("Name", model);
    }

    [WorkflowState(DeliveryPhaseWorkflowState.Name)]
    [HttpPost("{deliveryPhaseId}/name")]
    public async Task<IActionResult> Name(
        [FromRoute] string applicationId,
        [FromRoute] string deliveryPhaseId,
        DeliveryPhaseNameViewModel model,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ProvideDeliveryPhaseNameCommand(AhpApplicationId.From(applicationId), new DeliveryPhaseId(deliveryPhaseId), model.DeliveryPhaseName),
            cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("Name", model);
        }

        return await this.ReturnToTaskListOrContinue(
            async () => await ContinueWithRedirect(new { applicationId, deliveryPhaseId }));
    }

    [HttpGet("{deliveryPhaseId}/details")]
    [WorkflowState(DeliveryPhaseWorkflowState.TypeOfHomes)]
    public async Task<IActionResult> Details([FromRoute] string applicationId, string deliveryPhaseId, CancellationToken cancellationToken)
    {
        var deliveryPhaseDetails = await GetDeliveryPhaseDetails(applicationId, deliveryPhaseId, cancellationToken);

        return View("Details", deliveryPhaseDetails);
    }

    [HttpPost("{deliveryPhaseId}/details")]
    [WorkflowState(DeliveryPhaseWorkflowState.TypeOfHomes)]
    public async Task<IActionResult> Details([FromRoute] string applicationId, string deliveryPhaseId, DeliveryPhaseDetails deliveryPhaseDetails, CancellationToken cancellationToken)
    {
        return await ExecuteCommand(
            new ProvideTypeOfHomesCommand(
                AhpApplicationId.From(applicationId),
                new DeliveryPhaseId(deliveryPhaseId),
                deliveryPhaseDetails.TypeOfHomes),
            nameof(Details),
            savedModel => savedModel with { TypeOfHomes = deliveryPhaseDetails.TypeOfHomes },
            cancellationToken);
    }

    [HttpGet("{deliveryPhaseId}/new-build-activity-type")]
    [WorkflowState(DeliveryPhaseWorkflowState.NewBuildActivityType)]
    public async Task<IActionResult> NewBuildActivityType([FromRoute] string applicationId, string deliveryPhaseId, CancellationToken cancellationToken)
    {
        return await GetBuildActivityType(applicationId, deliveryPhaseId, cancellationToken);
    }

    [HttpPost("{deliveryPhaseId}/new-build-activity-type")]
    [WorkflowState(DeliveryPhaseWorkflowState.NewBuildActivityType)]
    public async Task<IActionResult> NewBuildActivityType(
        [FromRoute] string applicationId,
        string deliveryPhaseId,
        DeliveryPhaseDetails deliveryPhaseDetails,
        CancellationToken cancellationToken)
    {
        return await SaveBuildActivityType(applicationId, deliveryPhaseId, deliveryPhaseDetails, cancellationToken);
    }

    [HttpGet("{deliveryPhaseId}/rehab-build-activity-type")]
    [WorkflowState(DeliveryPhaseWorkflowState.RehabBuildActivityType)]
    public async Task<IActionResult> RehabBuildActivityType([FromRoute] string applicationId, string deliveryPhaseId, CancellationToken cancellationToken)
    {
        return await GetBuildActivityType(applicationId, deliveryPhaseId, cancellationToken);
    }

    [HttpPost("{deliveryPhaseId}/rehab-build-activity-type")]
    [WorkflowState(DeliveryPhaseWorkflowState.RehabBuildActivityType)]
    public async Task<IActionResult> RehabBuildActivityType(
        [FromRoute] string applicationId,
        string deliveryPhaseId,
        DeliveryPhaseDetails deliveryPhaseDetails,
        CancellationToken cancellationToken)
    {
        return await SaveBuildActivityType(applicationId, deliveryPhaseId, deliveryPhaseDetails, cancellationToken);
    }

    [HttpGet("{deliveryPhaseId}/add-homes")]
    [WorkflowState(DeliveryPhaseWorkflowState.AddHomes)]
    public async Task<IActionResult> AddHomes([FromRoute] string applicationId, string deliveryPhaseId, CancellationToken cancellationToken)
    {
        return View("AddHomes", await new AddHomesModelFactory(_mediator).Create(applicationId, deliveryPhaseId, cancellationToken));
    }

    [HttpPost("{deliveryPhaseId}/add-homes")]
    [WorkflowState(DeliveryPhaseWorkflowState.AddHomes)]
    public async Task<IActionResult> AddHomes([FromRoute] string applicationId, string deliveryPhaseId, AddHomesModel model, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<AddHomesModel>(
            _mediator,
            new ProvideDeliveryPhaseHomesCommand(
                AhpApplicationId.From(applicationId),
                new DeliveryPhaseId(deliveryPhaseId),
                model.HomesToDeliver ?? new Dictionary<string, string?>()),
            async () => await this.ReturnToTaskListOrContinue(
                async () => await ContinueWithRedirect(new { applicationId, deliveryPhaseId })),
            async () => View("AddHomes", await new AddHomesModelFactory(_mediator).Create(applicationId, deliveryPhaseId, model, cancellationToken)),
            cancellationToken);
    }

    [HttpGet("{deliveryPhaseId}/reconfiguring-existing")]
    [WorkflowState(DeliveryPhaseWorkflowState.ReconfiguringExisting)]
    public async Task<IActionResult> ReconfiguringExisting([FromRoute] string applicationId, string deliveryPhaseId, CancellationToken cancellationToken)
    {
        var deliveryPhaseDetails = await GetDeliveryPhaseDetails(applicationId, deliveryPhaseId, cancellationToken);

        return View("ReconfiguringExisting", deliveryPhaseDetails);
    }

    [HttpPost("{deliveryPhaseId}/reconfiguring-existing")]
    [WorkflowState(DeliveryPhaseWorkflowState.ReconfiguringExisting)]
    public async Task<IActionResult> ReconfiguringExisting(
        [FromRoute] string applicationId,
        string deliveryPhaseId,
        DeliveryPhaseDetails deliveryPhaseDetails,
        CancellationToken cancellationToken)
    {
        return await ExecuteCommand(
            new ProvideReconfiguringExistingCommand(
                AhpApplicationId.From(applicationId),
                new DeliveryPhaseId(deliveryPhaseId),
                deliveryPhaseDetails.ReconfiguringExisting),
            nameof(ReconfiguringExisting),
            savedModel => savedModel with { ReconfiguringExisting = deliveryPhaseDetails.ReconfiguringExisting },
            cancellationToken);
    }

    [HttpGet("{deliveryPhaseId}/summary-of-delivery")]
    [WorkflowState(DeliveryPhaseWorkflowState.SummaryOfDelivery)]
    public async Task<IActionResult> SummaryOfDelivery([FromRoute] string applicationId, string deliveryPhaseId, CancellationToken cancellationToken)
    {
        var deliveryPhaseDetails = await GetDeliveryPhaseDetails(applicationId, deliveryPhaseId, cancellationToken);

        return View("SummaryOfDelivery", deliveryPhaseDetails);
    }

    [HttpPost("{deliveryPhaseId}/summary-of-delivery")]
    [WorkflowState(DeliveryPhaseWorkflowState.SummaryOfDelivery)]
    public async Task<IActionResult> SummaryOfDelivery(
        [FromRoute] string applicationId,
        string deliveryPhaseId,
        DeliveryPhaseDetails model,
        CancellationToken cancellationToken)
    {
        if (model.Tranches?.ShouldBeAmended ?? false)
        {
            return await ExecuteCommand(
                new ClaimMilestonesCommand(
                    AhpApplicationId.From(applicationId),
                    new DeliveryPhaseId(deliveryPhaseId),
                    model.Tranches?.SummaryOfDelivery?.UnderstandClaimingMilestones),
                nameof(SummaryOfDelivery),
                deliveryPhaseDetails => deliveryPhaseDetails,
                cancellationToken);
        }

        return await this.ReturnToTaskListOrContinue(
            async () => await ContinueWithRedirect(new { applicationId, deliveryPhaseId }));
    }

    [HttpGet("{deliveryPhaseId}/summary-of-delivery/tranche/{trancheType}")]
    [WorkflowState(DeliveryPhaseWorkflowState.SummaryOfDeliveryTranche)]
    public async Task<IActionResult> SummaryOfDeliveryAcquisitionTranche(
        [FromRoute] string applicationId,
        [FromRoute] string deliveryPhaseId,
        [FromRoute] SummaryOfDeliveryTrancheType trancheType,
        CancellationToken cancellationToken)
    {
        var deliveryPhaseDetails = await GetDeliveryPhaseDetails(applicationId, deliveryPhaseId, cancellationToken);

        var tranche = trancheType switch
        {
            SummaryOfDeliveryTrancheType.Acquisition => deliveryPhaseDetails.Tranches?.SummaryOfDelivery?.AcquisitionPercentage,
            SummaryOfDeliveryTrancheType.Completion => deliveryPhaseDetails.Tranches?.SummaryOfDelivery?.CompletionPercentage,
            SummaryOfDeliveryTrancheType.StartOnSite => deliveryPhaseDetails.Tranches?.SummaryOfDelivery?.StartOnSitePercentage,
            _ => throw new NotSupportedException(nameof(trancheType)),
        };

        return View(
            "SummaryOfDeliveryTranche",
            new SummaryOfDeliveryTrancheModel(
                trancheType,
                tranche.ToWholePercentage().WithoutPercentageChar(),
                deliveryPhaseDetails.Id,
                deliveryPhaseDetails.Name,
                deliveryPhaseDetails.Application.Name));
    }

    [HttpPost("{deliveryPhaseId}/summary-of-delivery/tranche")]
    [WorkflowState(DeliveryPhaseWorkflowState.SummaryOfDeliveryTranche)]
    public async Task<IActionResult> SummaryOfDeliveryTranche(
        [FromRoute] string applicationId,
        string deliveryPhaseId,
        SummaryOfDeliveryTrancheModel summaryOfDeliveryAmend,
        CancellationToken cancellationToken)
    {
        var ahpApplicationId = AhpApplicationId.From(applicationId);
        var deliveryId = new DeliveryPhaseId(deliveryPhaseId);
        Request.TryGetWorkflowQueryParameter(out var workflow);

        IRequest<OperationResult> command = summaryOfDeliveryAmend.TrancheType switch
        {
            SummaryOfDeliveryTrancheType.Acquisition => new ProvideAcquisitionTrancheCommand(
                ahpApplicationId,
                deliveryId,
                summaryOfDeliveryAmend.Value),
            SummaryOfDeliveryTrancheType.Completion => new ProvideCompletionTrancheCommand(
                ahpApplicationId,
                deliveryId,
                summaryOfDeliveryAmend.Value),
            SummaryOfDeliveryTrancheType.StartOnSite => new ProvideStartOnSiteTrancheCommand(
                ahpApplicationId,
                deliveryId,
                summaryOfDeliveryAmend.Value),
            _ => throw new NotSupportedException(nameof(summaryOfDeliveryAmend.TrancheType)),
        };

        var result = await _mediator.Send(command, cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("SummaryOfDeliveryTranche", summaryOfDeliveryAmend);
        }

        return RedirectToAction("SummaryOfDelivery", new { applicationId, deliveryPhaseId, workflow });
    }

    [HttpGet("{deliveryPhaseId}/remove")]
    public async Task<IActionResult> Remove([FromRoute] string applicationId, string deliveryPhaseId, CancellationToken cancellationToken)
    {
        var deliveryPhaseDetails = await GetDeliveryPhaseDetails(applicationId, deliveryPhaseId, cancellationToken);

        return View("RemoveDeliveryPhaseConfirmation", new RemoveDeliveryPhaseModel(deliveryPhaseDetails.Application.Name, deliveryPhaseDetails.Name));
    }

    [HttpPost("{deliveryPhaseId}/remove")]
    public async Task<IActionResult> Remove(
        [FromRoute] string applicationId,
        string deliveryPhaseId,
        RemoveDeliveryPhaseModel model,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new RemoveDeliveryPhaseCommand(AhpApplicationId.From(applicationId), new DeliveryPhaseId(deliveryPhaseId), model.RemoveDeliveryPhaseAnswer),
            cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("RemoveDeliveryPhaseConfirmation", model);
        }

        return RedirectToAction("List", "Delivery", new { applicationId });
    }

    [WorkflowState(DeliveryPhaseWorkflowState.AcquisitionMilestone)]
    [HttpGet("{deliveryPhaseId}/acquisition-milestone")]
    public async Task<IActionResult> AcquisitionMilestone([FromRoute] string applicationId, string deliveryPhaseId, CancellationToken cancellationToken)
    {
        var deliveryPhaseDetails = await GetDeliveryPhaseDetails(applicationId, deliveryPhaseId, cancellationToken);

        return View(CreateMilestoneViewModel(
            deliveryPhaseDetails.Application.Name,
            deliveryPhaseDetails.Name,
            deliveryPhaseDetails.AcquisitionDate,
            deliveryPhaseDetails.AcquisitionPaymentDate));
    }

    [WorkflowState(DeliveryPhaseWorkflowState.AcquisitionMilestone)]
    [HttpPost("{deliveryPhaseId}/acquisition-milestone")]
    public async Task<IActionResult> AcquisitionMilestone(string deliveryPhaseId, MilestoneViewModel model, CancellationToken cancellationToken)
    {
        return await ExecuteMilestoneCommand(
            new ProvideAcquisitionMilestoneDetailsCommand(
                this.GetApplicationIdFromRoute(),
                new DeliveryPhaseId(deliveryPhaseId),
                model.MilestoneStartAt,
                model.ClaimMilestonePaymentAt),
            nameof(AcquisitionMilestone),
            model,
            cancellationToken);
    }

    [WorkflowState(DeliveryPhaseWorkflowState.StartOnSiteMilestone)]
    [HttpGet("{deliveryPhaseId}/start-on-site-milestone")]
    public async Task<IActionResult> StartOnSiteMilestone([FromRoute] string applicationId, string deliveryPhaseId, CancellationToken cancellationToken)
    {
        var deliveryPhaseDetails = await GetDeliveryPhaseDetails(applicationId, deliveryPhaseId, cancellationToken);

        return View(CreateMilestoneViewModel(
            deliveryPhaseDetails.Application.Name,
            deliveryPhaseDetails.Name,
            deliveryPhaseDetails.StartOnSiteDate,
            deliveryPhaseDetails.StartOnSitePaymentDate));
    }

    [WorkflowState(DeliveryPhaseWorkflowState.StartOnSiteMilestone)]
    [HttpPost("{deliveryPhaseId}/start-on-site-milestone")]
    public async Task<IActionResult> StartOnSiteMilestone(string deliveryPhaseId, MilestoneViewModel model, CancellationToken cancellationToken)
    {
        return await ExecuteMilestoneCommand(
            new ProvideStartOnSiteMilestoneDetailsCommand(
                this.GetApplicationIdFromRoute(),
                new DeliveryPhaseId(deliveryPhaseId),
                model.MilestoneStartAt,
                model.ClaimMilestonePaymentAt),
            nameof(StartOnSiteMilestone),
            model,
            cancellationToken);
    }

    [WorkflowState(DeliveryPhaseWorkflowState.PracticalCompletionMilestone)]
    [HttpGet("{deliveryPhaseId}/practical-completion-milestone")]
    public async Task<IActionResult> PracticalCompletionMilestone([FromRoute] string applicationId, string deliveryPhaseId, CancellationToken cancellationToken)
    {
        var deliveryPhaseDetails = await GetDeliveryPhaseDetails(applicationId, deliveryPhaseId, cancellationToken);

        return View(CreateMilestoneViewModel(
            deliveryPhaseDetails.Application.Name,
            deliveryPhaseDetails.Name,
            deliveryPhaseDetails.PracticalCompletionDate,
            deliveryPhaseDetails.PracticalCompletionPaymentDate));
    }

    [WorkflowState(DeliveryPhaseWorkflowState.PracticalCompletionMilestone)]
    [HttpPost("{deliveryPhaseId}/practical-completion-milestone")]
    public async Task<IActionResult> PracticalCompletionMilestone(string deliveryPhaseId, MilestoneViewModel model, CancellationToken cancellationToken)
    {
        return await ExecuteMilestoneCommand(
            new ProvideCompletionMilestoneDetailsCommand(
                this.GetApplicationIdFromRoute(),
                new DeliveryPhaseId(deliveryPhaseId),
                model.MilestoneStartAt,
                model.ClaimMilestonePaymentAt),
            nameof(PracticalCompletionMilestone),
            model,
            cancellationToken);
    }

    [WorkflowState(DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp)]
    [HttpGet("{deliveryPhaseId}/unregistered-body-follow-up")]
    public async Task<IActionResult> UnregisteredBodyFollowUp([FromRoute] string applicationId, string deliveryPhaseId, CancellationToken cancellationToken)
    {
        var deliveryPhaseDetails = await GetDeliveryPhaseDetails(applicationId, deliveryPhaseId, cancellationToken);

        return View(new DeliveryRequestAdditionalPaymentViewModel(
            applicationId,
            deliveryPhaseDetails.Application.Name,
            deliveryPhaseDetails.Name,
            deliveryPhaseDetails.IsAdditionalPaymentRequested));
    }

    [WorkflowState(DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp)]
    [HttpPost("{deliveryPhaseId}/unregistered-body-follow-up")]
    public async Task<IActionResult> UnregisteredBodyFollowUp(
        [FromRoute] string applicationId,
        string deliveryPhaseId,
        bool? isAdditionalPaymentRequested,
        CancellationToken cancellationToken)
    {
        return await ExecuteCommand(
            new ProvideAdditionalPaymentRequestCommand(
                this.GetApplicationIdFromRoute(),
                new DeliveryPhaseId(deliveryPhaseId),
                isAdditionalPaymentRequested),
            nameof(UnregisteredBodyFollowUp),
            savedModel => new DeliveryRequestAdditionalPaymentViewModel(
                applicationId,
                savedModel.Application.Name,
                savedModel.Name,
                isAdditionalPaymentRequested),
            cancellationToken);
    }

    [WorkflowState(DeliveryPhaseWorkflowState.CheckAnswers)]
    [HttpGet("{deliveryPhaseId}/check-answers")]
    public async Task<IActionResult> CheckAnswers(CancellationToken cancellationToken)
    {
        return await DisplayChecksAnswersPage(cancellationToken);
    }

    [WorkflowState(DeliveryPhaseWorkflowState.CheckAnswers)]
    [HttpPost("{deliveryPhaseId}/check-answers")]
    public async Task<IActionResult> Complete([FromRoute] string applicationId, string deliveryPhaseId, [FromForm] IsSectionCompleted? isCompleted, CancellationToken cancellationToken)
    {
        if (isCompleted == null)
        {
            return await DisplayChecksAnswersPage(cancellationToken, new List<string> { "Select whether you have completed this section" });
        }

        var result = isCompleted == IsSectionCompleted.Yes
            ? await _mediator.Send(new CompleteDeliveryPhaseCommand(AhpApplicationId.From(applicationId), new DeliveryPhaseId(deliveryPhaseId)), cancellationToken)
            : await _mediator.Send(new UnCompleteDeliveryPhaseCommand(AhpApplicationId.From(applicationId), new DeliveryPhaseId(deliveryPhaseId)), cancellationToken);

        if (result.HasValidationErrors)
        {
            return await DisplayChecksAnswersPage(cancellationToken, result.Errors.Select(e => e.ErrorMessage));
        }

        return RedirectToAction("List", "Delivery", new { applicationId });
    }

    protected override async Task<IStateRouting<DeliveryPhaseWorkflowState>> Routing(DeliveryPhaseWorkflowState currentState, object? routeData = null)
    {
        var applicationId = this.GetApplicationIdFromRoute();
        var deliveryPhase = currentState != DeliveryPhaseWorkflowState.Create
            ? await _mediator.Send(new GetDeliveryPhaseDetailsQuery(applicationId, this.GetDeliveryPhaseIdFromRoute()), CancellationToken.None)
            : new DeliveryPhaseDetails(ToApplicationDetails(await _mediator.Send(new GetApplicationQuery(applicationId), CancellationToken.None)), string.Empty, string.Empty, SectionStatus.NotStarted);

        var workflow = new DeliveryPhaseWorkflow(currentState, deliveryPhase, deliveryPhase.Application.IsReadOnly);
        if (Request.TryGetWorkflowQueryParameter(out var lastEncodedWorkflow))
        {
            var lastWorkflow = new EncodedWorkflow<DeliveryPhaseWorkflowState>(lastEncodedWorkflow);
            var currentWorkflow = workflow.GetEncodedWorkflow();
            var changedState = currentWorkflow.GetNextChangedWorkflowState(currentState, lastWorkflow);

            return new DeliveryPhaseWorkflow(changedState, deliveryPhase, true);
        }

        return workflow;
    }

    private static ApplicationDetails ToApplicationDetails(Application application)
    {
        return new ApplicationDetails(application.Id, application.Name, application.Tenure, application.Status, application.AllowedOperations);
    }

    private async Task<IActionResult> GetBuildActivityType(
        string applicationId,
        string deliveryPhaseId,
        CancellationToken cancellationToken)
    {
        var deliveryPhaseDetails = await GetDeliveryPhaseDetails(applicationId, deliveryPhaseId, cancellationToken);

        return View("BuildActivityType", deliveryPhaseDetails);
    }

    private async Task<IActionResult> SaveBuildActivityType(
        string applicationId,
        string deliveryPhaseId,
        DeliveryPhaseDetails deliveryPhaseDetails,
        CancellationToken cancellationToken)
    {
        return await ExecuteCommand(
            new ProvideBuildActivityCommand(
                AhpApplicationId.From(applicationId),
                new DeliveryPhaseId(deliveryPhaseId),
                deliveryPhaseDetails.BuildActivityType),
            "BuildActivityType",
            savedModel => savedModel with { BuildActivityType = deliveryPhaseDetails.BuildActivityType },
            cancellationToken);
    }

    private async Task<IActionResult> DisplayChecksAnswersPage(CancellationToken cancellationToken, IEnumerable<string>? errorMessages = null)
    {
        foreach (var errorMessage in errorMessages ?? Array.Empty<string>())
        {
            ModelState.AddModelError(nameof(DeliveryPhaseSummaryViewModel.IsCompleted), errorMessage);
        }

        return View("CheckAnswers", await GetDeliveryPhaseAndCreateSummary(true, cancellationToken));
    }

    private async Task<DeliveryPhaseSummaryViewModel> GetDeliveryPhaseAndCreateSummary(bool useWorkflowRedirection, CancellationToken cancellationToken)
    {
        var applicationId = this.GetApplicationIdFromRoute();
        var deliveryPhaseId = this.GetDeliveryPhaseIdFromRoute();
        var deliveryPhaseDetails = await _mediator.Send(new GetDeliveryPhaseDetailsQuery(applicationId, deliveryPhaseId), cancellationToken);
        var deliveryPhaseHomes = await _mediator.Send(new GetDeliveryPhaseHomesQuery(applicationId, deliveryPhaseId), cancellationToken);
        var sections = _deliveryPhaseCheckAnswersViewModelFactory.CreateSummary(
            applicationId,
            deliveryPhaseDetails,
            deliveryPhaseHomes,
            Url,
            useWorkflowRedirection);

        return new DeliveryPhaseSummaryViewModel(
            applicationId.Value,
            deliveryPhaseDetails.Id,
            deliveryPhaseDetails.Application.Name,
            deliveryPhaseDetails.Name,
            deliveryPhaseDetails.Status == SectionStatus.Completed ? IsSectionCompleted.Yes : null,
            sections,
            deliveryPhaseDetails.Application.AllowedOperations);
    }

    private MilestoneViewModel CreateMilestoneViewModel(
        string applicationName,
        string? deliveryPhaseName,
        DateDetails? milestoneDate,
        DateDetails? milestonePaymentDate)
    {
        return new MilestoneViewModel(
            this.GetApplicationIdFromRoute().Value,
            applicationName,
            deliveryPhaseName,
            milestoneDate ?? DateDetails.Empty(),
            milestonePaymentDate ?? DateDetails.Empty());
    }

    private async Task<IActionResult> ExecuteMilestoneCommand(
        IRequest<OperationResult> command,
        string viewName,
        MilestoneViewModel modelWithError,
        CancellationToken cancellationToken)
    {
        return await ExecuteCommand(
            command,
            viewName,
            savedModel => CreateMilestoneViewModel(
                savedModel.Application.Name,
                savedModel.Name,
                modelWithError.MilestoneStartAt,
                modelWithError.ClaimMilestonePaymentAt),
            cancellationToken);
    }

    private async Task<IActionResult> ExecuteCommand<TViewModel>(
        IRequest<OperationResult> command,
        string viewName,
        Func<DeliveryPhaseDetails, TViewModel> createViewModelForError,
        CancellationToken cancellationToken)
    {
        var applicationId = this.GetApplicationIdFromRoute();
        var deliveryPhaseId = this.GetDeliveryPhaseIdFromRoute();
        Request.TryGetWorkflowQueryParameter(out var workflow);

        return await this.ExecuteCommand<TViewModel>(
            _mediator,
            command,
            async () => await this.ReturnToTaskListOrContinue(async () => await Continue(new { applicationId = applicationId.Value, deliveryPhaseId, workflow })),
            async () =>
            {
                var deliveryPhaseDetails = await _mediator.Send(
                    new GetDeliveryPhaseDetailsQuery(applicationId, deliveryPhaseId),
                    cancellationToken);
                var model = createViewModelForError(deliveryPhaseDetails);

                return View(viewName, model);
            },
            cancellationToken);
    }

    private async Task<DeliveryPhaseDetails> GetDeliveryPhaseDetails(string applicationId, string deliveryPhaseId, CancellationToken cancellationToken)
    {
        return await _mediator.Send(
            new GetDeliveryPhaseDetailsQuery(AhpApplicationId.From(applicationId), new DeliveryPhaseId(deliveryPhaseId)),
            cancellationToken);
    }
}
