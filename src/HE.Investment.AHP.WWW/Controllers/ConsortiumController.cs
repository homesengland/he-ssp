using HE.Investment.AHP.WWW.Models.Consortium;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Contract.Commands;
using HE.Investments.AHP.Consortium.Contract.Queries;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using HE.Investments.Common.WWW.Controllers;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.Consortium.Shared.UserContext;
using HE.Investments.Programme.Contract;
using HE.Investments.Programme.Contract.Enums;
using HE.Investments.Programme.Contract.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[Route("{organisationId}/consortium")]
[AuthorizeWithCompletedProfile(ConsortiumAccessContext.ViewConsortium)]
public class ConsortiumController : WorkflowController<ConsortiumWorkflowState>
{
    private readonly IMediator _mediator;

    private readonly IConsortiumAccessContext _consortiumAccessContext;

    public ConsortiumController(IMediator mediator, IConsortiumAccessContext consortiumAccessContext)
    {
        _mediator = mediator;
        _consortiumAccessContext = consortiumAccessContext;
    }

    [HttpGet]
    [WorkflowState(ConsortiumWorkflowState.Index)]
    public async Task<IActionResult> Index()
    {
        var consortiumsList = await _mediator.Send(new GetConsortiumsListQuery());
        return View((consortiumsList, await _consortiumAccessContext.CanManageConsortium()));
    }

    [HttpGet("start")]
    [WorkflowState(ConsortiumWorkflowState.Start)]
    [AuthorizeWithCompletedProfile(ConsortiumAccessContext.ManageConsortium)]
    public IActionResult Start()
    {
        return View();
    }

    [HttpGet("programme")]
    [WorkflowState(ConsortiumWorkflowState.Programme)]
    [AuthorizeWithCompletedProfile(ConsortiumAccessContext.ManageConsortium)]
    public async Task<IActionResult> Programme(CancellationToken cancellationToken)
    {
        return View(new SelectProgramme(string.Empty, await GetOpenAhpProgrammes(cancellationToken)));
    }

    [HttpPost("programme")]
    [WorkflowState(ConsortiumWorkflowState.Programme)]
    [AuthorizeWithCompletedProfile(ConsortiumAccessContext.ManageConsortium)]
    public async Task<IActionResult> ProgrammePost(SelectProgramme model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new CreateConsortiumCommand(model.SelectedProgrammeId.IsProvided() ? ProgrammeId.From(model.SelectedProgrammeId) : null),
            cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("Programme", model with { AvailableProgrammes = await GetOpenAhpProgrammes(cancellationToken) });
        }

        return this.OrganisationRedirectToAction("SearchOrganisation", "ConsortiumMember", new { consortiumId = result.ReturnedData.Value });
    }

    protected override async Task<IStateRouting<ConsortiumWorkflowState>> Routing(ConsortiumWorkflowState currentState, object? routeData = null)
    {
        return await Task.FromResult<IStateRouting<ConsortiumWorkflowState>>(new ConsortiumWorkflow(currentState));
    }

    private async Task<IList<Programme>> GetOpenAhpProgrammes(CancellationToken cancellationToken)
    {
        var programmes = await _mediator.Send(new GetProgrammesQuery(ProgrammeType.Ahp), cancellationToken);

        return programmes.Where(x => x.IsOpenForApplications).ToList();
    }
}
