using HE.Investment.AHP.Contract.AhpProgramme;
using HE.Investment.AHP.Contract.Project.Commands;
using HE.Investment.AHP.Contract.Project.Queries;
using HE.Investment.AHP.Contract.Site.Queries;
using HE.Investment.AHP.WWW.Models.Project;
using HE.Investments.AHP.Allocation.Contract.Project.Queries;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.WWW.Controllers;
using HE.Investments.Consortium.Shared.Authorization;
using HE.Investments.Consortium.Shared.UserContext;
using HE.Investments.FrontDoor.Shared.Project;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[ConsortiumAuthorize]
[Route("{organisationId}/project")]
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
    [ConsortiumAuthorize]
    public async Task<IActionResult> Start([FromQuery] string fdProjectId, CancellationToken cancellationToken)
    {
        var userAccount = await _consortiumUserContext.GetSelectedAccount();
        if (userAccount.Consortium.HasNoConsortium || await _consortiumAccessContext.IsConsortiumLeadPartner())
        {
            var ahpProgramme = await _mediator.Send(new GetTheAhpProgrammeQuery(), cancellationToken);
            return View(new ProjectStartModel(
                fdProjectId,
                ahpProgramme,
                userAccount.Role() != Investments.Account.Api.Contract.User.UserRole.Limited));
        }

        return this.OrganisationRedirectToAction("ContactHomesEngland", "ConsortiumMember", new { consortiumId = userAccount.Consortium.ConsortiumId.Value });
    }

    [HttpPost("start")]
    [ConsortiumAuthorize(ConsortiumAccessContext.Edit)]
    public async Task<IActionResult> StartPost([FromQuery] string fdProjectId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new TryCreateAhpProjectCommand(FrontDoorProjectId.From(fdProjectId)), cancellationToken);

        var response = await _mediator.Send(new GetSiteListQuery(new PaginationRequest(1, 1)), cancellationToken);
        if (response.Page.Items.Any())
        {
            return this.OrganisationRedirectToAction("Select", "Site", new { projectId = fdProjectId, isAfterFdProject = true });
        }

        return this.OrganisationRedirectToAction("Start", "Site", new { projectId = fdProjectId });
    }

    [HttpGet("{projectId}")]
    public async Task<IActionResult> Details(string projectId)
    {
        return View("Details", await _mediator.Send(new GetProjectDetailsQuery(FrontDoorProjectId.From(projectId), new PaginationRequest(1, 100))));
    }

    [HttpGet("{projectId}/applications")]
    public async Task<IActionResult> Applications(string projectId, int? page)
    {
        return View(
            "ListOfApplications",
            await _mediator.Send(new GetProjectDetailsQuery(FrontDoorProjectId.From(projectId), new PaginationRequest(page ?? 1))));
    }

    [HttpGet("{projectId}/allocations")]
    public async Task<IActionResult> Allocations(string projectId, int? page)
    {
        return View(
            "ListOfAllocations",
            await _mediator.Send(new GetProjectAllocationsQuery(FrontDoorProjectId.From(projectId), new PaginationRequest(page ?? 1))));
    }

    [HttpGet("{projectId}/sites")]
    public async Task<IActionResult> Sites(string projectId, [FromQuery] int? page)
    {
        return View("ListOfSites", await _mediator.Send(new GetProjectSitesQuery(FrontDoorProjectId.From(projectId), new PaginationRequest(page ?? 1))));
    }
}
