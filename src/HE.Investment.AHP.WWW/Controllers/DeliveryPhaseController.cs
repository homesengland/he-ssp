using HE.Investment.AHP.Contract.Delivery;
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
    public async Task<IActionResult> New()
    {
        return View("Name");
    }

    [HttpGet("{deliveryPhaseId}/name")]
    [WorkflowState(DeliveryPhaseWorkflowState.Name)]
    public async Task<IActionResult> Name()
    {
        return View("Name");
    }

    [WorkflowState(DeliveryPhaseWorkflowState.Remove)]
    [HttpGet("{deliveryPhaseId}/remove")]
    public async Task<IActionResult> Remove()
    {
        return View("RemoveDeliveryPhaseConfirmation");
    }

    protected override Task<IStateRouting<DeliveryPhaseWorkflowState>> Routing(DeliveryPhaseWorkflowState currentState, object? routeData = null)
    {
        return Task.FromResult<IStateRouting<DeliveryPhaseWorkflowState>>(new DeliveryPhaseWorkflow());
    }
}
