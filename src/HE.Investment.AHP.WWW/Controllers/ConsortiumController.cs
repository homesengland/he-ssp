using HE.Investment.AHP.Contract.Consortium;
using HE.Investment.AHP.WWW.Models.Consortium;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.AHP.Consortium.Contract.Queries;
using HE.Investments.Common.WWW.Routing;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[Route("consortium")]
[AuthorizeWithCompletedProfile]
public class ConsortiumController : WorkflowController<ConsortiumWorkflowState>
{
    private readonly IMediator _mediator;

    public ConsortiumController(IMediator mediator)
    {
        _mediator = mediator;
    }

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

    [HttpGet("programme")]
    [WorkflowState(ConsortiumWorkflowState.Programme)]
    public async Task<IActionResult> Programme()
    {
        var availableProgrammes = await _mediator.Send(new GetAvailableProgrammesQuery());
        return View(new SelectProgramme(null, availableProgrammes));
    }

    [HttpPost("programme")]
    [WorkflowState(ConsortiumWorkflowState.Programme)]
    public async Task<IActionResult> ProgrammePost(SelectProgramme model)
    {
        var availableProgrammes = await _mediator.Send(new GetAvailableProgrammesQuery());
        return View("Programme", model with { AvailableProgrammes = availableProgrammes });
    }

    protected override async Task<IStateRouting<ConsortiumWorkflowState>> Routing(ConsortiumWorkflowState currentState, object? routeData = null)
    {
        return await Task.FromResult<IStateRouting<ConsortiumWorkflowState>>(new ConsortiumWorkflow(currentState));
    }
}
