using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Validators;
using HE.Investments.Common.WWW.Controllers;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.FrontDoor.Contract.LocalAuthority.Queries;
using HE.Investments.FrontDoor.Contract.Project.Queries;
using HE.Investments.FrontDoor.Contract.Site;
using HE.Investments.FrontDoor.Contract.Site.Commands;
using HE.Investments.FrontDoor.Contract.Site.Queries;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.WWW.Constants;
using HE.Investments.FrontDoor.WWW.Extensions;
using HE.Investments.FrontDoor.WWW.Models;
using HE.Investments.FrontDoor.WWW.Workflows;
using HE.Investments.Organisation.LocalAuthorities.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SiteDetails = HE.Investments.FrontDoor.Contract.Site.SiteDetails;

namespace HE.Investments.FrontDoor.WWW.Controllers;

[AuthorizeWithCompletedProfile]
[Route("project/{projectId}/site")]
public class SiteController : WorkflowController<SiteWorkflowState>
{
    private readonly IMediator _mediator;

    public SiteController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{siteId}/back")]
    public async Task<IActionResult> Back([FromRoute] string projectId, [FromRoute] string siteId, SiteWorkflowState currentPage)
    {
        return await Back(currentPage, new { projectId, siteId });
    }

    [HttpGet("")]
    public async Task<IActionResult> NewName([FromRoute] string projectId, CancellationToken cancellationToken)
    {
        var project = await _mediator.Send(new GetProjectDetailsQuery(FrontDoorProjectId.From(projectId)), cancellationToken);
        ViewBag.ProjectName = project.Name;
        return View("Name");
    }

    [HttpPost("")]
    public async Task<IActionResult> NewName([FromRoute] string projectId, string? name, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateSiteCommand(FrontDoorProjectId.From(projectId), name), cancellationToken);
        if (result.HasValidationErrors)
        {
            var project = await _mediator.Send(new GetProjectDetailsQuery(FrontDoorProjectId.From(projectId)), cancellationToken);
            ViewBag.ProjectName = project.Name;
            ModelState.AddValidationErrors(result);
            return View(nameof(Name), name);
        }

