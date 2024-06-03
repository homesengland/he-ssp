using HE.Investment.AHP.Contract.Project;
using HE.Investment.AHP.Contract.Project.Commands;
using HE.Investment.AHP.Contract.Project.Queries;
using HE.Investment.AHP.Contract.Site.Queries;
using HE.Investment.AHP.Domain.UserContext;
using HE.Investment.AHP.WWW.Models.Project;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.AHP.Consortium.Shared.UserContext;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.Programme.Contract.Enums;
using HE.Investments.Programme.Contract.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[AuthorizeWithCompletedProfile]
[Route("project")]
public class ProjectController : Controller
{
    private readonly IMediator _mediator;

    private readonly IConsortiumUserContext _consortiumUserContext;

    private readonly IConsortiumAccessContext _consortiumAccessContext;

    public ProjectController(IMediator mediator, IConsortiumUserContext consortiumUserContext, IConsortiumAccessContext consortiumAccessContext)
    {
        _mediator = mediator;
        _consortiumUserContext = consortiumUserContext;
        _consortiumAccessContext = consortiumAccessContext;
    }

    [HttpGet("start")]
    [AuthorizeWithCompletedProfile(ConsortiumAccessContext.EditApplications)]
    public async Task<IActionResult> Start([FromQuery] string fdProjectId, CancellationToken cancellationToken)
    {
        var userAccount = await _consortiumUserContext.GetSelectedAccount();
        if (userAccount.Consortium.HasNoConsortium || await _consortiumAccessContext.IsConsortiumLeadPartner())
        {
            var availableProgrammes = await _mediator.Send(new GetProgrammesQuery(ProgrammeType.Ahp), cancellationToken);
            return View(new ProjectBasicModel(fdProjectId, availableProgrammes[0]));
        }

        return RedirectToAction("ContactHomesEngland", "ConsortiumMember", new { consortiumId = userAccount.Consortium.ConsortiumId.Value });
    }

    [HttpPost("start")]
    [AuthorizeWithCompletedProfile(ConsortiumAccessContext.EditApplications)]
    public async Task<IActionResult> StartPost([FromQuery] string fdProjectId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CreateAhpProjectCommand(FrontDoorProjectId.From(fdProjectId)), cancellationToken);

        var response = await _mediator.Send(new GetSiteListQuery(new PaginationRequest(1, 1)), cancellationToken);
        if (response.Page.Items.Any())
        {
            return RedirectToAction("Select", "Site", new { projectId = fdProjectId });
        }

        return RedirectToAction("Start", "Site", new { projectId = fdProjectId });
    }

    [HttpGet("{projectId}")]
    public async Task<IActionResult> Details(string projectId)
    {
        return View("Details", await _mediator.Send(new GetProjectDetailsQuery(FrontDoorProjectId.From(projectId), new PaginationRequest(1, 100))));
    }

    [HttpGet("{projectId}/applications")]
    public async Task<IActionResult> Applications(string projectId, int? page)
    {
        return View("ListOfApplications", await _mediator.Send(new GetProjectDetailsQuery(FrontDoorProjectId.From(projectId), new PaginationRequest(page ?? 1))));
    }

    [HttpGet("{projectId}/sites")]
    public async Task<IActionResult> Sites(string projectId, [FromQuery] int? page)
    {
        return View("ListOfSites", await _mediator.Send(new GetProjectSitesQuery(FrontDoorProjectId.From(projectId), new PaginationRequest(page ?? 1))));
    }
}
