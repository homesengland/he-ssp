using HE.Investment.AHP.Contract.Project;
using HE.Investment.AHP.Contract.Project.Commands;
using HE.Investment.AHP.Contract.Project.Queries;
using HE.Investment.AHP.Contract.Site.Queries;
using HE.Investment.AHP.Domain.UserContext;
using HE.Investment.AHP.WWW.Models.Project;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.AHP.Consortium.Contract.Queries;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.FrontDoor.Shared.Project;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[AuthorizeWithCompletedProfile]
[Route("project")]
public class ProjectController : Controller
{
    private readonly IMediator _mediator;

    private readonly IAhpUserContext _ahpUserContext;

    private readonly IAhpAccessContext _ahpAccessContext;

    public ProjectController(IMediator mediator, IAhpUserContext ahpUserContext, IAhpAccessContext ahpAccessContext)
    {
        _mediator = mediator;
        _ahpUserContext = ahpUserContext;
        _ahpAccessContext = ahpAccessContext;
    }

    [HttpGet("start")]
    [AuthorizeWithCompletedProfile(AhpAccessContext.EditApplications)]
    public async Task<IActionResult> Start([FromQuery] string fdProjectId, CancellationToken cancellationToken)
    {
        var userAccount = await _ahpUserContext.GetSelectedAccount();
        if (userAccount.Consortium.HasNoConsortium || await _ahpAccessContext.IsConsortiumLeadPartner())
        {
            var availableProgrammes = await _mediator.Send(new GetAvailableProgrammesQuery(), cancellationToken);
            return View(new ProjectBasicModel(fdProjectId, availableProgrammes[0]));
        }

        return RedirectToAction("ContactHomesEngland", "ConsortiumMember", new { consortiumId = userAccount.Consortium.ConsortiumId.Value });
    }

    [HttpPost("start")]
    [AuthorizeWithCompletedProfile(AhpAccessContext.EditApplications)]
    public async Task<IActionResult> StartPost([FromQuery] string fdProjectId, CancellationToken cancellationToken)
    {
        var projectId = await _mediator.Send(new CreateAhpProjectCommand(FrontDoorProjectId.From(fdProjectId)), cancellationToken);

        var response = await _mediator.Send(new GetSiteListQuery(new PaginationRequest(1, 1)), cancellationToken);
        if (response.Page.Items.Any())
        {
            return RedirectToAction("Select", "Site", new { projectId });
        }

        return RedirectToAction("Start", "Site", new { projectId });
    }

    [HttpGet("{projectId}")]
    public async Task<IActionResult> Details(string projectId)
    {
        return View("Details", await _mediator.Send(new GetProjectDetailsQuery(new FrontDoorProjectId(projectId), new PaginationRequest(1, 100))));
    }

    [HttpGet("{projectId}/applications")]
    public async Task<IActionResult> Applications(string projectId, int? page)
    {
        return View("ListOfApplications", await _mediator.Send(new GetProjectDetailsQuery(new FrontDoorProjectId(projectId), new PaginationRequest(page ?? 1))));
    }

    [HttpGet("{projectId}/sites")]
    public async Task<IActionResult> Sites(string projectId, [FromQuery] int? page)
    {
        return View("ListOfSites", await _mediator.Send(new GetProjectSitesQuery(new FrontDoorProjectId(projectId), new PaginationRequest(page ?? 1))));
    }
}
