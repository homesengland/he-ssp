using HE.Investment.AHP.Contract.Consortium;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.WWW.Routing;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[Route("consortium")]
[AuthorizeWithCompletedProfile]
public class ConsortiumController : WorkflowController<ConsortiumWorkflowState>
{
    [HttpGet]
    [WorkflowState(ConsortiumWorkflowState.Index)]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("start")]
    [WorkflowState(ConsortiumWorkflowState.Start)]
    public IActionResult Start()
    {
        return View();
    }

    protected override async Task<IStateRouting<ConsortiumWorkflowState>> Routing(ConsortiumWorkflowState currentState, object? routeData = null)
    {
        return await Task.FromResult<IStateRouting<ConsortiumWorkflowState>>(new ConsortiumWorkflow(currentState));
    }
}
