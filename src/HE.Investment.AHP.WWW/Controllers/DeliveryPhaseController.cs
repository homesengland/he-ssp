using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Contract.Delivery.Queries;
using HE.Investment.AHP.WWW.Extensions;
using HE.Investment.AHP.WWW.Models.Delivery;
using HE.Investment.AHP.WWW.Utils;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Validators;
using HE.Investments.Common.WWW.Controllers;
using HE.Investments.Common.WWW.Routing;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[AuthorizeWithCompletedProfile]
[Route("application/{applicationId}/DeliveryPhase")]
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

    [HttpGet("new")]
    [WorkflowState(DeliveryPhaseWorkflowState.New)]
    public IActionResult New()
    {
        return View("Name");
    }

    [HttpGet("{deliveryPhaseId}/Name")]
    [WorkflowState(DeliveryPhaseWorkflowState.Name)]
    public IActionResult Name([FromRoute] string applicationId, string deliveryPhaseId)
    {
        return View("Name");
    }

    [HttpPost("{deliveryPhaseId}/Name")]
    [WorkflowState(DeliveryPhaseWorkflowState.Name)]
    public async Task<IActionResult> Name([FromRoute] string applicationId, string deliveryPhaseId, DeliveryPhaseDetails deliveryPhaseDetails, CancellationToken cancellationToken)
    {
        return await ContinueWithRedirect(new { applicationId, deliveryPhaseId });
    }

    [HttpGet("{deliveryPhaseId}/Details")]
    [WorkflowState(DeliveryPhaseWorkflowState.TypeOfHomes)]
    public async Task<IActionResult> Details([FromRoute] string applicationId, string deliveryPhaseId, CancellationToken cancellationToken)
    {
        var deliveryPhaseDetails = await _mediator.Send(new GetDeliveryPhaseDetailsQuery(applicationId, deliveryPhaseId), cancellationToken);

        return View("Details", deliveryPhaseDetails);
    }

    [HttpPost("{deliveryPhaseId}/Details")]
    [WorkflowState(DeliveryPhaseWorkflowState.TypeOfHomes)]
    public async Task<IActionResult> Details([FromRoute] string applicationId, string deliveryPhaseId, DeliveryPhaseDetails deliveryPhaseDetails, CancellationToken cancellationToken)
    {
        return await ExecuteCommand(
            deliveryPhaseId,
            new ProvideTypeOfHomesCommand(applicationId, deliveryPhaseId, deliveryPhaseDetails.TypeOfHomes),
            nameof(Details),
            cancellationToken);
    }

    [HttpGet("{deliveryPhaseId}/remove")]
    public async Task<IActionResult> Remove([FromRoute] string applicationId, string deliveryPhaseId, CancellationToken cancellationToken)
    {
        var deliveryPhaseDetails = await _deliveryPhaseProvider.Get(new GetDeliveryPhaseDetailsQuery(applicationId, deliveryPhaseId), cancellationToken);

        return View("RemoveDeliveryPhaseConfirmation", new RemoveDeliveryPhaseModel(deliveryPhaseDetails.ApplicationName, deliveryPhaseDetails.Name));
    }

    [HttpPost("{deliveryPhaseId}/remove")]
    public async Task<IActionResult> Remove(
        [FromRoute] string applicationId,
        string deliveryPhaseId,
        RemoveDeliveryPhaseModel model,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RemoveDeliveryPhaseCommand(applicationId, deliveryPhaseId, model.RemoveDeliveryPhaseAnswer), cancellationToken);
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
        var deliveryPhaseDetails = await _deliveryPhaseProvider.Get(new GetDeliveryPhaseDetailsQuery(applicationId, deliveryPhaseId), cancellationToken);

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
                deliveryPhaseId,
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
        var deliveryPhaseDetails = await _deliveryPhaseProvider.Get(new GetDeliveryPhaseDetailsQuery(applicationId, deliveryPhaseId), cancellationToken);

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
                deliveryPhaseId,
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
        var deliveryPhaseDetails = await _deliveryPhaseProvider.Get(new GetDeliveryPhaseDetailsQuery(applicationId, deliveryPhaseId), cancellationToken);

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
            new ProvideStartOnSiteMilestoneDetailsCommand(
                this.GetApplicationIdFromRoute(),
                deliveryPhaseId,
                model.MilestoneStartAt,
                model.ClaimMilestonePaymentAt),
            nameof(PracticalCompletionMilestone),
            deliveryPhaseId,
            model,
            cancellationToken);
    }

    [WorkflowState(DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp)]
    [HttpGet("{deliveryPhaseId}/unregistered-provider-follow-up")]
    public async Task<IActionResult> UnregisteredProviderFollowUp([FromRoute] string applicationId, string deliveryPhaseId, CancellationToken cancellationToken)
    {
        var deliveryPhaseDetails = await _deliveryPhaseProvider.Get(new GetDeliveryPhaseDetailsQuery(applicationId, deliveryPhaseId), cancellationToken);

        return View(new DeliveryViewModelBase(
            applicationId,
            deliveryPhaseDetails.ApplicationName,
            deliveryPhaseDetails.Name));
    }

    [WorkflowState(DeliveryPhaseWorkflowState.UnregisteredBodyFollowUp)]
    [HttpPost("{deliveryPhaseId}/unregistered-provider-follow-up")]
    public async Task<IActionResult> UnregisteredProviderFollowUp(
        [FromRoute] string applicationId,
        string deliveryPhaseId,
        bool? requestAdditionalPayments,
        CancellationToken cancellationToken)
    {
        var deliveryPhaseDetails = await _deliveryPhaseProvider.Get(new GetDeliveryPhaseDetailsQuery(applicationId, deliveryPhaseId), cancellationToken);

        if (requestAdditionalPayments == null)
        {
            ModelState.AddModelError("requestAdditionalPayments", "Select value");
        }

        return View(new DeliveryViewModelBase(
            applicationId,
            deliveryPhaseDetails.ApplicationName,
            deliveryPhaseDetails.Name));
    }

    protected override async Task<IStateRouting<DeliveryPhaseWorkflowState>> Routing(DeliveryPhaseWorkflowState currentState, object? routeData = null)
    {
        var deliveryPhase = await _deliveryPhaseProvider.Get(new GetDeliveryPhaseDetailsQuery(this.GetApplicationIdFromRoute(), this.GetDeliveryPhaseIdFromRoute()), CancellationToken.None);

        return new DeliveryPhaseWorkflow(currentState, deliveryPhase.IsUnregisteredBody);
    }

    private MilestoneViewModel CreateMilestoneViewModel(
        string applicationName,
        string deliveryPhaseName,
        DateDetails? milestoneDate,
        DateDetails? milestonePaymentDate)
    {
        return new MilestoneViewModel(
            this.GetApplicationIdFromRoute(),
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
            () => ContinueWithRedirect(new { applicationId, deliveryPhaseId }),
            async () =>
            {
                var deliveryPhaseDetails = await _deliveryPhaseProvider.Get(new GetDeliveryPhaseDetailsQuery(applicationId, deliveryPhaseId), cancellationToken);
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
            () => ContinueWithRedirect(new { applicationId, deliveryPhaseId }),
            async () =>
            {
                var deliveryPhaseDetails = await _deliveryPhaseProvider.Get(new GetDeliveryPhaseDetailsQuery(applicationId, deliveryPhaseId), cancellationToken);
                return View(viewName, deliveryPhaseDetails);
            },
            cancellationToken);
    }
}
