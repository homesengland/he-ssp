using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using HE.Investments.Common.Workflow;
using HE.Investments.Common.WWW.Controllers;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Contract.Project.Commands;
using HE.Investments.FrontDoor.Contract.Project.Enums;
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
    public async Task<IActionResult> EnglandHousingDelivery(
        [FromRoute] string projectId,
        [FromForm] bool? isEnglandHousingDelivery,
        CancellationToken cancellationToken)
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

    [HttpPost("{projectId}/support-required-activities")]
    [WorkflowState(ProjectWorkflowState.SupportRequiredActivities)]
    public async Task<IActionResult> SupportRequiredActivities([FromRoute] string projectId, ProjectDetails model, CancellationToken cancellationToken)
    {
        return await ExecuteProjectCommand(
            new ProvideSupportActivitiesCommand(new FrontDoorProjectId(projectId), model.SupportActivityTypes ?? new List<SupportActivityType>()),
            nameof(SupportRequiredActivities),
            project =>
            {
                project.SupportActivityTypes = model.SupportActivityTypes;
                return project;
            },
            cancellationToken);
    }

    [HttpGet("{projectId}/infrastructure")]
    [WorkflowState(ProjectWorkflowState.Infrastructure)]
    public async Task<IActionResult> Infrastructure([FromRoute] string projectId, CancellationToken cancellationToken)
    {
        return View(await GetProjectDetails(projectId, cancellationToken));
    }

    [HttpPost("{projectId}/infrastructure")]
    [WorkflowState(ProjectWorkflowState.Infrastructure)]
    public async Task<IActionResult> Infrastructure([FromRoute] string projectId, ProjectDetails model, CancellationToken cancellationToken)
    {
        return await ExecuteProjectCommand(
            new ProvideInfrastructureTypesCommand(new FrontDoorProjectId(projectId), model.InfrastructureTypes ?? new List<InfrastructureType>()),
            nameof(Infrastructure),
            project =>
            {
                project.InfrastructureTypes = model.InfrastructureTypes;
                return project;
            },
            cancellationToken);
    }

    [HttpGet("{projectId}/tenure")]
    [WorkflowState(ProjectWorkflowState.Tenure)]
    public async Task<IActionResult> Tenure([FromRoute] string projectId, CancellationToken cancellationToken)
    {
        return View(await GetProjectDetails(projectId, cancellationToken));
    }

    [HttpPost("{projectId}/tenure")]
    [WorkflowState(ProjectWorkflowState.Tenure)]
    public async Task<IActionResult> Tenure([FromRoute] string projectId, ProjectDetails model, CancellationToken cancellationToken)
    {
        return await ExecuteProjectCommand(
            new ProvideAffordableHomesAmountCommand(new FrontDoorProjectId(projectId), model.AffordableHomesAmount),
            nameof(Tenure),
            project =>
            {
                project.AffordableHomesAmount = model.AffordableHomesAmount;
                return project;
            },
            cancellationToken);
    }

    [HttpGet("{projectId}/organisation-homes-built")]
    [WorkflowState(ProjectWorkflowState.OrganisationHomesBuilt)]
    public async Task<IActionResult> OrganisationHomesBuilt([FromRoute] string projectId, CancellationToken cancellationToken)
    {
        return View(await GetProjectDetails(projectId, cancellationToken));
    }

    [HttpPost("{projectId}/organisation-homes-built")]
    [WorkflowState(ProjectWorkflowState.OrganisationHomesBuilt)]
    public async Task<IActionResult> OrganisationHomesBuilt([FromRoute] string projectId, ProjectDetails model, CancellationToken cancellationToken)
    {
        return await ExecuteProjectCommand(
            new ProvideOrganisationHomesBuiltCommand(new FrontDoorProjectId(projectId), model.OrganisationHomesBuilt),
            nameof(OrganisationHomesBuilt),
            project =>
            {
                project.OrganisationHomesBuilt = model.OrganisationHomesBuilt;
                return project;
            },
            cancellationToken);
    }

    [HttpGet("{projectId}/identified-site")]
    [WorkflowState(ProjectWorkflowState.IdentifiedSite)]
    public async Task<IActionResult> IdentifiedSite([FromRoute] string projectId, CancellationToken cancellationToken)
    {
        return View(await GetProjectDetails(projectId, cancellationToken));
    }

    [HttpPost("{projectId}/identified-site")]
    [WorkflowState(ProjectWorkflowState.IdentifiedSite)]
    public async Task<IActionResult> IdentifiedSite([FromRoute] string projectId, ProjectDetails model, CancellationToken cancellationToken)
    {
        return await ExecuteProjectCommand(
            new ProvideIdentifiedSiteCommand(new FrontDoorProjectId(projectId), model.IsSiteIdentified),
            nameof(IdentifiedSite),
            project =>
            {
                project.IsSiteIdentified = model.IsSiteIdentified;
                return project;
            },
            cancellationToken);
    }

    [HttpGet("{projectId}/start-site-details")]
    [WorkflowState(ProjectWorkflowState.StartSiteDetails)]
    public async Task<IActionResult> StartSiteDetails([FromRoute] string projectId, CancellationToken cancellationToken)
    {
        var project = await GetProjectDetails(projectId, cancellationToken);
        return project.LastSiteId.IsProvided()
            ? RedirectToAction("Name", "Site", new { projectId, siteId = project.LastSiteId!.Value })
            : RedirectToAction("NewName", "Site", new { projectId });
    }

    [HttpGet("{projectId}/last-site-details")]
    [WorkflowState(ProjectWorkflowState.LastSiteDetails)]
    public async Task<IActionResult> LastSiteDetails([FromRoute] string projectId, CancellationToken cancellationToken)
    {
        var project = await GetProjectDetails(projectId, cancellationToken);
        return project.LastSiteId.IsProvided()
            ? RedirectToAction("AddAnotherSite", "Site", new { projectId, siteId = project.LastSiteId!.Value })
            : RedirectToAction("IdentifiedSite", "Project", new { projectId });
    }

    [HttpGet("{projectId}/geographic-focus")]
    [WorkflowState(ProjectWorkflowState.GeographicFocus)]
    public async Task<IActionResult> GeographicFocus([FromRoute] string projectId, CancellationToken cancellationToken)
    {
        return View(await GetProjectDetails(projectId, cancellationToken));
    }

    [HttpPost("{projectId}/geographic-focus")]
    [WorkflowState(ProjectWorkflowState.GeographicFocus)]
    public async Task<IActionResult> GeographicFocus([FromRoute] string projectId, ProjectDetails model, CancellationToken cancellationToken)
    {
        return await ExecuteProjectCommand(
            new ProvideGeographicFocusCommand(new FrontDoorProjectId(projectId), model.GeographicFocus),
            nameof(GeographicFocus),
            project =>
            {
                project.GeographicFocus = model.GeographicFocus;
                return project;
            },
            cancellationToken);
    }

    [HttpGet("{projectId}/region")]
    [WorkflowState(ProjectWorkflowState.Region)]
    public async Task<IActionResult> Region([FromRoute] string projectId, CancellationToken cancellationToken)
    {
        return View(await GetProjectDetails(projectId, cancellationToken));
    }

    [HttpPost("{projectId}/region")]
    [WorkflowState(ProjectWorkflowState.Region)]
    public async Task<IActionResult> Region([FromRoute] string projectId, ProjectDetails model, CancellationToken cancellationToken)
    {
        return await ExecuteProjectCommand(
            new ProvideRegionCommand(new FrontDoorProjectId(projectId), model.Regions ?? new List<RegionType>()),
            nameof(Region),
            project =>
            {
                project.Regions = model.Regions;
                return project;
            },
            cancellationToken);
    }

    [HttpGet("{projectId}/local-authority-search")]
    [WorkflowState(ProjectWorkflowState.LocalAuthoritySearch)]
    public IActionResult LocalAuthoritySearch([FromRoute] string projectId)
    {
        return View(nameof(LocalAuthoritySearch));
    }

    [HttpPost("{projectId}/local-authority-search")]
    [WorkflowState(ProjectWorkflowState.LocalAuthoritySearch)]
    public async Task<IActionResult> LocalAuthoritySearch([FromRoute] string projectId, [FromQuery] string phrase, CancellationToken cancellationToken)
    {
        return await Continue(new { projectId, phrase });
    }

    [HttpGet("{projectId}/local-authority-result")]
    [WorkflowState(ProjectWorkflowState.LocalAuthorityResult)]
    public IActionResult LocalAuthorityResult([FromRoute] string projectId)
    {
        return View(nameof(LocalAuthorityResult));
    }

    [HttpPost("{projectId}/local-authority-result")]
    [WorkflowState(ProjectWorkflowState.LocalAuthorityResult)]
    public async Task<IActionResult> LocalAuthorityResult([FromRoute] string projectId, [FromQuery] string phrase, CancellationToken cancellationToken)
    {
        return await Continue(new { projectId, phrase });
    }

    [HttpGet("{projectId}/local-authority-not-found")]
    [WorkflowState(ProjectWorkflowState.LocalAuthorityNotFound)]
    public IActionResult LocalAuthorityNotFound([FromRoute] string projectId)
    {
        return View(nameof(LocalAuthorityNotFound));
    }

    [HttpGet("{projectId}/local-authority-confirm")]
    [WorkflowState(ProjectWorkflowState.LocalAuthorityConfirm)]
    public IActionResult LocalAuthorityConfirm([FromRoute] string projectId)
    {
        return View(nameof(LocalAuthorityConfirm));
    }

    [HttpPost("{projectId}/local-authority-confirm")]
    [WorkflowState(ProjectWorkflowState.LocalAuthorityConfirm)]
    public async Task<IActionResult> LocalAuthorityConfirm([FromRoute] string projectId, CancellationToken cancellationToken)
    {
        return await Continue(new { projectId });
    }

    [HttpGet("{projectId}/homes-number")]
    [WorkflowState(ProjectWorkflowState.HomesNumber)]
    public async Task<IActionResult> HomesNumber([FromRoute] string projectId, CancellationToken cancellationToken)
    {
        return View(await GetProjectDetails(projectId, cancellationToken));
    }

    [HttpPost("{projectId}/homes-number")]
    [WorkflowState(ProjectWorkflowState.HomesNumber)]
    public async Task<IActionResult> HomesNumber([FromRoute] string projectId, ProjectDetails model, CancellationToken cancellationToken)
    {
        return await ExecuteProjectCommand(
            new ProvideHomesNumberCommand(new FrontDoorProjectId(projectId), model.HomesNumber),
            nameof(HomesNumber),
            project =>
            {
                project.HomesNumber = model.HomesNumber;
                return project;
            },
            cancellationToken);
    }

    [HttpGet("{projectId}/progress")]
    [WorkflowState(ProjectWorkflowState.Progress)]
    public async Task<IActionResult> Progress([FromRoute] string projectId, CancellationToken cancellationToken)
    {
        return View(await GetProjectDetails(projectId, cancellationToken));
    }

    [HttpPost("{projectId}/progress")]
    [WorkflowState(ProjectWorkflowState.Progress)]
    public async Task<IActionResult> Progress([FromRoute] string projectId, ProjectDetails model, CancellationToken cancellationToken)
    {
        return await ExecuteProjectCommand(
            new ProvideIsSupportRequiredCommand(new FrontDoorProjectId(projectId), model.IsSupportRequired),
            nameof(Progress),
            project =>
            {
                project.IsSupportRequired = model.IsSupportRequired;
                return project;
            },
            cancellationToken);
    }

    [HttpGet("{projectId}/requires-funding")]
    [WorkflowState(ProjectWorkflowState.RequiresFunding)]
    public async Task<IActionResult> RequiresFunding([FromRoute] string projectId, CancellationToken cancellationToken)
    {
        return View(await GetProjectDetails(projectId, cancellationToken));
    }

    [HttpPost("{projectId}/requires-funding")]
    [WorkflowState(ProjectWorkflowState.RequiresFunding)]
    public async Task<IActionResult> RequiresFunding([FromRoute] string projectId, ProjectDetails model, CancellationToken cancellationToken)
    {
        return await ExecuteProjectCommand(
            new ProvideIsFundingRequiredCommand(new FrontDoorProjectId(projectId), model.IsFundingRequired),
            nameof(RequiresFunding),
            project =>
            {
                project.IsFundingRequired = model.IsFundingRequired;
                return project;
            },
            cancellationToken);
    }

    [HttpGet("{projectId}/funding-amount")]
    [WorkflowState(ProjectWorkflowState.FundingAmount)]
    public async Task<IActionResult> FundingAmount([FromRoute] string projectId, CancellationToken cancellationToken)
    {
        return View(await GetProjectDetails(projectId, cancellationToken));
    }

    [HttpPost("{projectId}/funding-amount")]
    [WorkflowState(ProjectWorkflowState.FundingAmount)]
    public async Task<IActionResult> FundingAmount([FromRoute] string projectId, ProjectDetails model, CancellationToken cancellationToken)
    {
        return await ExecuteProjectCommand(
            new ProvideRequiredFundingCommand(new FrontDoorProjectId(projectId), model.RequiredFunding),
            nameof(FundingAmount),
            project =>
            {
                project.RequiredFunding = model.RequiredFunding;
                return project;
            },
            cancellationToken);
    }

    [HttpGet("{projectId}/profit")]
    [WorkflowState(ProjectWorkflowState.Profit)]
    public async Task<IActionResult> Profit([FromRoute] string projectId, CancellationToken cancellationToken)
    {
        return View(await GetProjectDetails(projectId, cancellationToken));
    }

    [HttpPost("{projectId}/profit")]
    [WorkflowState(ProjectWorkflowState.Profit)]
    public async Task<IActionResult> Profit([FromRoute] string projectId, ProjectDetails model, CancellationToken cancellationToken)
    {
        return await Continue(new { projectId });
    }

    [HttpGet("{projectId}/expected-start")]
    [WorkflowState(ProjectWorkflowState.ExpectedStart)]
    public async Task<IActionResult> ExpectedStart([FromRoute] string projectId, CancellationToken cancellationToken)
    {
        return View(await GetProjectDetails(projectId, cancellationToken));
    }

    [HttpPost("{projectId}/expected-start")]
    [WorkflowState(ProjectWorkflowState.ExpectedStart)]
    public async Task<IActionResult> ExpectedStart([FromRoute] string projectId, ProjectDetails model, CancellationToken cancellationToken)
    {
        return await Continue(new { projectId });
    }

    [HttpGet("{projectId}/check-answers")]
    [WorkflowState(ProjectWorkflowState.CheckAnswers)]
    public async Task<IActionResult> CheckAnswers([FromRoute] string projectId, CancellationToken cancellationToken)
    {
        return View(await GetProjectDetails(projectId, cancellationToken));
    }

    [HttpPost("{projectId}/complete")]
    [WorkflowState(ProjectWorkflowState.CheckAnswers)]
    public IActionResult Complete([FromRoute] string projectId, ProjectDetails model)
    {
        return RedirectToAction("Index", "Account");
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
            async () => await ReturnToAccountOrContinue(async () => await ContinueWithRedirect(routeData ?? new { projectId = projectId.Value })),
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

    private async Task<IActionResult> ReturnToAccountOrContinue(Func<Task<IActionResult>> onContinue)
    {
        if (Request.IsSaveAndReturnAction())
        {
            return RedirectToAction("Index", "Account");
        }

        return await onContinue();
    }
}
