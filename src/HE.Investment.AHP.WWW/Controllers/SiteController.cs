using He.AspNetCore.Mvc.Gds.Components.Extensions;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Contract.Site.Commands.PlanningDetails;
using HE.Investment.AHP.Contract.Site.Queries;
using HE.Investment.AHP.WWW.Extensions;
using HE.Investment.AHP.WWW.Models.Site;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using HE.Investments.Common.WWW.Controllers;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Models;
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
        var response = await _mediator.Send(new GetSiteListQuery(new PaginationRequest(1, int.MaxValue)), cancellationToken);
        return View("Index", response);
    }

    [HttpGet("select")]
    public async Task<IActionResult> Select([FromQuery] int? page, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetSiteListQuery(new PaginationRequest(page ?? 1)), cancellationToken);
        return View("Select", response);
    }

    [HttpGet("{siteId}/confirm-select")]
    public async Task<IActionResult> ConfirmSelect(string siteId, CancellationToken cancellationToken)
    {
        var site = await _mediator.Send(new GetSiteQuery(siteId), cancellationToken);
        return View("ConfirmSelect", site);
    }

    [HttpPost("{siteId}/confirm-select")]
    public async Task<IActionResult> SelectConfirmed(string siteId, bool? isConfirmed, CancellationToken cancellationToken)
    {
        if (isConfirmed.IsNotProvided())
        {
            ModelState.AddValidationErrors(OperationResult.New().AddValidationError("IsConfirmed", "Select whether this is the correct site"));
            return View("ConfirmSelect", await _mediator.Send(new GetSiteQuery(siteId), cancellationToken));
        }

        return isConfirmed!.Value
            ? RedirectToAction("Name", "Application", new { siteId })
            : RedirectToAction("Select");
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

        return await Continue(new { siteId = result.ReturnedData?.Value });
    }

    [HttpGet("{siteId}/section-106-general-agreement")]
    [WorkflowState(SiteWorkflowState.Section106GeneralAgreement)]
    public async Task<IActionResult> Section106Agreement([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await _mediator.Send(new GetSiteQuery(siteId), cancellationToken);
        return View("Section106Agreement", siteModel);
    }

    [HttpPost("{siteId}/section-106-general-agreement")]
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
        var result = await _mediator.Send(
            new ProvideSection106AffordableHousingCommand(new SiteId(siteId), model.Section106AffordableHousing),
            cancellationToken);
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
        var result = await _mediator.Send(
            new ProvideSection106OnlyAffordableHousingCommand(new SiteId(siteId), model.Section106OnlyAffordableHousing),
            cancellationToken);
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
        var result = await _mediator.Send(
            new ProvideSection106AdditionalAffordableHousingCommand(new SiteId(siteId), model.Section106AdditionalAffordableHousing),
            cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("Section106AdditionalAffordableHousing", model);
        }

        return await Continue(new { siteId });
    }

    [HttpGet("{siteId}/section-106-capital-funding-eligibility")]
    [WorkflowState(SiteWorkflowState.Section106CapitalFundingEligibility)]
    public async Task<IActionResult> Section106CapitalFundingEligibility([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await _mediator.Send(new GetSiteQuery(siteId), cancellationToken);
        return View("Section106CapitalFundingEligibility", siteModel);
    }

    [HttpPost("{siteId}/section-106-capital-funding-eligibility")]
    [WorkflowState(SiteWorkflowState.Section106CapitalFundingEligibility)]
    public async Task<IActionResult> Section106CapitalFundingEligibility([FromRoute] string siteId, SiteModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ProvideSection106CapitalFundingEligibilityCommand(new SiteId(siteId), model.Section106CapitalFundingEligibility),
            cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("Section106CapitalFundingEligibility", model);
        }

        return await Continue(new { siteId });
    }

    [HttpGet("{siteId}/section-106-local-authority-confirmation")]
    [WorkflowState(SiteWorkflowState.Section106LocalAuthorityConfirmation)]
    public async Task<IActionResult> Section106LocalAuthorityConfirmation([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await _mediator.Send(new GetSiteQuery(siteId), cancellationToken);
        return View("Section106LocalAuthorityConfirmation", siteModel);
    }

    [HttpPost("{siteId}/section-106-local-authority-confirmation")]
    [WorkflowState(SiteWorkflowState.Section106LocalAuthorityConfirmation)]
    public async Task<IActionResult> Section106LocalAuthorityConfirmation([FromRoute] string siteId, SiteModel model, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ProvideSection106LocalAuthorityConfirmationCommand(new SiteId(siteId), model.Section106LocalAuthorityConfirmation), cancellationToken);
        if (result.HasValidationErrors)
        {
            ModelState.AddValidationErrors(result);
            return View("Section106LocalAuthorityConfirmation", model);
        }

        return await Continue(new { siteId });
    }

    [HttpGet("{siteId}/local-authority/search")]
    [WorkflowState(SiteWorkflowState.LocalAuthoritySearch)]
    public IActionResult LocalAuthoritySearch(string siteId)
    {
        return View(new LocalAuthorities { SiteId = siteId });
    }

    [HttpPost("{siteId}/local-authority/search")]
    [WorkflowState(SiteWorkflowState.LocalAuthoritySearch)]
    public async Task<IActionResult> LocalAuthoritySearch(string siteId, LocalAuthorities model, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<LocalAuthorities>(
            _mediator,
            new ProvideLocalAuthoritySearchPhraseCommand(new SiteId(siteId), model.Phrase),
            () => ContinueWithRedirect(new { siteId, phrase = model.Phrase }),
            async () => await Task.FromResult<IActionResult>(View("LocalAuthoritySearch", model)),
            cancellationToken);
    }

    [HttpGet("{siteId}/local-authority/search/result")]
    [WorkflowState(SiteWorkflowState.LocalAuthorityResult)]
    public async Task<IActionResult> LocalAuthorityResult(
        string siteId,
        string phrase,
        [FromQuery] string redirect,
        CancellationToken token,
        [FromQuery] int page = 0)
    {
        var result = await _mediator.Send(new SearchLocalAuthoritiesQuery(phrase, new PaginationRequest(page - 1)), token);

        if (result.ReturnedData.Page?.TotalItems == 0)
        {
            return RedirectToAction(nameof(LocalAuthorityNotFound), new { siteId, redirect });
        }

        var model = result.ReturnedData;

        model.SiteId = siteId;
        model.Page = new PaginationResult<LocalAuthority>(model.Page!.Items, page, model.Page.ItemsPerPage, model.Page.TotalItems);

        return View(model);
    }

    [HttpGet("{siteId}/local-authority/not-found")]
    public IActionResult LocalAuthorityNotFound(string siteId)
    {
        return View(new LocalAuthorities { SiteId = siteId });
    }

    [HttpGet("{siteId}/local-authority/{localAuthorityId}/{localAuthorityName}/confirm")]
    [WorkflowState(SiteWorkflowState.LocalAuthorityConfirm)]
    public IActionResult LocalAuthorityConfirm(string siteId, string localAuthorityId, string localAuthorityName, string phrase)
    {
        var model = new LocalAuthorities
        {
            SiteId = siteId,
            LocalAuthorityId = localAuthorityId,
            LocalAuthorityName = localAuthorityName,
            Phrase = phrase,
        };

        return View(new ConfirmModel<LocalAuthorities>(model));
    }

    [HttpPost("{siteId}/local-authority/{localAuthorityId}/{localAuthorityName}/confirm")]
    [WorkflowState(SiteWorkflowState.LocalAuthorityConfirm)]
    public async Task<IActionResult> LocalAuthorityConfirm(
        string siteId,
        string localAuthorityId,
        string localAuthorityName,
        ConfirmModel<LocalAuthorities> model,
        [FromQuery] string redirect,
        CancellationToken cancellationToken)
    {
        if (model.Response == YesNoType.No.ToString())
        {
            return RedirectToAction(nameof(LocalAuthoritySearch), new { siteId, redirect });
        }

        return await this.ExecuteCommand<ConfirmModel<LocalAuthorities>>(
            _mediator,
            new ProvideLocalAuthorityCommand(new SiteId(siteId), localAuthorityId, localAuthorityName, model.Response),
            () => ContinueWithRedirect(new { siteId, redirect }),
            async () => await Task.FromResult<IActionResult>(View(model)),
            cancellationToken);
    }

    [HttpGet("{siteId}/local-authority/reset")]
    [WorkflowState(SiteWorkflowState.LocalAuthorityReset)]
    public async Task<IActionResult> LocalAuthorityReset(string siteId, [FromQuery] string redirect, CancellationToken token)
    {
        await _mediator.Send(
            new ProvideLocalAuthorityCommand(new SiteId(siteId), null, null, null),
            token);

        return await Continue(redirect, new { siteId });
    }

    [HttpGet("{siteId}/back")]
    public async Task<IActionResult> Back([FromRoute] string siteId, SiteWorkflowState currentPage)
    {
        return await Back(currentPage, new { siteId });
    }

    [HttpGet("{siteId}/planning-status")]
    [WorkflowState(SiteWorkflowState.PlanningStatus)]
    public async Task<IActionResult> PlanningStatus([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await _mediator.Send(new GetSiteQuery(siteId), cancellationToken);
        ViewBag.SiteName = siteModel.Name!;
        return View($"PlanningDetails/{nameof(PlanningStatus)}", siteModel.PlanningDetails);
    }

    [HttpPost("{siteId}/planning-status")]
    [WorkflowState(SiteWorkflowState.PlanningStatus)]
    public async Task<IActionResult> PlanningStatus(SitePlanningDetails model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand<SitePlanningDetails>(
            new ProvideSitePlanningStatusCommand(this.GetSiteIdFromRoute(), model.PlanningStatus),
            $"PlanningDetails/{nameof(PlanningStatus)}",
            savedModel =>
            {
                ViewBag.SiteName = savedModel.Name!;
                return model;
            },
            cancellationToken);
    }

    [HttpGet("{siteId}/national-design-guide")]
    [WorkflowState(SiteWorkflowState.NationalDesignGuide)]
    public async Task<IActionResult> NationalDesignGuide([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await _mediator.Send(new GetSiteQuery(siteId), cancellationToken);
        ViewBag.SiteName = siteModel.Name!;
        var designGuideModel = new NationalDesignGuidePrioritiesModel()
        {
            SiteName = siteModel.Name,
            DesignPriorities = siteModel.NationalDesignGuidePriorities,
        };
        return View("NationalDesignGuide", designGuideModel);
    }

    [HttpPost("{siteId}/national-design-guide")]
    [WorkflowState(SiteWorkflowState.NationalDesignGuide)]
    public async Task<IActionResult> NationalDesignGuide(NationalDesignGuidePrioritiesModel model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand(
            new ProvideNationaDesignGuidePrioritiesCommand(this.GetSiteIdFromRoute(), model.DesignPriorities.Concat(model.OtherPriorities).ToList()),
            $"NationalDesignGuide",
            savedModel =>
            {
                ViewBag.SiteName = savedModel.Name!;
                return model;
            },
            cancellationToken);
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

    private async Task<IActionResult> ExecuteSiteCommand<TViewModel>(
        IRequest<OperationResult> command,
        string viewName,
        Func<SiteModel, TViewModel> createViewModelForError,
        CancellationToken cancellationToken)
    {
        var siteId = this.GetSiteIdFromRoute();

        return await this.ExecuteCommand<TViewModel>(
            _mediator,
            command,
            async () => await ContinueWithRedirect(new { siteId }),
            async () =>
            {
                var siteDetails = await _mediator.Send(new GetSiteQuery(siteId.Value), cancellationToken);
                var model = createViewModelForError(siteDetails);

                return View(viewName, model);
            },
            cancellationToken);
    }
}
