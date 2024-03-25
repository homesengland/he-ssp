using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Validators;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.FrontDoor.Contract.LocalAuthority;
using HE.Investments.FrontDoor.Contract.LocalAuthority.Queries;
using HE.Investments.FrontDoor.Contract.Project.Queries;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.FrontDoor.WWW.Extensions;
using HE.Investments.FrontDoor.WWW.Models;
using HE.Investments.FrontDoor.WWW.Workflows;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.FrontDoor.WWW.Controllers;

[AuthorizeWithCompletedProfile]
[Route("local-authority")]
public class LocalAuthorityController : WorkflowController<LocalAuthorityWorkflowState>
{
    private readonly IMediator _mediator;

    public LocalAuthorityController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("back")]
    public async Task<IActionResult> Back([FromQuery] string projectId, [FromQuery] string? siteId, LocalAuthorityWorkflowState currentPage)
    {
        if (currentPage == LocalAuthorityWorkflowState.Search && !string.IsNullOrWhiteSpace(siteId))
        {
            return RedirectToAction("HomesNumber", "Site", new { projectId, siteId });
        }

        if (currentPage == LocalAuthorityWorkflowState.Search && string.IsNullOrWhiteSpace(siteId))
        {
            return RedirectToAction("GeographicFocus", "Project", new { projectId });
        }

        return await Back(currentPage, new { projectId, siteId });
    }

    [HttpGet("search")]
    [WorkflowState(LocalAuthorityWorkflowState.Search)]
    public async Task<IActionResult> Search([FromQuery] string projectId, [FromQuery] string? siteId, CancellationToken cancellationToken)
    {
        await GetProjectDetails(projectId, cancellationToken);
        return View(nameof(Search), new LocalAuthoritySearchViewModel(string.Empty, projectId, siteId));
    }

    [HttpPost("search")]
    [WorkflowState(LocalAuthorityWorkflowState.Search)]
    public async Task<IActionResult> Search([FromForm] string projectId, [FromForm] string? siteId, [FromForm] string phrase, [FromQuery] string? redirect)
    {
        var optional = this.GetOptionalParameterFromRoute();
        if (string.IsNullOrEmpty(phrase))
        {
            ModelState.Clear();
            ModelState.AddModelError(nameof(LocalAuthoritySearchViewModel.Phrase), ValidationErrorMessage.MustProvideRequiredField("local authority"));
            return View(nameof(Search), new LocalAuthoritySearchViewModel(phrase, projectId, siteId));
        }

        return await Continue(new { projectId, siteId, phrase, redirect, optional });
    }

    [HttpGet("search-result")]
    [WorkflowState(LocalAuthorityWorkflowState.SearchResult)]
    public async Task<IActionResult> SearchResult([FromQuery] string projectId, [FromQuery] string? siteId, [FromQuery] string? phrase, [FromQuery] int? page, CancellationToken cancellationToken)
    {
        await GetProjectDetails(projectId, cancellationToken);
        var projectLocalAuthorities = await _mediator.Send(new GetLocalAuthoritiesQuery(new PaginationRequest(page ?? 1), phrase ?? string.Empty), cancellationToken);

        if (!projectLocalAuthorities.Items.Any())
        {
            return RedirectToAction("NotFound", "LocalAuthority", new { projectId, siteId });
        }

        return View(nameof(SearchResult), (projectLocalAuthorities, projectId, siteId, phrase));
    }

    [HttpGet("not-found")]
    [WorkflowState(LocalAuthorityWorkflowState.NotFound)]
    public IActionResult NotFound([FromQuery] string projectId, [FromQuery] string? siteId)
    {
        return View(nameof(NotFound), (projectId, siteId));
    }

    protected override Task<IStateRouting<LocalAuthorityWorkflowState>> Routing(LocalAuthorityWorkflowState currentState, object? routeData = null)
    {
        return Task.FromResult<IStateRouting<LocalAuthorityWorkflowState>>(new ProjectLocalAuthorityWorkflow(currentState));
    }

    private async Task GetProjectDetails(string projectId, CancellationToken cancellationToken)
    {
        var projectDetails = await _mediator.Send(new GetProjectDetailsQuery(new FrontDoorProjectId(projectId)), cancellationToken);
        ViewBag.ProjectName = projectDetails.Name;
    }
}
