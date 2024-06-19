using HE.Investment.AHP.Contract.Project.Queries;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Consortium.Shared.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[ConsortiumAuthorize]
[Route("projects")]
public class ProjectsController : Controller
{
    private readonly IMediator _mediator;

    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index([FromQuery] int? page, CancellationToken cancellationToken)
    {
        var projectsList = await _mediator.Send(new GetProjectsListQuery(new PaginationRequest(page ?? 1, 3)), cancellationToken);

        return View("Index", projectsList);
    }
}
