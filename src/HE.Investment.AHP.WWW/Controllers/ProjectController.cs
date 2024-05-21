using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.Contract.Project;
using HE.Investment.AHP.Contract.Project.Queries;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[AuthorizeWithCompletedProfile]
[Route("project")]
public class ProjectController : Controller
{
    private readonly IMediator _mediator;

    public ProjectController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{projectId}")]
    public async Task<IActionResult> Details(string projectId)
    {
        return View("Details", await _mediator.Send(new GetProjectDetailsQuery(new AhpProjectId(projectId), new PaginationRequest(1, 100))));
    }

    [HttpGet("{projectId}/applications")]
    public async Task<IActionResult> Applications(string projectId, int? page)
    {
        return View("ListOfApplications", await _mediator.Send(new GetProjectDetailsQuery(new AhpProjectId(projectId), new PaginationRequest(page ?? 1))));
    }

    [HttpGet("{projectId}/sites")]
    public async Task<IActionResult> Sites(string projectId, [FromQuery] int? page)
    {
        return View("ListOfSites", await _mediator.Send(new GetProjectSitesQuery(new AhpProjectId(projectId), new PaginationRequest(page ?? 1))));
    }
}
