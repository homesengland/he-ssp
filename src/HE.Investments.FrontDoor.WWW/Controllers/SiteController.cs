using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.FrontDoor.Contract.Project;
using HE.Investments.FrontDoor.Contract.Site;
using HE.Investments.FrontDoor.Contract.Site.Queries;
using HE.Investments.FrontDoor.WWW.Workflows;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
    public IActionResult NewName([FromRoute] string projectId)
    {
        return View("Name");
    }

    [HttpPost("")]
    public IActionResult NewName([FromRoute] string projectId, string? name)
    {
        return RedirectToAction("HomesNumber", "Site", new { projectId, siteId = Guid.NewGuid().ToString() });
    }

    [HttpGet("{siteId}/name")]
    [WorkflowState(SiteWorkflowState.Name)]
    public IActionResult Name([FromRoute] string projectId, [FromRoute] string siteId)
    {
        return View("Name");
    }

    [HttpPost("{siteId}/name")]
    [WorkflowState(SiteWorkflowState.Name)]
    public async Task<IActionResult> Name([FromRoute] string projectId, [FromRoute] string siteId, string? name)
    {
        return await Continue(new { projectId, siteId });
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
        return await Continue(new { projectId, siteId });
    }

    [HttpGet("{siteId}/local-authority-search")]
    [WorkflowState(SiteWorkflowState.LocalAuthoritySearch)]
    public async Task<IActionResult> LocalAuthoritySearch([FromRoute] string projectId, [FromRoute] string siteId, CancellationToken cancellationToken)
    {
        return View(nameof(LocalAuthoritySearch), await GetSiteDetails(projectId, siteId, cancellationToken));
    }

    [HttpPost("{siteId}/local-authority-search")]
    [WorkflowState(SiteWorkflowState.LocalAuthoritySearch)]
    public async Task<IActionResult> LocalAuthoritySearch([FromRoute] string projectId, [FromRoute] string siteId, [FromQuery] string phrase, CancellationToken cancellationToken)
    {
        return await Continue(new { projectId, siteId, phrase });
    }

    [HttpGet("{siteId}/local-authority-result")]
    [WorkflowState(SiteWorkflowState.LocalAuthorityResult)]
    public async Task<IActionResult> LocalAuthorityResult([FromRoute] string projectId, [FromRoute] string siteId, CancellationToken cancellationToken)
    {
        return View(nameof(LocalAuthorityResult), await GetSiteDetails(projectId, siteId, cancellationToken));
    }

    [HttpPost("{siteId}/local-authority-result")]
    [WorkflowState(SiteWorkflowState.LocalAuthorityResult)]
    public async Task<IActionResult> LocalAuthorityResult([FromRoute] string projectId, [FromRoute] string siteId, [FromQuery] string phrase, CancellationToken cancellationToken)
    {
        return await Continue(new { projectId, siteId, phrase });
    }

    [HttpGet("{siteId}/local-authority-not-found")]
    [WorkflowState(SiteWorkflowState.LocalAuthorityNotFound)]
    public IActionResult LocalAuthorityNotFound([FromRoute] string projectId, [FromRoute] string siteId)
    {
        return View(nameof(LocalAuthorityNotFound));
    }

    [HttpGet("{siteId}/local-authority-confirm")]
    [WorkflowState(SiteWorkflowState.LocalAuthorityConfirm)]
    public async Task<IActionResult> LocalAuthorityConfirm([FromRoute] string projectId, [FromRoute] string siteId, CancellationToken cancellationToken)
    {
        return View(nameof(LocalAuthorityConfirm), await GetSiteDetails(projectId, siteId, cancellationToken));
    }

    [HttpPost("{siteId}/local-authority-confirm")]
    [WorkflowState(SiteWorkflowState.LocalAuthorityConfirm)]
    public async Task<IActionResult> LocalAuthorityConfirm([FromRoute] string projectId, [FromRoute] string siteId, SiteDetails model, CancellationToken cancellationToken)
    {
        return await Continue(new { projectId, siteId });
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
        return await Continue(new { projectId, siteId });
    }

    [HttpGet("{siteId}/add-another-site")]
    [WorkflowState(SiteWorkflowState.AddAnotherSite)]
    public async Task<IActionResult> AddAnotherSite([FromRoute] string projectId, [FromRoute] string siteId, CancellationToken cancellationToken)
    {
        return View(nameof(AddAnotherSite), await GetSiteDetails(projectId, siteId, cancellationToken));
    }

    [HttpPost("{siteId}/add-another-site")]
    [WorkflowState(SiteWorkflowState.AddAnotherSite)]
    public IActionResult AddAnotherSite([FromRoute] string projectId, [FromRoute] string siteId, SiteDetails model)
    {
        return RedirectToAction("Progress", "Project", new { projectId });
    }

    protected override Task<IStateRouting<SiteWorkflowState>> Routing(SiteWorkflowState currentState, object? routeData = null)
    {
        return Task.FromResult<IStateRouting<SiteWorkflowState>>(new SiteWorkflow(currentState));
    }

    private async Task<SiteDetails> GetSiteDetails(string projectId, string siteId, CancellationToken cancellationToken)
    {
        var siteDetails = await _mediator.Send(new GetSiteDetailsQuery(new FrontDoorProjectId(projectId), new FrontDoorSiteId(siteId)), cancellationToken);
        ViewBag.ProjectName = siteDetails.ProjectName;
        return siteDetails;
    }
}
