using HE.Investment.AHP.Contract.Application.Queries;
using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.Contract.Delivery.Commands;
using HE.Investment.AHP.Contract.Delivery.Queries;
using HE.Investment.AHP.Domain.Delivery.ValueObjects;
using HE.Investment.AHP.WWW.Models.Delivery;
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

    public DeliveryPhaseController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("new")]
    [WorkflowState(DeliveryPhaseWorkflowState.New)]
    public async Task<IActionResult> New([FromRoute] string applicationId)
    {
        var application = await _mediator.Send(new GetApplicationQuery(applicationId));
        var model = new DeliveryPhaseNameViewModel(applicationId, application.Name, null, nameof(this.New));
        return View("Name", model);
    }

    [HttpPost("new")]
    [WorkflowState(DeliveryPhaseWorkflowState.New)]
    public async Task<IActionResult> New([FromRoute] string applicationId, DeliveryPhaseNameViewModel model, CancellationToken cancellationToken)
    {
        var deliveryPhaseId = await _mediator.Send(new CreateDeliveryPhaseCommand(applicationId, model.DeliveryPhaseName ?? string.Empty), cancellationToken);

        var result = await _mediator.Send(new ProvideDeliveryPhaseNameCommand(applicationId, deliveryPhaseId.Id, model.DeliveryPhaseName), cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("Name", model);
        }

        return await Continue(new { applicationId, deliveryPhaseId.Id });
    }

    [HttpGet("{deliveryPhaseId}/name")]
    [WorkflowState(DeliveryPhaseWorkflowState.Name)]
    public async Task<IActionResult> Name([FromRoute] string applicationId, [FromRoute] string deliveryPhaseId, CancellationToken cancellationToken)
    {
        var deliveryPhase = await _mediator.Send(new GetDeliveryPhaseDetailsQuery(applicationId, deliveryPhaseId), cancellationToken);
        var model = new DeliveryPhaseNameViewModel(applicationId, deliveryPhase.ApplicationName, deliveryPhase.Name, nameof(this.Name));
        return View("Name", model);
    }

    [WorkflowState(DeliveryPhaseWorkflowState.Name)]
    [HttpPost("{deliveryPhaseId}/name")]
    public async Task<IActionResult> Name([FromRoute] string applicationId, [FromRoute] string deliveryPhaseId, DeliveryPhaseNameViewModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ProvideDeliveryPhaseNameCommand(applicationId, deliveryPhaseId, model.DeliveryPhaseName), cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("Name", model);
        }

        return await Continue(new { applicationId, deliveryPhaseId });
    }

    [HttpGet("{deliveryPhaseId}/remove")]
    public async Task<IActionResult> Remove([FromRoute] string applicationId, string deliveryPhaseId, CancellationToken cancellationToken)
    {
        var deliveryPhaseDetails = await _mediator.Send(new GetDeliveryPhaseDetailsQuery(applicationId, deliveryPhaseId), cancellationToken);

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
    public IActionResult AcquisitionMilestone(string deliveryPhaseId)
    {
        return View(CreateMilestoneViewModel(deliveryPhaseId));
    }

    [WorkflowState(DeliveryPhaseWorkflowState.AcquisitionMilestone)]
    [HttpPost("{deliveryPhaseId}/acquisition-milestone")]
    public async Task<IActionResult> AcquisitionMilestone(string deliveryPhaseId, MilestoneViewModel model, CancellationToken cancellationToken)
    {
        return await ExecuteCommand(
            new ProvideAcquisitionMilestoneDetailsCommand(
                this.GetApplicationIdFromRoute(),
                deliveryPhaseId,
                model.MilestoneStartAt,
                model.ClaimMilestonePaymentAt),
            nameof(AcquisitionMilestone),
            model,
            cancellationToken);
    }

    [WorkflowState(DeliveryPhaseWorkflowState.StartOnSiteMilestone)]
    [HttpGet("{deliveryPhaseId}/start-on-site-milestone")]
    public IActionResult StartOnSiteMilestone(string deliveryPhaseId)
    {
        return View(CreateMilestoneViewModel(deliveryPhaseId));
    }

    [WorkflowState(DeliveryPhaseWorkflowState.StartOnSiteMilestone)]
    [HttpPost("{deliveryPhaseId}/start-on-site-milestone")]
    public async Task<IActionResult> StartOnSiteMilestone(string deliveryPhaseId, MilestoneViewModel model, CancellationToken cancellationToken)
    {
        return await ExecuteCommand(
            new ProvideStartOnSiteMilestoneDetailsCommand(
                this.GetApplicationIdFromRoute(),
                deliveryPhaseId,
                model.MilestoneStartAt,
                model.ClaimMilestonePaymentAt),
            nameof(StartOnSiteMilestone),
            model,
            cancellationToken);
    }

    [WorkflowState(DeliveryPhaseWorkflowState.PracticalCompletionMilestone)]
    [HttpGet("{deliveryPhaseId}/practical-completion-milestone")]
    public IActionResult PracticalCompletionMilestone(string deliveryPhaseId)
    {
        return View(CreateMilestoneViewModel(deliveryPhaseId));
    }

    [WorkflowState(DeliveryPhaseWorkflowState.PracticalCompletionMilestone)]
    [HttpPost("{deliveryPhaseId}/practical-completion-milestone")]
    public async Task<IActionResult> PracticalCompletionMilestone(string deliveryPhaseId, MilestoneViewModel model, CancellationToken cancellationToken)
    {
        return await ExecuteCommand(
            new ProvideStartOnSiteMilestoneDetailsCommand(
                this.GetApplicationIdFromRoute(),
                deliveryPhaseId,
                model.MilestoneStartAt,
                model.ClaimMilestonePaymentAt),
            nameof(PracticalCompletionMilestone),
            model,
            cancellationToken);
    }

    [WorkflowState(DeliveryPhaseWorkflowState.UnregisteredProviderFollowUp)]
    [HttpGet("{deliveryPhaseId}/unregistered-provider-follow-up")]
    public IActionResult UnregisteredProviderFollowUp(string deliveryPhaseId)
    {
        return View(CreateMilestoneViewModel(deliveryPhaseId));
    }

    [WorkflowState(DeliveryPhaseWorkflowState.UnregisteredProviderFollowUp)]
    [HttpPost("{deliveryPhaseId}/unregistered-provider-follow-up")]
    public IActionResult UnregisteredProviderFollowUp(string deliveryPhaseId, bool? requestAdditionalPayments, CancellationToken cancellationToken)
    {
        if (requestAdditionalPayments == null)
        {
            ModelState.AddModelError("requestAdditionalPayments", "Select value");
        }

        return View(CreateMilestoneViewModel(deliveryPhaseId));
    }

    protected override Task<IStateRouting<DeliveryPhaseWorkflowState>> Routing(DeliveryPhaseWorkflowState currentState, object? routeData = null)
    {
        return Task.FromResult<IStateRouting<DeliveryPhaseWorkflowState>>(new DeliveryPhaseWorkflow());
    }

    private MilestoneViewModel CreateMilestoneViewModel(string deliveryPhaseId)
    {
        var start = new DateDetails("1", "1", "1");
        var end = new DateDetails(null, null, null);

        return new MilestoneViewModel("123", "App name", "delivery phase name", start, end);
    }

    private async Task<IActionResult> ExecuteCommand(
        IRequest<OperationResult> command,
        string viewName,
        object model,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View(viewName, model);
        }

        return await ContinueWithRedirect(new { applicationId = this.GetApplicationIdFromRoute() });
    }
}
