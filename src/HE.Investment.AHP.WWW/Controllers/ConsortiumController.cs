using HE.Investment.AHP.Domain.UserContext;
using HE.Investment.AHP.WWW.Models.Consortium;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Contract.Commands;
using HE.Investments.AHP.Consortium.Contract.Queries;
using HE.Investments.Common.Validators;
using HE.Investments.Common.WWW.Routing;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[Route("consortium")]
[AuthorizeWithCompletedProfile(AhpAccessContext.ViewConsortium)]
public class ConsortiumController : WorkflowController<ConsortiumWorkflowState>
{
    private readonly IMediator _mediator;

    private readonly IAhpAccessContext _ahpAccessContext;

    public ConsortiumController(IMediator mediator, IAhpAccessContext ahpAccessContext)
    {
        _mediator = mediator;
        _ahpAccessContext = ahpAccessContext;
    }

    [HttpGet]
    [WorkflowState(ConsortiumWorkflowState.Index)]
    public async Task<IActionResult> Index()
    {
        var consortiumsList = await _mediator.Send(new GetConsortiumsListQuery());
        return View((consortiumsList, await _ahpAccessContext.CanManageConsortium()));
    }

    [HttpGet("start")]
    [WorkflowState(ConsortiumWorkflowState.Start)]
    [AuthorizeWithCompletedProfile(AhpAccessContext.ManageConsortium)]
    public IActionResult Start()
    {
        return View();
    }

    [HttpGet("programme")]
    [WorkflowState(ConsortiumWorkflowState.Programme)]
    [AuthorizeWithCompletedProfile(AhpAccessContext.ManageConsortium)]
    public async Task<IActionResult> Programme()
    {
        var availableProgrammes = await _mediator.Send(new GetAvailableProgrammesQuery());
        return View(new SelectProgramme(string.Empty, availableProgrammes));
    }

    [HttpPost("programme")]
    [WorkflowState(ConsortiumWorkflowState.Programme)]
    [AuthorizeWithCompletedProfile(AhpAccessContext.ManageConsortium)]
    public async Task<IActionResult> ProgrammePost(SelectProgramme model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateConsortiumCommand(model.SelectedProgrammeId), cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("Programme", model with { AvailableProgrammes = await _mediator.Send(new GetAvailableProgrammesQuery(), cancellationToken) });
        }

        return RedirectToAction("SearchOrganisation", "ConsortiumMember", new { consortiumId = result.ReturnedData.Value });
    }

    protected override async Task<IStateRouting<ConsortiumWorkflowState>> Routing(ConsortiumWorkflowState currentState, object? routeData = null)
    {
        return await Task.FromResult<IStateRouting<ConsortiumWorkflowState>>(new ConsortiumWorkflow(currentState));
    }
}
