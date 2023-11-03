using HE.Investment.AHP.Contract.Application.Queries;
using HE.Investment.AHP.Contract.Scheme;
using HE.Investment.AHP.Contract.Scheme.Queries;
using HE.Investment.AHP.Domain.Scheme.Commands;
using HE.Investment.AHP.Domain.Scheme.ValueObjects;
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

    [HttpGet("{schemeId}/Back")]
    public Task<IActionResult> Back([FromRoute] string applicationId, [FromRoute] string schemeId, SchemeWorkflowState currentPage)
    {
        return Back(currentPage, new { applicationId, schemeId });
    }

    [WorkflowState(SchemeWorkflowState.Funding)]
    [HttpGet("funding")]
    [HttpGet("{schemeId}/funding")]
    public async Task<IActionResult> Funding([FromRoute] string applicationId, string schemeId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(applicationId), cancellationToken);
        if (string.IsNullOrWhiteSpace(schemeId))
        {
            return View("Funding", CreateModel(applicationId, application.Name, null));
        }

        var scheme = await _mediator.Send(new GetSchemeQuery(schemeId), cancellationToken);

        return View("Funding", CreateModel(applicationId, application.Name, scheme));
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

    [WorkflowState(SchemeWorkflowState.Affordability)]
    [HttpGet("{schemeId}/affordability")]
    public async Task<IActionResult> Affordability([FromRoute] string applicationId, string schemeId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(applicationId), cancellationToken);
        var scheme = await _mediator.Send(new GetSchemeQuery(schemeId), cancellationToken);

        return View("Affordability", CreateModel(applicationId, application.Name, scheme));
    }

    [WorkflowState(SchemeWorkflowState.Affordability)]
    [HttpPost("{schemeId}/affordability")]
    public async Task<IActionResult> Affordability(SchemeViewModel model, CancellationToken cancellationToken)
    {
        return await ExecuteCommand(
            new ChangeSchemeAffordabilityCommand(model.SchemeId, model.AffordabilityEvidence),
            model.ApplicationId,
            nameof(Affordability),
            model,
            cancellationToken);
    }

    [WorkflowState(SchemeWorkflowState.SalesRisk)]
    [HttpGet("{schemeId}/sales-risk")]
    public async Task<IActionResult> SalesRisk([FromRoute] string applicationId, string schemeId, CancellationToken cancellationToken)
    {
        var application = await _mediator.Send(new GetApplicationQuery(applicationId), cancellationToken);
        var scheme = await _mediator.Send(new GetSchemeQuery(schemeId), cancellationToken);

        return View("SalesRisk", CreateModel(applicationId, application.Name, scheme));
    }

    [WorkflowState(SchemeWorkflowState.SalesRisk)]
    [HttpPost("{schemeId}/sales-risk")]
    public async Task<IActionResult> SalesRisk(SchemeViewModel model, CancellationToken cancellationToken)
    {
        return await ExecuteCommand(
            new ChangeSchemeSalesRiskCommand(model.SchemeId, model.SalesRisk),
            model.ApplicationId,
            nameof(SalesRisk),
            model,
            cancellationToken);
    }

    protected override async Task<IStateRouting<SchemeWorkflowState>> Routing(SchemeWorkflowState currentState, object routeData = null)
    {
        return await Task.FromResult(new SchemeWorkflow(currentState));
    }

    private SchemeViewModel CreateModel(string applicationId, string applicationName, Scheme scheme)
    {
        return new SchemeViewModel(
            applicationId,
            applicationName,
            scheme?.SchemeId,
            scheme?.RequiredFunding,
            scheme?.HousesToDeliver,
            scheme?.AffordabilityEvidence,
            scheme?.SalesRisk);
    }

    private async Task<IActionResult> ExecuteCommand(
        IRequest<OperationResult<SchemeId>> command,
        string applicationId,
        string viewName,
        object model,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View(viewName, model);
        }

        return await Continue(new { applicationId, schemeId = result.ReturnedData.Value });
    }
}
