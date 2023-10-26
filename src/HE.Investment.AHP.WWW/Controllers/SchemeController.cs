using HE.Investment.AHP.Contract.Scheme;
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
[Route("scheme")]
public class SchemeController : WorkflowController<SchemeWorkflowState>
{
    private readonly IMediator _mediator;

    public SchemeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("list")]
    public async Task<IActionResult> List(CancellationToken cancellationToken)
    {
        var schemes = await _mediator.Send(new GetSchemesQuery(), cancellationToken);

        return View("List", schemes.Select(CreateModel).ToList());
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        // TODO: Fetch site name when implemented
        var siteName = "Church road";
        return View("Index", siteName);
    }

    [WorkflowState(SchemeWorkflowState.SchemeName)]
    [HttpGet("name/{schemeId?}")]
    public async Task<IActionResult> Name(string schemeId, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(schemeId))
        {
            return View();
        }

        var scheme = await _mediator.Send(new GetSchemeQuery(schemeId), cancellationToken);

        return View("Name", CreateModel(scheme));
    }

    [WorkflowState(SchemeWorkflowState.SchemeName)]
    [HttpPost("name/{schemeId?}")]
    public async Task<IActionResult> Name(SchemeModel model, CancellationToken cancellationToken)
    {
        var result = string.IsNullOrWhiteSpace(model.Id)
            ? await _mediator.Send(new CreateSchemeCommand(model.Name), cancellationToken)
            : await _mediator.Send(new UpdateSchemeNameCommand(model.Id, model.Name), cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View(model);
        }

        return await Continue(new { schemeId = result.ReturnedData.Value });
    }

    [WorkflowState(SchemeWorkflowState.SchemeTenure)]
    [HttpGet("tenure/{schemeId}")]
    public async Task<IActionResult> Tenure(string schemeId, CancellationToken cancellationToken)
    {
        var scheme = await _mediator.Send(new GetSchemeQuery(schemeId), cancellationToken);

        return View("Tenure", CreateModel(scheme));
    }

    [WorkflowState(SchemeWorkflowState.SchemeTenure)]
    [HttpPost("tenure/{schemeId}")]
    public async Task<IActionResult> Tenure(SchemeModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateSchemeTenureCommand(model.Id, model.Tenure), cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View(model);
        }

        return RedirectToAction("List");
    }

    protected override async Task<IStateRouting<SchemeWorkflowState>> Routing(SchemeWorkflowState currentState, object routeData = null)
    {
        return await Task.FromResult(new SchemeWorkflow(currentState));
    }

    private static SchemeModel CreateModel(Scheme scheme)
    {
        return new SchemeModel(scheme.Id, scheme.Name, scheme.Tenure);
    }
}
