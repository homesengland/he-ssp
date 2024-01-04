using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Queries;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.WWW.Routing;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[Route("site")]
[AuthorizeWithCompletedProfile]
public class SiteController : WorkflowController<SiteWorkflowState>
{
    private readonly IMediator _mediator;

    public SiteController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [WorkflowState(SiteWorkflowState.Index)]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetSiteListQuery(), cancellationToken);
        return View("Index", response);
    }

    [HttpGet("{siteId}")]
    public async Task<IActionResult> Details(string siteId, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetSiteDetailsQuery(siteId), cancellationToken);
        return View("Details", response);
    }

    [HttpGet("start")]
    [WorkflowState(SiteWorkflowState.Start)]
    public IActionResult Start()
    {
        return View("Start");
    }

    [HttpPost("start")]
    [WorkflowState(SiteWorkflowState.Start)]
    public async Task<IActionResult> StartPost()
    {
        return await Continue();
    }

    [HttpGet("name")]
    [WorkflowState(SiteWorkflowState.Name)]
    public IActionResult Name()
    {
        return View("Name");
    }

    [HttpPost("name")]
    [WorkflowState(SiteWorkflowState.Name)]
    public async Task<IActionResult> NamePost()
    {
        return await Continue();
    }

    [HttpGet("back")]
    public Task<IActionResult> Back(SiteWorkflowState currentPage, string? siteId)
    {
        return Back(currentPage, new { siteId });
    }

    protected override Task<IStateRouting<SiteWorkflowState>> Routing(SiteWorkflowState currentState, object? routeData = null)
    {
        return Task.FromResult<IStateRouting<SiteWorkflowState>>(new SiteWorkflow(currentState));
    }
}
