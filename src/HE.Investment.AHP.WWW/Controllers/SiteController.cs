using He.AspNetCore.Mvc.Gds.Components.Extensions;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Contract.Site.Queries;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using HE.Investments.Common.WWW.Extensions;
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
    [HttpGet("{siteId}/name")]
    [WorkflowState(SiteWorkflowState.Name)]
    public async Task<IActionResult> Name(string? siteId, CancellationToken cancellationToken)
    {
        SiteModel siteModel = new();
        if (siteId.IsProvided())
        {
            siteModel = await _mediator.Send(new GetSiteQuery(siteId!), cancellationToken);
        }

        return View("Name", siteModel);
    }

    [HttpPost("name")]
    [HttpPost("{siteId}/name")]
    [WorkflowState(SiteWorkflowState.Name)]
    public async Task<IActionResult> NamePost(string? siteId, SiteModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ProvideNameCommand(siteId ?? model.Id, model.Name), cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("Name", model);
        }

        return await Continue(new { siteId });
    }

    [HttpGet("{siteId}/section-106-agreement")]
    [WorkflowState(SiteWorkflowState.Section106GeneralAgreement)]
    public async Task<IActionResult> Section106Agreement([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await _mediator.Send(new GetSiteQuery(siteId), cancellationToken);
        return View("Section106Agreement", siteModel);
    }

    [HttpPost("{siteId}/section-106-agreement")]
    [WorkflowState(SiteWorkflowState.Section106GeneralAgreement)]
    public async Task<IActionResult> Section106Agreement([FromRoute] string siteId, SiteModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ProvideSection106AgreementCommand(new SiteId(siteId), model.Section106GeneralAgreement), cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("Section106Agreement", model);
        }

        return await Continue(new { siteId });
    }

    [HttpGet("{siteId}/section-106-affordable-housing")]
    [WorkflowState(SiteWorkflowState.Section106AffordableHousing)]
    public async Task<IActionResult> Section106AffordableHousing([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await _mediator.Send(new GetSiteQuery(siteId), cancellationToken);
        return View("Section106AffordableHousing", siteModel);
    }

    [HttpPost("{siteId}/section-106-affordable-housing")]
    [WorkflowState(SiteWorkflowState.Section106AffordableHousing)]
    public async Task<IActionResult> Section106AffordableHousing([FromRoute] string siteId, SiteModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ProvideSection106AffordableHousingCommand(new SiteId(siteId), model.Section106AffordableHousing), cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("Section106AffordableHousing", model);
        }

        return await Continue(new { siteId });
    }

    [HttpGet("{siteId}/section-106-only-affordable-housing")]
    [WorkflowState(SiteWorkflowState.Section106OnlyAffordableHousing)]
    public async Task<IActionResult> Section106OnlyAffordableHousing([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await _mediator.Send(new GetSiteQuery(siteId), cancellationToken);
        return View("Section106OnlyAffordableHousing", siteModel);
    }

    [HttpPost("{siteId}/section-106-only-affordable-housing")]
    [WorkflowState(SiteWorkflowState.Section106OnlyAffordableHousing)]
    public async Task<IActionResult> Section106OnlyAffordableHousing([FromRoute] string siteId, SiteModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ProvideSection106OnlyAffordableHousingCommand(new SiteId(siteId), model.Section106OnlyAffordableHousing), cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("Section106OnlyAffordableHousing", model);
        }

        return await Continue(new { siteId });
    }

    [HttpGet("{siteId}/section-106-additional-affordable-housing")]
    [WorkflowState(SiteWorkflowState.Section106AdditionalAffordableHousing)]
    public async Task<IActionResult> Section106AdditionalAffordableHousing([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await _mediator.Send(new GetSiteQuery(siteId), cancellationToken);
        return View("Section106AdditionalAffordableHousing", siteModel);
    }

    [HttpPost("{siteId}/section-106-additional-affordable-housing")]
    [WorkflowState(SiteWorkflowState.Section106AdditionalAffordableHousing)]
    public async Task<IActionResult> Section106AdditionalAffordableHousing([FromRoute] string siteId, SiteModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ProvideSection106AdditionalAffordableHousingCommand(new SiteId(siteId), model.Section106AdditionalAffordableHousing), cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("Section106OAdditionalAffordableHousing", model);
        }

        return await Continue(new { siteId });
    }

    [HttpGet("{siteId}/back")]
    public async Task<IActionResult> Back([FromRoute] string siteId, SiteWorkflowState currentPage)
    {
        return await Back(currentPage, new { siteId });
    }

    protected override async Task<IStateRouting<SiteWorkflowState>> Routing(SiteWorkflowState currentState, object? routeData = null)
    {
        SiteModel? siteModel = null;
        var siteId = Request.GetRouteValue("siteId")
                            ?? routeData?.GetPropertyValue<string>("siteId")
                            ?? string.Empty;
        if (siteId.IsNotNullOrEmpty())
        {
            siteModel = await _mediator.Send(new GetSiteQuery(siteId));
        }

        return await Task.FromResult<IStateRouting<SiteWorkflowState>>(new SiteWorkflow(currentState, siteModel));
    }
}
