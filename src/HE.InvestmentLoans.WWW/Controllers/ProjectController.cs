using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.Project;
using HE.InvestmentLoans.Contract.Security;
using HE.InvestmentLoans.WWW.Attributes;
using HE.InvestmentLoans.WWW.Routing;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

[Route("application/{id}/project")]
[AuthorizeWithCompletedProfile]
public class ProjectController : WorkflowController<ProjectState>
{
    private readonly IMediator _mediator;

    public ProjectController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("start")]
    [WorkflowState(ProjectState.Index)]
    public IActionResult StartProject(Guid id)
    {
        return View("Index", LoanApplicationId.From(id));
    }

    [HttpPost("start")]
    [WorkflowState(ProjectState.Index)]
    public async Task<IActionResult> StartProjectPost(Guid id)
    {


        return await Continue(new { projectId = ""});
    }

    [HttpGet("{projectId}/name")]
    [WorkflowState(ProjectState.Index)]
    public IActionResult ProjectName(Guid id, Guid projectId)
    {
        return View("Index", LoanApplicationId.From(id));
    }

    protected override IStateRouting<ProjectState> Routing(ProjectState currentState)
    {
        throw new NotImplementedException();
    }
}
