using HE.Investment.AHP.Contract.Application.Queries;
using HE.Investment.AHP.Contract.Scheme.Queries;
using HE.Investment.AHP.Domain.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.Workflows;
using HE.Investment.AHP.WWW.Models.Scheme;
using HE.InvestmentLoans.Common.Routing;
using HE.InvestmentLoans.Common.Validation;
using HE.Investments.Common.WWW.Routing;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[Authorize]
[Route("application/{applicationId}/scheme")]
public class SchemeController : WorkflowController<SchemeWorkflowState>
{
    private readonly IMediator _mediator;

    public SchemeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromRoute] string applicationId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(applicationId), cancellationToken);

        return View("Index", application.Name);
    }

    [WorkflowState(SchemeWorkflowState.Funding)]
    [HttpGet("funding")]
    [HttpGet("{schemeId}/funding")]
    public async Task<IActionResult> Funding([FromRoute] string applicationId, string schemeId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(applicationId), cancellationToken);
        if (string.IsNullOrWhiteSpace(schemeId))
        {
            return View("Funding", new SchemeViewModel(applicationId, application.Name, null, null, null));
        }

        var scheme = await _mediator.Send(new GetSchemeQuery(schemeId), cancellationToken);

        return View("Funding", new SchemeViewModel(applicationId, application.Name, scheme.SchemeId, scheme.RequiredFunding, scheme.HousesToDeliver));
    }

    [WorkflowState(SchemeWorkflowState.Funding)]
    [HttpPost("funding")]
    [HttpPost("{schemeId}/funding")]
    public async Task<IActionResult> Funding(SchemeViewModel model, CancellationToken cancellationToken)
    {
        var result = string.IsNullOrWhiteSpace(model.SchemeId)
            ? await _mediator.Send(new AddSchemeWithFundingCommand(model.ApplicationId, model.RequiredFunding, model.HousesToDeliver), cancellationToken)
            : await _mediator.Send(new ChangeSchemeFundingCommand(model.SchemeId, model.RequiredFunding, model.HousesToDeliver), cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View(model);
        }

        return await Continue(new { applicationId = model.ApplicationId, schemeId = result.ReturnedData.Value });
    }

    [WorkflowState(SchemeWorkflowState.Partner)]
    [HttpGet("{schemeId}/partner")]
    public IActionResult Partner([FromRoute] string applicationId, string schemeId, CancellationToken cancellationToken)
    {
        // TODO: add next page
        return View();
    }

    protected override async Task<IStateRouting<SchemeWorkflowState>> Routing(SchemeWorkflowState currentState, object routeData = null)
    {
        return await Task.FromResult(new SchemeWorkflow(currentState));
    }
}
