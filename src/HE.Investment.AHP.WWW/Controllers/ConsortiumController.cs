using HE.Investment.AHP.WWW.Models.Consortium;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Account.Api.Contract.User;
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
[AuthorizeWithCompletedProfile(new[] { UserRole.Admin, UserRole.Enhanced, UserRole.Input, UserRole.ViewOnly })]
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
    [AuthorizeWithCompletedProfile(new[] { UserRole.Admin, UserRole.Enhanced })]
    public IActionResult Start()
    {
        return View();
    }

    [HttpGet("programme")]
    [WorkflowState(ConsortiumWorkflowState.Programme)]
    [AuthorizeWithCompletedProfile(new[] { UserRole.Admin, UserRole.Enhanced })]
    public async Task<IActionResult> Programme()
    {
        var availableProgrammes = await _mediator.Send(new GetAvailableProgrammesQuery());
        return View(new SelectProgramme(string.Empty, availableProgrammes));
    }

    [HttpPost("programme")]
    [WorkflowState(ConsortiumWorkflowState.Programme)]
    [AuthorizeWithCompletedProfile(new[] { UserRole.Admin, UserRole.Enhanced })]
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
