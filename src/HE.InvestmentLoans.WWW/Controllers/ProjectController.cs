using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.Projects;
using HE.InvestmentLoans.Contract.Projects.Commands;
using HE.InvestmentLoans.Contract.Projects.Queries;
using HE.InvestmentLoans.Contract.Projects.ViewModels;
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
        var result = await _mediator.Send(new CreateProjectCommand(LoanApplicationId.From(id)));

        return await Continue(new { id, projectId = result.Result.Value });
    }

    [HttpGet("{projectId}/name")]
    [WorkflowState(ProjectState.Name)]
    public async Task<IActionResult> ProjectName(Guid id, Guid projectId)
    {
        var result = await _mediator.Send(new GetProjectQuery(LoanApplicationId.From(id), ProjectId.From(projectId)));

        return View(result);
    }

    [HttpPost("{projectId}/name")]
    [WorkflowState(ProjectState.Name)]
    public async Task<IActionResult> ProjectName(Guid id, Guid projectId, ProjectViewModel model, CancellationToken token)
    {
        var result = await _mediator.Send(new ChangeProjectNameCommand(LoanApplicationId.From(id), ProjectId.From(projectId), model.Name), token);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View("ProjectName", model);
        }

        return await Continue(new { id, projectId });
    }

    [HttpGet("{projectId}/start-date")]
    [WorkflowState(ProjectState.StartDate)]
    public async Task<IActionResult> StartDate(Guid id, Guid projectId)
    {
        var result = await _mediator.Send(new GetProjectQuery(LoanApplicationId.From(id), ProjectId.From(projectId)));

        return View(result);
    }

    [HttpPost("{projectId}/start-date")]
    [WorkflowState(ProjectState.StartDate)]
    public async Task<IActionResult> StartDate(Guid id, Guid projectId, ProjectViewModel model, CancellationToken token)
    {
        var result = await _mediator.Send(new ProvideStartDateCommand(LoanApplicationId.From(id), ProjectId.From(projectId), model.HasEstimatedStartDate, model.EstimatedStartDay, model.EstimatedStartMonth, model.EstimatedStartYear), token);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View("StartDate", model);
        }

        return await Continue(new { id, projectId });
    }

    [HttpGet("{projectId}/many-homes")]
    [WorkflowState(ProjectState.ManyHomes)]
    public async Task<IActionResult> ManyHomes(Guid id, Guid projectId)
    {
        var result = await _mediator.Send(new GetProjectQuery(LoanApplicationId.From(id), ProjectId.From(projectId)));

        return View(result);
    }

    [HttpPost("{projectId}/many-homes")]
    [WorkflowState(ProjectState.ManyHomes)]
    public async Task<IActionResult> ManyHomes(Guid id, Guid projectId, ProjectViewModel model, CancellationToken token)
    {
        var result = await _mediator.Send(new ProvideHomesCountCommand(LoanApplicationId.From(id), ProjectId.From(projectId), model.HomesCount), token);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View("ManyHomes", model);
        }

        return await Continue(new { id, projectId });
    }

    [HttpGet("{projectId}/type-homes")]
    [WorkflowState(ProjectState.TypeHomes)]
    public async Task<IActionResult> TypeHomes(Guid id, Guid projectId)
    {
        var result = await _mediator.Send(new GetProjectQuery(LoanApplicationId.From(id), ProjectId.From(projectId)));

        return View(result);
    }

    [HttpPost("{projectId}/type-homes")]
    [WorkflowState(ProjectState.TypeHomes)]
    public async Task<IActionResult> TypeHomes(Guid id, Guid projectId, ProjectViewModel model, CancellationToken token)
    {
        var result = await _mediator.Send(new ProvideHomesTypesCommand(LoanApplicationId.From(id), ProjectId.From(projectId), model.HomesTypes), token);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View("TypeHomes", model);
        }

        return await Continue(new { id, projectId });
    }

    [HttpGet("{projectId}/type")]
    [WorkflowState(ProjectState.Type)]
    public async Task<IActionResult> Type(Guid id, Guid projectId)
    {
        var result = await _mediator.Send(new GetProjectQuery(LoanApplicationId.From(id), ProjectId.From(projectId)));

        return View(result);
    }

    [HttpPost("{projectId}/type")]
    [WorkflowState(ProjectState.Type)]
    public async Task<IActionResult> Type(Guid id, Guid projectId, ProjectViewModel model, CancellationToken token)
    {
        var result = await _mediator.Send(new ProvideProjectTypeCommand(LoanApplicationId.From(id), ProjectId.From(projectId), model.ProjectType), token);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View("Type", model);
        }

        return await Continue(new { id, projectId });
    }

    protected override Task<IStateRouting<ProjectState>> Routing(ProjectState currentState)
    {
        return Task.FromResult((IStateRouting<ProjectState>)new ProjectWorkflow(currentState));
    }
}
