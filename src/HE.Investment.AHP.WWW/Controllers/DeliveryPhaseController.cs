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
using HE.Investment.AHP.WWW.Utils;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;
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
    private readonly IDeliveryPhaseProvider _deliveryPhaseProvider;
    private readonly IDeliveryPhaseSummaryViewModelFactory _deliveryPhaseSummaryViewModelFactory;
    private readonly IAccountAccessContext _accountAccessContext;

    public DeliveryPhaseController(
        IMediator mediator,
        IDeliveryPhaseProvider deliveryPhaseProvider,
        IDeliveryPhaseSummaryViewModelFactory deliveryPhaseSummaryViewModelFactory,
        IAccountAccessContext accountAccessContext)
    {
        _mediator = mediator;
        _deliveryPhaseProvider = deliveryPhaseProvider;
        _deliveryPhaseSummaryViewModelFactory = deliveryPhaseSummaryViewModelFactory;
        _accountAccessContext = accountAccessContext;
    }

    [HttpGet("{deliveryPhaseId}/back")]
    public async Task<IActionResult> Back([FromRoute] string applicationId, [FromRoute] string deliveryPhaseId, DeliveryPhaseWorkflowState currentPage)
    {
        return await Back(currentPage, new { applicationId, deliveryPhaseId });
    }

    [HttpGet("{deliveryPhaseId}/start")]
    [WorkflowState(DeliveryPhaseWorkflowState.Start)]
    public async Task<IActionResult> Start([FromRoute] string applicationId, [FromRoute] string deliveryPhaseId)
    {
        var application = await _mediator.Send(new GetApplicationQuery(AhpApplicationId.From(applicationId)));

        var model = new DeliveryPhaseNameViewModel(applicationId, null, application.Name, null, nameof(this.Create));
        return View("Name", model);
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

        return await ContinueWithAllRedirects(new { applicationId, deliveryPhaseId = result.ReturnedData?.Value });
    }

    [HttpGet("{deliveryPhaseId}/name")]
    [WorkflowState(DeliveryPhaseWorkflowState.Name)]
    public async Task<IActionResult> Name([FromRoute] string applicationId, [FromRoute] string deliveryPhaseId, CancellationToken cancellationToken)
    {
        var deliveryPhase = await _mediator.Send(
            new GetDeliveryPhaseDetailsQuery(AhpApplicationId.From(applicationId), new DeliveryPhaseId(deliveryPhaseId)),
            cancellationToken);
        var model = new DeliveryPhaseNameViewModel(applicationId, deliveryPhaseId, deliveryPhase.ApplicationName, deliveryPhase.Name, nameof(this.Name));
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

        return await ContinueWithAllRedirects(new { applicationId, deliveryPhaseId });
    }

    [HttpGet("{deliveryPhaseId}/details")]
    [WorkflowState(DeliveryPhaseWorkflowState.TypeOfHomes)]
    public async Task<IActionResult> Details([FromRoute] string applicationId, string deliveryPhaseId, CancellationToken cancellationToken)
    {
        var deliveryPhaseDetails =
            await _mediator.Send(
                new GetDeliveryPhaseDetailsQuery(AhpApplicationId.From(applicationId), new DeliveryPhaseId(deliveryPhaseId)),
                cancellationToken);

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

    [HttpGet("{deliveryPhaseId}/build-activity-type")]
    [WorkflowState(DeliveryPhaseWorkflowState.BuildActivityType)]
    public async Task<IActionResult> BuildActivityType([FromRoute] string applicationId, string deliveryPhaseId, CancellationToken cancellationToken)
    {
        var deliveryPhaseDetails =
            await _mediator.Send(
                new GetDeliveryPhaseDetailsQuery(AhpApplicationId.From(applicationId), new DeliveryPhaseId(deliveryPhaseId)),
                cancellationToken);

        return View("BuildActivityType", deliveryPhaseDetails);
    }

    [HttpPost("{deliveryPhaseId}/build-activity-type")]
    [WorkflowState(DeliveryPhaseWorkflowState.BuildActivityType)]
    public async Task<IActionResult> BuildActivityType(
        [FromRoute] string applicationId,
        string deliveryPhaseId,
        DeliveryPhaseDetails deliveryPhaseDetails,
        CancellationToken cancellationToken)
    {
        return await ExecuteCommand(
            new ProvideBuildActivityCommand(
                AhpApplicationId.From(applicationId),
                new DeliveryPhaseId(deliveryPhaseId),
                deliveryPhaseDetails.BuildActivityType),
            nameof(BuildActivityType),
            savedModel => savedModel with { BuildActivityType = deliveryPhaseDetails.BuildActivityType },
            cancellationToken);
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
            () => ContinueWithAllRedirects(new { applicationId, deliveryPhaseId }),
            async () => View("AddHomes", await new AddHomesModelFactory(_mediator).Create(applicationId, deliveryPhaseId, model, cancellationToken)),
            cancellationToken);
    }

    [HttpGet("{deliveryPhaseId}/reconfiguring-existing")]
    [WorkflowState(DeliveryPhaseWorkflowState.ReconfiguringExisting)]
    public async Task<IActionResult> ReconfiguringExisting([FromRoute] string applicationId, string deliveryPhaseId, CancellationToken cancellationToken)
    {
        var deliveryPhaseDetails =
            await _mediator.Send(
                new GetDeliveryPhaseDetailsQuery(AhpApplicationId.From(applicationId), new DeliveryPhaseId(deliveryPhaseId)),
                cancellationToken);

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
        var deliveryPhaseDetails =
            await _deliveryPhaseProvider.Get(
                new GetDeliveryPhaseDetailsQuery(AhpApplicationId.From(applicationId), new DeliveryPhaseId(deliveryPhaseId), true),
                cancellationToken);

        return View("SummaryOfDelivery", deliveryPhaseDetails);
    }

    [HttpPost("{deliveryPhaseId}/summary-of-delivery")]
    [WorkflowState(DeliveryPhaseWorkflowState.SummaryOfDelivery)]
    public async Task<IActionResult> SummaryOfDelivery([FromRoute] string applicationId, string deliveryPhaseId)
    {
        return await ContinueWithAllRedirects(new { applicationId, deliveryPhaseId });
    }

    [HttpGet("{deliveryPhaseId}/summary-of-delivery-editable")]
    [WorkflowState(DeliveryPhaseWorkflowState.SummaryOfDeliveryEditable)]
    public async Task<IActionResult> SummaryOfDeliveryEditable([FromRoute] string applicationId, string deliveryPhaseId, CancellationToken cancellationToken)
    {
        var deliveryPhaseDetails =
            await _deliveryPhaseProvider.Get(
                new GetDeliveryPhaseDetailsQuery(AhpApplicationId.From(applicationId), new DeliveryPhaseId(deliveryPhaseId), true),
                cancellationToken);

        return View("SummaryOfDeliveryEditable", deliveryPhaseDetails);
    }

    [HttpGet("{deliveryPhaseId}/summary-of-delivery/tranche/{trancheType}")]
    [WorkflowState(DeliveryPhaseWorkflowState.SummaryOfDeliveryTranche)]
    public async Task<IActionResult> SummaryOfDeliveryAcquisitionTranche(
        [FromRoute] string applicationId,
        [FromRoute] string deliveryPhaseId,
        [FromRoute] SummaryOfDeliveryTrancheType trancheType,
        CancellationToken cancellationToken)
    {
        var deliveryPhaseDetails =
            await _deliveryPhaseProvider.Get(
                new GetDeliveryPhaseDetailsQuery(AhpApplicationId.From(applicationId), new DeliveryPhaseId(deliveryPhaseId), true),
                cancellationToken);

        var tranche = trancheType switch
        {
            SummaryOfDeliveryTrancheType.Acquisition => deliveryPhaseDetails.SummaryOfDeliveryAmend?.AcquisitionMilestone,
            SummaryOfDeliveryTrancheType.Completion => deliveryPhaseDetails.SummaryOfDeliveryAmend?.CompletionMilestone,
            SummaryOfDeliveryTrancheType.StartOnSite => deliveryPhaseDetails.SummaryOfDeliveryAmend?.StartOnSiteMilestone,
            _ => throw new NotSupportedException(nameof(trancheType)),
        };

        return View(
            "SummaryOfDeliveryTranche",
            new SummaryOfDeliveryTrancheModel(
                trancheType,
                tranche,
                deliveryPhaseDetails.Id,
                deliveryPhaseDetails.Name,
                deliveryPhaseDetails.ApplicationName));
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

        return await ExecuteCommand(command, nameof(SummaryOfDeliveryTranche), _ => summaryOfDeliveryAmend, cancellationToken);
    }

    [HttpGet("{deliveryPhaseId}/remove")]
    public async Task<IActionResult> Remove([FromRoute] string applicationId, string deliveryPhaseId, CancellationToken cancellationToken)
    {
        var deliveryPhaseDetails =
            await _deliveryPhaseProvider.Get(
                new GetDeliveryPhaseDetailsQuery(AhpApplicationId.From(applicationId), new DeliveryPhaseId(deliveryPhaseId)),
                cancellationToken);

        return View("RemoveDeliveryPhaseConfirmation", new RemoveDeliveryPhaseModel(deliveryPhaseDetails.ApplicationName, deliveryPhaseDetails.Name));
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
        var deliveryPhaseDetails =
            await _deliveryPhaseProvider.Get(
                new GetDeliveryPhaseDetailsQuery(AhpApplicationId.From(applicationId), new DeliveryPhaseId(deliveryPhaseId)),
                cancellationToken);

        return View(CreateMilestoneViewModel(
            deliveryPhaseDetails.ApplicationName,
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
        var deliveryPhaseDetails =
            await _deliveryPhaseProvider.Get(
                new GetDeliveryPhaseDetailsQuery(AhpApplicationId.From(applicationId), new DeliveryPhaseId(deliveryPhaseId)),
                cancellationToken);

        return View(CreateMilestoneViewModel(
            deliveryPhaseDetails.ApplicationName,
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
        var deliveryPhaseDetails =
            await _deliveryPhaseProvider.Get(
                new GetDeliveryPhaseDetailsQuery(AhpApplicationId.From(applicationId), new DeliveryPhaseId(deliveryPhaseId)),
                cancellationToken);

        return View(CreateMilestoneViewModel(
            deliveryPhaseDetails.ApplicationName,
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
        var deliveryPhaseDetails =
            await _deliveryPhaseProvider.Get(
                new GetDeliveryPhaseDetailsQuery(AhpApplicationId.From(applicationId), new DeliveryPhaseId(deliveryPhaseId)),
                cancellationToken);

        return View(new DeliveryRequestAdditionalPaymentViewModel(
            applicationId,
            deliveryPhaseDetails.ApplicationName,
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
                savedModel.ApplicationName,
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

        return await Task.FromResult(RedirectToAction("List", "Delivery", new { applicationId }));
    }

    protected override async Task<IStateRouting<DeliveryPhaseWorkflowState>> Routing(DeliveryPhaseWorkflowState currentState, object? routeData = null)
    {
        var deliveryPhase = currentState != DeliveryPhaseWorkflowState.Create
            ? await _deliveryPhaseProvider.Get(
                new GetDeliveryPhaseDetailsQuery(this.GetApplicationIdFromRoute(), this.GetDeliveryPhaseIdFromRoute()), CancellationToken.None)
            : new DeliveryPhaseDetails(string.Empty, string.Empty, string.Empty, SectionStatus.NotStarted, false);

        var isReadOnly = !await _accountAccessContext.CanEditApplication() || deliveryPhase.IsReadOnly;

        return new DeliveryPhaseWorkflow(currentState, deliveryPhase, isReadOnly);
    }

    private async Task<IActionResult> DisplayChecksAnswersPage(CancellationToken cancellationToken, IEnumerable<string>? errorMessages = null)
    {
        foreach (var errorMessage in errorMessages ?? Array.Empty<string>())
        {
            ModelState.AddModelError(nameof(DeliveryPhaseSummaryViewModel.IsCompleted), errorMessage);
        }

        return View("CheckAnswers", await GetDeliveryPhaseAndCreateSummary(cancellationToken));
    }

    private async Task<DeliveryPhaseSummaryViewModel> GetDeliveryPhaseAndCreateSummary(CancellationToken cancellationToken)
    {
        var applicationId = this.GetApplicationIdFromRoute();
        var deliveryPhaseId = this.GetDeliveryPhaseIdFromRoute();
        var deliveryPhaseDetails = await _deliveryPhaseProvider.Get(new GetDeliveryPhaseDetailsQuery(applicationId, deliveryPhaseId, true), cancellationToken);
        var deliveryPhaseHomes = await _mediator.Send(new GetDeliveryPhaseHomesQuery(applicationId, deliveryPhaseId), cancellationToken);
        var isEditable = await _accountAccessContext.CanEditApplication() && !deliveryPhaseDetails.IsReadOnly;
        var sections = _deliveryPhaseSummaryViewModelFactory.CreateSummary(applicationId, deliveryPhaseDetails, deliveryPhaseHomes, Url, isEditable);

        return new DeliveryPhaseSummaryViewModel(
            applicationId.Value,
            deliveryPhaseDetails.Id,
            deliveryPhaseDetails.ApplicationName,
            deliveryPhaseDetails.Name,
            deliveryPhaseDetails.Status == SectionStatus.Completed ? IsSectionCompleted.Yes : null,
            sections,
            isEditable);
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
                savedModel.ApplicationName,
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

        return await this.ExecuteCommand<TViewModel>(
            _mediator,
            command,
            async () => await ContinueWithAllRedirects(new { applicationId = applicationId.Value, deliveryPhaseId }),
            async () =>
            {
                var deliveryPhaseDetails =
                    await _deliveryPhaseProvider.Get(new GetDeliveryPhaseDetailsQuery(applicationId, deliveryPhaseId), cancellationToken);
                var model = createViewModelForError(deliveryPhaseDetails);

                return View(viewName, model);
            },
            cancellationToken);
    }

    private async Task<IActionResult> ContinueWithAllRedirects(object routeData)
    {
        var action = HttpContext.Request.Form["action"];
        var applicationId = this.GetApplicationIdFromRoute();

        if (action == GenericMessages.SaveAndReturn)
        {
            return Url.RedirectToTaskList(applicationId.Value);
        }

        return await ContinueWithRedirect(routeData);
    }
}
