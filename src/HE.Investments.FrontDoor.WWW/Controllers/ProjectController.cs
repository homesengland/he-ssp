using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Validators;
using HE.Investments.Common.Workflow;
using HE.Investments.Common.WWW.Controllers;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Contract.Project.Commands;
using HE.Investments.FrontDoor.Contract.Project.Queries;
using HE.Investments.FrontDoor.WWW.Extensions;
using HE.Investments.FrontDoor.WWW.Workflows;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.FrontDoor.WWW.Controllers;

[AuthorizeWithCompletedProfile]
[Route("project")]
public class ProjectController : WorkflowController<ProjectWorkflowState>
{
    private readonly IMediator _mediator;

    public ProjectController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{projectId}/back")]
    public async Task<IActionResult> Back([FromRoute] string projectId, ProjectWorkflowState currentPage)
    {
        return await Back(currentPage, new { projectId });
    }

    [HttpGet("start")]
    public IActionResult Start()
    {
        return View();
    }

    [HttpGet("new-england-housing-delivery")]
    public IActionResult NewEnglandHousingDelivery([FromQuery] bool? isEnglandHousingDelivery)
    {
        return View(nameof(EnglandHousingDelivery), isEnglandHousingDelivery);
    }

    [HttpPost("new-england-housing-delivery")]
    public async Task<IActionResult> NewEnglandHousingDelivery([FromForm] bool? isEnglandHousingDelivery, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<ProjectDetails>(
            _mediator,
            new ProvideEnglandHousingDeliveryCommand(null, isEnglandHousingDelivery),
            () => Task.FromResult<IActionResult>(RedirectToAction(isEnglandHousingDelivery == true ? nameof(NewName) : nameof(NewNotEligibleForAnything))),
            () => Task.FromResult<IActionResult>(View("EnglandHousingDelivery", isEnglandHousingDelivery)),
            cancellationToken);
    }

    [HttpGet("new-not-eligible-for-anything")]
    public IActionResult NewNotEligibleForAnything()
    {
        return View(nameof(NotEligibleForAnything));
    }

    [HttpGet("new-name")]
    public IActionResult NewName()
    {
        return View(nameof(Name));
    }

    [HttpPost("new-name")]
    public async Task<IActionResult> NewName([FromForm] string? name, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateProjectCommand(name), cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View(nameof(Name), name);
        }

        return await Continue(ProjectWorkflowState.Name, new { projectId = result.ReturnedData.Value });
    }

    [HttpGet("{projectId}/england-housing-delivery")]
    [WorkflowState(ProjectWorkflowState.EnglandHousingDelivery)]
    public async Task<IActionResult> EnglandHousingDelivery([FromRoute] string projectId, CancellationToken cancellationToken)
    {
        var projectDetails = await GetProjectDetails(projectId, cancellationToken);
        return View(nameof(EnglandHousingDelivery), projectDetails.IsEnglandHousingDelivery);
    }

    [HttpPost("{projectId}/england-housing-delivery")]
    [WorkflowState(ProjectWorkflowState.EnglandHousingDelivery)]
    public async Task<IActionResult> EnglandHousingDelivery([FromRoute] string projectId, [FromForm] bool? isEnglandHousingDelivery, CancellationToken cancellationToken)
    {
        return await ExecuteProjectCommand(
            new ProvideEnglandHousingDeliveryCommand(new FrontDoorProjectId(projectId), isEnglandHousingDelivery),
            nameof(EnglandHousingDelivery),
            _ => isEnglandHousingDelivery,
            cancellationToken);
    }

    [HttpGet("{projectId}/not-eligible-for-anything")]
    [WorkflowState(ProjectWorkflowState.NotEligibleForAnything)]
    public IActionResult NotEligibleForAnything([FromRoute] string projectId)
    {
        return View();
    }

    [HttpGet("{projectId}/name")]
    [WorkflowState(ProjectWorkflowState.Name)]
    public async Task<IActionResult> Name([FromRoute] string projectId, CancellationToken cancellationToken)
    {
        var model = await GetProjectDetails(projectId, cancellationToken);
        return View(nameof(Name), model.Name);
    }

    [HttpPost("{projectId}/name")]
    [WorkflowState(ProjectWorkflowState.Name)]
    public async Task<IActionResult> Name([FromRoute] string projectId, string? name, CancellationToken cancellationToken)
    {
        return await ExecuteProjectCommand(
            new ProvideProjectNameCommand(new FrontDoorProjectId(projectId), name),
            nameof(Name),
            _ => name,
            cancellationToken);
    }

    [HttpGet("{projectId}/support-required-activities")]
    [WorkflowState(ProjectWorkflowState.SupportRequiredActivities)]
    public async Task<IActionResult> SupportRequiredActivities([FromRoute] string projectId, CancellationToken cancellationToken)
    {
        return View(await GetProjectDetails(projectId, cancellationToken));
    }

    protected override async Task<IStateRouting<ProjectWorkflowState>> Routing(ProjectWorkflowState currentState, object? routeData = null)
    {
        var projectId = routeData?.GetPropertyValue<string>("projectId") ?? this.GetProjectIdFromRoute().Value;
        var project = await GetProjectDetails(projectId, CancellationToken.None);
        var workflow = new ProjectWorkflow(currentState, project);
        if (Request.TryGetWorkflowQueryParameter(out var lastEncodedWorkflow))
        {
            var lastWorkflow = new EncodedWorkflow<ProjectWorkflowState>(lastEncodedWorkflow);
            var currentWorkflow = workflow.GetEncodedWorkflow();
            var changedState = currentWorkflow.GetNextChangedWorkflowState(currentState, lastWorkflow);

            return new ProjectWorkflow(changedState, project, true);
        }

        return workflow;
    }

    private async Task<IActionResult> ExecuteProjectCommand<TViewModel>(
        IRequest<OperationResult> command,
        string viewName,
        Func<ProjectDetails, TViewModel> createViewModelForError,
        CancellationToken cancellationToken,
        object? routeData = null)
    {
        var projectId = this.GetProjectIdFromRoute();

        return await this.ExecuteCommand<TViewModel>(
            _mediator,
            command,
            async () => await ContinueWithRedirect(routeData ?? new { projectId.Value }),
            async () =>
            {
                var siteDetails = await GetProjectDetails(projectId.Value, cancellationToken);
                var model = createViewModelForError(siteDetails);

                return View(viewName, model);
            },
            cancellationToken);
    }

    private async Task<ProjectDetails> GetProjectDetails(string projectId, CancellationToken cancellationToken)
    {
        var projectDetails = await _mediator.Send(new GetProjectDetailsQuery(new FrontDoorProjectId(projectId)), cancellationToken);
        ViewBag.ProjectName = projectDetails.Name;
        return projectDetails;
    }
}