        return await ContinueSite(SiteWorkflowState.Name, projectId, result.ReturnedData.Value);
    }

    [HttpGet("{siteId}/name")]
    [WorkflowState(SiteWorkflowState.Name)]
    public async Task<IActionResult> Name([FromRoute] string projectId, [FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteDetails = await GetSiteDetails(projectId, siteId, cancellationToken);
        return View("Name", siteDetails.Name);
    }

    [HttpPost("{siteId}/name")]
    [WorkflowState(SiteWorkflowState.Name)]
    public async Task<IActionResult> Name([FromRoute] string projectId, [FromRoute] string siteId, string? name, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand(
            new ProvideSiteNameCommand(
                FrontDoorProjectId.From(projectId),
                FrontDoorSiteId.From(siteId),
                name),
            nameof(Name),
            project => View(nameof(Name), name),
            cancellationToken);
    }

    [HttpGet("{siteId}/homes-number")]
    [WorkflowState(SiteWorkflowState.HomesNumber)]
    public async Task<IActionResult> HomesNumber([FromRoute] string projectId, [FromRoute] string siteId, CancellationToken cancellationToken)
    {
        return View(nameof(HomesNumber), await GetSiteDetails(projectId, siteId, cancellationToken));
    }

    [HttpPost("{siteId}/homes-number")]
    [WorkflowState(SiteWorkflowState.HomesNumber)]
    public async Task<IActionResult> HomesNumber([FromRoute] string projectId, [FromRoute] string siteId, SiteDetails model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand(
            new ProvideSiteHomesNumberCommand(FrontDoorProjectId.From(projectId), FrontDoorSiteId.From(siteId), model.HomesNumber),
            nameof(HomesNumber),
            project =>
            {
                project.HomesNumber = model.HomesNumber;
                return project;
            },
            cancellationToken);
    }

    [HttpGet("{siteId}/local-authority-search")]
    [WorkflowState(SiteWorkflowState.LocalAuthoritySearch)]
    public IActionResult LocalAuthoritySearch([FromRoute] string projectId, [FromQuery] string? redirect, [FromRoute] string siteId)
    {
        var optional = this.GetOptionalParameterFromRoute();
        return RedirectToAction("Search", "LocalAuthority", new { projectId, siteId, redirect, optional });
    }

    [HttpGet("{siteId}/local-authority-confirm")]
    [WorkflowState(SiteWorkflowState.LocalAuthorityConfirm)]
    public async Task<IActionResult> LocalAuthorityConfirm([FromRoute] string projectId, [FromRoute] string siteId, [FromQuery] string? localAuthorityCode, CancellationToken cancellationToken)
    {
        var site = await GetSiteDetails(projectId, siteId, cancellationToken);
        var localAuthority = await _mediator.Send(new GetLocalAuthorityQuery(new LocalAuthorityCode(localAuthorityCode ?? site.LocalAuthorityCode!)), cancellationToken);
        bool? isConfirmed = site.LocalAuthorityCode == localAuthority.Code ? true : null;

        return View(nameof(LocalAuthorityConfirm), new LocalAuthorityViewModel(localAuthority.Code, localAuthority.Name, projectId, siteId, isConfirmed));
    }

    [HttpPost("{siteId}/local-authority-confirm")]
    [WorkflowState(SiteWorkflowState.LocalAuthorityConfirm)]
    public async Task<IActionResult> LocalAuthorityConfirm([FromRoute] string projectId, [FromRoute] string siteId, LocalAuthorityViewModel model, CancellationToken cancellationToken)
    {
        if (model.IsConfirmed == null)
        {
            ModelState.Clear();
            ModelState.AddModelError("IsConfirmed", "Select yes if the local authority is correct");

            await GetSiteDetails(projectId, siteId, cancellationToken);
            var localAuthority = await _mediator.Send(new GetLocalAuthorityQuery(new LocalAuthorityCode(model.LocalAuthorityCode)), cancellationToken);

            return View("LocalAuthorityConfirm", new LocalAuthorityViewModel(localAuthority.Code, localAuthority.Name, projectId, siteId));
        }

        if (model.IsConfirmed.Value)
        {
            return await ExecuteSiteCommand(
                new ProvideLocalAuthorityCommand(
                    FrontDoorProjectId.From(projectId),
                    FrontDoorSiteId.From(siteId),
                    new LocalAuthorityCode(model.LocalAuthorityCode),
                    model.LocalAuthorityName),
                nameof(LocalAuthorityConfirm),
                project => project,
                cancellationToken);
        }

        var optional = this.GetOptionalParameterFromRoute();
        return RedirectToAction("Search", "LocalAuthority", new { projectId, siteId, optional });
    }

    [HttpGet("{siteId}/planning-status")]
    [WorkflowState(SiteWorkflowState.PlanningStatus)]
    public async Task<IActionResult> PlanningStatus([FromRoute] string projectId, [FromRoute] string siteId, CancellationToken cancellationToken)
    {
        return View(nameof(PlanningStatus), await GetSiteDetails(projectId, siteId, cancellationToken));
    }

    [HttpPost("{siteId}/planning-status")]
    [WorkflowState(SiteWorkflowState.PlanningStatus)]
    public async Task<IActionResult> PlanningStatus([FromRoute] string projectId, [FromRoute] string siteId, SiteDetails model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand(
            new ProvidePlanningStatusCommand(FrontDoorProjectId.From(projectId), FrontDoorSiteId.From(siteId), model.PlanningStatus),
            nameof(PlanningStatus),
            project =>
            {
                project.PlanningStatus = model.PlanningStatus;
                return project;
            },
            cancellationToken);
    }

    [HttpGet("{siteId}/add-another-site")]
    [WorkflowState(SiteWorkflowState.AddAnotherSite)]
    public async Task<IActionResult> AddAnotherSite([FromRoute] string projectId, [FromRoute] string siteId, CancellationToken cancellationToken)
    {
        return View(nameof(AddAnotherSite), await GetSiteDetails(projectId, siteId, cancellationToken));
    }

    [HttpPost("{siteId}/add-another-site")]
    [WorkflowState(SiteWorkflowState.AddAnotherSite)]
    public async Task<IActionResult> AddAnotherSite([FromRoute] string projectId, [FromRoute] string siteId, SiteDetails model, CancellationToken cancellationToken)
    {
        if (model.AddAnotherSite == Contract.Site.AddAnotherSite.Yes)
        {
            return RedirectToAction("NewName", "Site", new { projectId });
        }

        if (model.AddAnotherSite == Contract.Site.AddAnotherSite.No)
        {
            return RedirectToAction("Progress", "Project", new { projectId });
        }

        ModelState.Clear();
        ModelState.AddValidationErrors(OperationResult.New().AddValidationError(nameof(SiteDetails.AddAnotherSite), "Select yes if you want to add another site"));
        return View(nameof(AddAnotherSite), await GetSiteDetails(projectId, siteId, cancellationToken));
    }

    [WorkflowState(SiteWorkflowState.RemoveSite)]
    [HttpGet("{siteId}/remove")]
    public IActionResult Remove()
    {
        return View();
    }

    [WorkflowState(SiteWorkflowState.RemoveSite)]
    [HttpPost("{siteId}/remove")]
    public async Task<IActionResult> Remove([FromRoute] string projectId, [FromRoute] string siteId, SiteDetails model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand(
            new RemoveSiteCommand(FrontDoorProjectId.From(projectId), FrontDoorSiteId.From(siteId), model.RemoveSiteAnswer),
            nameof(Remove),
            site => site,
            cancellationToken,
            new { projectId, siteId, remove = true });
    }

    protected override async Task<IStateRouting<SiteWorkflowState>> Routing(SiteWorkflowState currentState, object? routeData = null)
    {
        var projectId = routeData?.GetPropertyValue<string>("projectId") ?? this.GetProjectIdFromRoute().Value;
        var siteId = routeData?.GetPropertyValue<string>("siteId") ?? this.GetSiteIdFromRoute().Value;
        var isSiteRemoved = routeData?.GetPropertyValue<bool>("remove") ?? false;
        return isSiteRemoved
            ? new SiteWorkflow(currentState)
            : new SiteWorkflow(currentState, await GetSiteDetails(projectId, siteId, CancellationToken.None));
    }

    private async Task<SiteDetails> GetSiteDetails(string projectId, string siteId, CancellationToken cancellationToken)
    {
        var siteDetails = await _mediator.Send(new GetSiteDetailsQuery(FrontDoorProjectId.From(projectId), FrontDoorSiteId.From(siteId)), cancellationToken);
        ViewBag.ProjectName = siteDetails.ProjectName;
        return siteDetails;
    }

    private async Task<IActionResult> ExecuteSiteCommand<TViewModel>(
        IRequest<OperationResult> command,
        string viewName,
        Func<SiteDetails, TViewModel> createViewModelForError,
        CancellationToken cancellationToken,
        object? routeData = null)
    {
        var projectId = this.GetProjectIdFromRoute();
        var siteId = this.GetSiteIdFromRoute();
        var optional = this.GetOptionalParameterFromRoute();

        if (optional == OptionalParameter.AddSite && viewName == nameof(PlanningStatus))
        {
            routeData = new { projectId = projectId.Value, siteId = siteId.Value, redirect = OptionalParameter.CheckAnswers };
        }

        return await this.ExecuteCommand<TViewModel>(
            _mediator,
            command,
            async () => await ReturnToAccountOrContinue(async () => await ContinueWithRedirect(routeData ?? new { projectId = projectId.Value, siteId = siteId.Value, optional })),
            async () =>
            {
                var siteDetails = await GetSiteDetails(projectId.Value, siteId.Value, cancellationToken);
                var model = createViewModelForError(siteDetails);

                return View(viewName, model);
            },
            cancellationToken);
    }

    private async Task<IActionResult> ReturnToAccountOrContinue(Func<Task<IActionResult>> onContinue)
    {
        if (Request.IsSaveAndReturnAction())
        {
            return RedirectToAction("Index", "Account");
        }

        return await onContinue();
    }

    private async Task<IActionResult> ContinueSite(SiteWorkflowState state, string projectId, string siteId)
    {
        var optional = this.GetOptionalParameterFromRoute();
        return await Continue(state, new { projectId, siteId, optional });
    }
}
