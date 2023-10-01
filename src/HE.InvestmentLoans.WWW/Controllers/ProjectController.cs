using He.AspNetCore.Mvc.Gds.Components.Constants;
using HE.InvestmentLoans.BusinessLogic.LoanApplicationLegacy.Workflow;
using HE.InvestmentLoans.Common.Extensions;
using HE.InvestmentLoans.Common.Validation;
using HE.InvestmentLoans.Contract.Application.ValueObjects;
using HE.InvestmentLoans.Contract.Projects;
using HE.InvestmentLoans.Contract.Projects.Commands;
using HE.InvestmentLoans.Contract.Projects.Queries;
using HE.InvestmentLoans.Contract.Projects.ViewModels;
using HE.InvestmentLoans.Contract.Security.Queries;
using HE.InvestmentLoans.WWW.Attributes;
using HE.InvestmentLoans.WWW.Routing;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;

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

    [HttpGet("{projectId}/planning-ref-number-exists")]
    [WorkflowState(ProjectState.PlanningRef)]
    public async Task<IActionResult> PlanningReferenceNumberExists(Guid id, Guid projectId)
    {
        var result = await _mediator.Send(new GetProjectQuery(LoanApplicationId.From(id), ProjectId.From(projectId)));

        return View(result);
    }

    [HttpPost("{projectId}/planning-ref-number-exists")]
    [WorkflowState(ProjectState.PlanningRef)]
    public async Task<IActionResult> PlanningReferenceNumberExists(Guid id, Guid projectId, ProjectViewModel model, CancellationToken token)
    {
        var result = await _mediator.Send(new ProvidePlanningReferenceNumberCommand(LoanApplicationId.From(id), ProjectId.From(projectId), model.PlanningReferenceNumberExists, null), token);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View("PlanningReferenceNumberExists", model);
        }

        return await Continue(new { id, projectId });
    }

    [HttpGet("{projectId}/planning-ref-number")]
    [WorkflowState(ProjectState.PlanningRefEnter)]
    public async Task<IActionResult> PlanningReferenceNumber(Guid id, Guid projectId)
    {
        var result = await _mediator.Send(new GetProjectQuery(LoanApplicationId.From(id), ProjectId.From(projectId)));

        return View(result);
    }

    [HttpPost("{projectId}/planning-ref-number")]
    [WorkflowState(ProjectState.PlanningRefEnter)]
    public async Task<IActionResult> PlanningReferenceNumber(Guid id, Guid projectId, ProjectViewModel model, CancellationToken token)
    {
        var result = await _mediator.Send(new ProvidePlanningReferenceNumberCommand(LoanApplicationId.From(id), ProjectId.From(projectId), CommonResponse.Yes, model.PlanningReferenceNumber), token);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View("PlanningReferenceNumber", model);
        }

        return await Continue(new { id, projectId });
    }

    [HttpGet("{projectId}/planning-permission-status")]
    [WorkflowState(ProjectState.PlanningPermissionStatus)]
    public async Task<IActionResult> PlanningPermissionStatus(Guid id, Guid projectId)
    {
        var result = await _mediator.Send(new GetProjectQuery(LoanApplicationId.From(id), ProjectId.From(projectId)));

        return View(result);
    }

    [HttpGet("{projectId}/location")]
    [WorkflowState(ProjectState.Location)]
    public async Task<IActionResult> Location(Guid id, Guid projectId)
    {
        var result = await _mediator.Send(new GetProjectQuery(LoanApplicationId.From(id), ProjectId.From(projectId)));

        return View(result);
    }

    [HttpPost("{projectId}/location")]
    [WorkflowState(ProjectState.Location)]
    public async Task<IActionResult> Location(Guid id, Guid projectId, ProjectViewModel model, CancellationToken token)
    {
        var result = await _mediator.Send(new ProvideLocationCommand(LoanApplicationId.From(id), ProjectId.From(projectId), model.LocationOption, model.LocationCoordinates, model.LocationLandRegistry), token);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);

            return View("Location", model);
        }

        return await Continue(new { id, projectId });
    }

    [HttpGet("{projectId}/ownership")]
    [WorkflowState(ProjectState.Ownership)]
    public async Task<IActionResult> Ownership(Guid id, Guid projectId)
    {
        var result = await _mediator.Send(new GetProjectQuery(LoanApplicationId.From(id), ProjectId.From(projectId)));

        return View(result);
    }

    [HttpGet("{projectId}/check-answers")]
    [WorkflowState(ProjectState.CheckAnswers)]
    public async Task<IActionResult> CheckAnswers(Guid id, Guid projectId)
    {
        var result = await _mediator.Send(new GetProjectQuery(LoanApplicationId.From(id), ProjectId.From(projectId)));

        return View(result);
    }

    [HttpGet("back")]
    public Task<IActionResult> Back(ProjectState currentPage, Guid id, Guid projectId)
    {
        return Back(currentPage, new { id, projectId });
    }

    protected override async Task<IStateRouting<ProjectState>> Routing(ProjectState currentState)
    {
        var id = Request.RouteValues.FirstOrDefault(x => x.Key == "id").Value as string;
        var projectId = Request.RouteValues.FirstOrDefault(x => x.Key == "projectId").Value as string;

        if (projectId.IsNotProvided())
        {
            return ProjectWorkflow.ForStartPage();
        }

        var response = await _mediator.Send(new GetProjectQuery(LoanApplicationId.From(id!), ProjectId.From(projectId!)));

        return new ProjectWorkflow(currentState, response);
    }
}
