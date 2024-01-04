using HE.Investment.AHP.Contract.Delivery;
using HE.Investment.AHP.WWW.Models.Delivery;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.WWW.Routing;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[AuthorizeWithCompletedProfile]
[Route("application/{applicationId}/DeliveryPhase")]
public class DeliveryPhaseController : WorkflowController<DeliveryPhaseWorkflowState>
{
    [HttpGet("new")]
    [WorkflowState(DeliveryPhaseWorkflowState.New)]
    public IActionResult New()
    {
        return View("Name");
    }

    [HttpGet("{deliveryPhaseId}/name")]
    [WorkflowState(DeliveryPhaseWorkflowState.Name)]
    public IActionResult Name()
    {
        return View("Name");
    }

    [WorkflowState(DeliveryPhaseWorkflowState.Remove)]
    [HttpGet("{deliveryPhaseId}/remove")]
    public IActionResult Remove()
    {
        return View("RemoveDeliveryPhaseConfirmation");
    }

    [WorkflowState(DeliveryPhaseWorkflowState.AcquisitionMilestone)]
    [HttpGet("{deliveryPhaseId}/acquisition-milestone")]
    public IActionResult AcquisitionMilestone(string deliveryPhaseId)
    {
        return View(CreateMilestoneViewModel(deliveryPhaseId));
    }

    [WorkflowState(DeliveryPhaseWorkflowState.AcquisitionMilestone)]
    [HttpPost("{deliveryPhaseId}/acquisition-milestone")]
    public IActionResult AcquisitionMilestone(string deliveryPhaseId, MilestoneViewModel model, CancellationToken cancellationToken)
    {
        ModelState.AddModelError("MilestoneDate.Day", "invalid day");

        return View(CreateMilestoneViewModel(deliveryPhaseId, model.MilestoneDates));
    }

    [WorkflowState(DeliveryPhaseWorkflowState.StartOnSiteMilestone)]
    [HttpGet("{deliveryPhaseId}/start-on-site-milestone")]
    public IActionResult StartOnSiteMilestone(string deliveryPhaseId)
    {
        return View(CreateMilestoneViewModel(deliveryPhaseId));
    }

    [WorkflowState(DeliveryPhaseWorkflowState.StartOnSiteMilestone)]
    [HttpPost("{deliveryPhaseId}/start-on-site-milestone")]
    public IActionResult StartOnSiteMilestone(string deliveryPhaseId, MilestoneViewModel model, CancellationToken cancellationToken)
    {
        ModelState.AddModelError("MilestoneDate.Day", "invalid day");

        return View(CreateMilestoneViewModel(deliveryPhaseId, model.MilestoneDates));
    }

    [WorkflowState(DeliveryPhaseWorkflowState.PracticalCompletionMilestone)]
    [HttpGet("{deliveryPhaseId}/practical-completion-milestone")]
    public IActionResult PracticalCompletionMilestone(string deliveryPhaseId)
    {
        return View(CreateMilestoneViewModel(deliveryPhaseId));
    }

    [WorkflowState(DeliveryPhaseWorkflowState.PracticalCompletionMilestone)]
    [HttpPost("{deliveryPhaseId}/practical-completion-milestone")]
    public IActionResult PracticalCompletionMilestone(string deliveryPhaseId, MilestoneViewModel model, CancellationToken cancellationToken)
    {
        ModelState.AddModelError("MilestoneDate.Day", "invalid day");

        return View(CreateMilestoneViewModel(deliveryPhaseId, model.MilestoneDates));
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

    private MilestoneViewModel CreateMilestoneViewModel(string deliveryPhaseId, MilestoneDatesModel? dates = null)
    {
        return new MilestoneViewModel("123", "App name", "delivery phase name", dates);
    }
}
