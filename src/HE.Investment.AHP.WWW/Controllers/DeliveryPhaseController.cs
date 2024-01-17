using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Application.Queries;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Contract.Delivery.Queries;
using HE.Investment.AHP.WWW.Extensions;
using HE.Investment.AHP.WWW.Models.Delivery;
using HE.Investment.AHP.WWW.Models.Delivery.Factories;
using HE.Investment.AHP.WWW.Utils;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Validators;
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

    public DeliveryPhaseController(IMediator mediator, IDeliveryPhaseProvider deliveryPhaseProvider)
    {
        _mediator = mediator;
        _deliveryPhaseProvider = deliveryPhaseProvider;
    }

    [HttpGet("{deliveryPhaseId}/back")]
    public async Task<IActionResult> Back([FromRoute] string applicationId, [FromRoute] string deliveryPhaseId, DeliveryPhaseWorkflowState currentPage)
    {
        return await Back(currentPage, new { applicationId, deliveryPhaseId });
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
            new CreateDeliveryPhaseCommand(AhpApplicationId.From(applicationId), model.DeliveryPhaseName), cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("Name", model);
        }

        return await Continue(new { applicationId, deliveryPhaseId = result.ReturnedData?.Value });
    }

    [HttpGet("{deliveryPhaseId}/name")]
    [WorkflowState(DeliveryPhaseWorkflowState.Name)]
    public async Task<IActionResult> Name([FromRoute] string applicationId, [FromRoute] string deliveryPhaseId, CancellationToken cancellationToken)
    {
        var deliveryPhase = await _mediator.Send(
                new GetDeliveryPhaseDetailsQuery(AhpApplicationId.From(applicationId), new DeliveryPhaseId(deliveryPhaseId)), cancellationToken);
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

        return await Continue(new { applicationId, deliveryPhaseId });
    }

    [HttpGet("{deliveryPhaseId}/details")]
    [WorkflowState(DeliveryPhaseWorkflowState.TypeOfHomes)]
    public async Task<IActionResult> Details([FromRoute] string applicationId, string deliveryPhaseId, CancellationToken cancellationToken)
    {
        var deliveryPhaseDetails =
            await _mediator.Send(
                new GetDeliveryPhaseDetailsQuery(AhpApplicationId.From(applicationId), new DeliveryPhaseId(deliveryPhaseId)), cancellationToken);

        return View("Details", deliveryPhaseDetails);
    }

    [HttpPost("{deliveryPhaseId}/details")]
    [WorkflowState(DeliveryPhaseWorkflowState.TypeOfHomes)]
    public async Task<IActionResult> Details(
        [FromRoute] string applicationId,
        string deliveryPhaseId,
        DeliveryPhaseDetails deliveryPhaseDetails,
        CancellationToken cancellationToken)
    {
        return await ExecuteCommand(
            deliveryPhaseId,
            new ProvideTypeOfHomesCommand(AhpApplicationId.From(applicationId), new DeliveryPhaseId(deliveryPhaseId), deliveryPhaseDetails.TypeOfHomes),
            nameof(Details),
            cancellationToken);
    }

    [HttpGet("{deliveryPhaseId}/build-activity-type")]
    [WorkflowState(DeliveryPhaseWorkflowState.BuildActivityType)]
    public async Task<IActionResult> BuildActivityType([FromRoute] string applicationId, string deliveryPhaseId, CancellationToken cancellationToken)
    {
        var deliveryPhaseDetails =
            await _mediator.Send(
                new GetDeliveryPhaseDetailsQuery(AhpApplicationId.From(applicationId), new DeliveryPhaseId(deliveryPhaseId)), cancellationToken);

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
            deliveryPhaseId,
            new ProvideBuildActivityCommand(AhpApplicationId.From(applicationId), new DeliveryPhaseId(deliveryPhaseId), deliveryPhaseDetails.BuildActivityType),
            nameof(BuildActivityType),
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
        return await this.ExecuteCommand(
            _mediator,
            new ProvideDeliveryPhaseHomesCommand(AhpApplicationId.From(applicationId), new DeliveryPhaseId(deliveryPhaseId)),
            () => ContinueWithRedirect(new { applicationId, deliveryPhaseId }),
            async () => View("AddHomes", await new AddHomesModelFactory(_mediator).Create(applicationId, deliveryPhaseId, cancellationToken)),
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
            deliveryPhaseId,
            new ProvideReconfiguringExistingCommand(AhpApplicationId.From(applicationId), new DeliveryPhaseId(deliveryPhaseId), deliveryPhaseDetails.ReconfiguringExisting),
            nameof(ReconfiguringExisting),
            cancellationToken);
    }

    [HttpGet("{deliveryPhaseId}/remove")]
    public async Task<IActionResult> Remove([FromRoute] string applicationId, string deliveryPhaseId, CancellationToken cancellationToken)
    {
        var deliveryPhaseDetails =
            await _deliveryPhaseProvider.Get(
                new GetDeliveryPhaseDetailsQuery(AhpApplicationId.From(applicationId), new DeliveryPhaseId(deliveryPhaseId)), cancellationToken);

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
                new GetDeliveryPhaseDetailsQuery(AhpApplicationId.From(applicationId), new DeliveryPhaseId(deliveryPhaseId)), cancellationToken);

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
            deliveryPhaseId,
            model,
            cancellationToken);
    }

    [WorkflowState(DeliveryPhaseWorkflowState.StartOnSiteMilestone)]
    [HttpGet("{deliveryPhaseId}/start-on-site-milestone")]
    public async Task<IActionResult> StartOnSiteMilestone([FromRoute] string applicationId, string deliveryPhaseId, CancellationToken cancellationToken)
    {
        var deliveryPhaseDetails =
            await _deliveryPhaseProvider.Get(
                new GetDeliveryPhaseDetailsQuery(AhpApplicationId.From(applicationId), new DeliveryPhaseId(deliveryPhaseId)), cancellationToken);

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
            deliveryPhaseId,
            model,
            cancellationToken);
    }

    [WorkflowState(DeliveryPhaseWorkflowState.PracticalCompletionMilestone)]
    [HttpGet("{deliveryPhaseId}/practical-completion-milestone")]
    public async Task<IActionResult> PracticalCompletionMilestone([FromRoute] string applicationId, string deliveryPhaseId, CancellationToken cancellationToken)
    {
        var deliveryPhaseDetails =
            await _deliveryPhaseProvider.Get(
                new GetDeliveryPhaseDetailsQuery(AhpApplicationId.From(applicationId), new DeliveryPhaseId(deliveryPhaseId)), cancellationToken);

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
            deliveryPhaseId,
            model,
            cancellationToken);
    }

    [WorkflowState(DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp)]
    [HttpGet("{deliveryPhaseId}/unregistered-body-follow-up")]
    public async Task<IActionResult> UnregisteredBodyFollowUp([FromRoute] string applicationId, string deliveryPhaseId, CancellationToken cancellationToken)
    {
        var deliveryPhaseDetails =
            await _deliveryPhaseProvider.Get(
                new GetDeliveryPhaseDetailsQuery(AhpApplicationId.From(applicationId), new DeliveryPhaseId(deliveryPhaseId)), cancellationToken);

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
            deliveryPhaseId,
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

    protected override async Task<IStateRouting<DeliveryPhaseWorkflowState>> Routing(DeliveryPhaseWorkflowState currentState, object? routeData = null)
    {
        var deliveryPhase = currentState != DeliveryPhaseWorkflowState.Create
            ? await _deliveryPhaseProvider.Get(new GetDeliveryPhaseDetailsQuery(this.GetApplicationIdFromRoute(), new DeliveryPhaseId(this.GetDeliveryPhaseIdFromRoute())), CancellationToken.None)
            : new DeliveryPhaseDetails(string.Empty, string.Empty, string.Empty);
        return new DeliveryPhaseWorkflow(currentState, deliveryPhase);
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
        string deliveryPhaseId,
        MilestoneViewModel modelWithError,
        CancellationToken cancellationToken)
    {
        return await ExecuteCommand(
            deliveryPhaseId,
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
        string deliveryPhaseId,
        IRequest<OperationResult> command,
        string viewName,
        Func<DeliveryPhaseDetails, TViewModel> createViewModelForError,
        CancellationToken cancellationToken)
    {
        var applicationId = this.GetApplicationIdFromRoute();

        return await this.ExecuteCommand(
            _mediator,
            command,
            () => ContinueWithRedirect(new { applicationId = applicationId.Value, deliveryPhaseId }),
            async () =>
            {
                var deliveryPhaseDetails =
                    await _deliveryPhaseProvider.Get(new GetDeliveryPhaseDetailsQuery(applicationId, new DeliveryPhaseId(deliveryPhaseId)), cancellationToken);
                var model = createViewModelForError(deliveryPhaseDetails);

                return View(viewName, model);
            },
            cancellationToken);
    }

    private async Task<IActionResult> ExecuteCommand(
        string deliveryPhaseId,
        IRequest<OperationResult> command,
        string viewName,
        CancellationToken cancellationToken)
    {
        var applicationId = this.GetApplicationIdFromRoute();

        return await this.ExecuteCommand(
            _mediator,
            command,
            () => ContinueWithRedirect(new { applicationId = applicationId.Value, deliveryPhaseId }),
            async () =>
            {
                var deliveryPhaseDetails =
                    await _deliveryPhaseProvider.Get(new GetDeliveryPhaseDetailsQuery(applicationId, new DeliveryPhaseId(deliveryPhaseId)), cancellationToken);
                return View(viewName, deliveryPhaseDetails);
            },
            cancellationToken);
    }
}
