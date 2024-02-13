using He.AspNetCore.Mvc.Gds.Components.Extensions;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Contract.Site.Commands.PlanningDetails;
using HE.Investment.AHP.Contract.Site.Commands.Section106;
using HE.Investment.AHP.Contract.Site.Commands.TenderingStatus;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.Contract.Site.Queries;
using HE.Investment.AHP.WWW.Extensions;
using HE.Investment.AHP.WWW.Models.Site;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.Contract;
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
            ModelState.Clear();
            ModelState.AddValidationErrors(result);

            var orderedProperties = new List<string> { nameof(SiteModel.Name) };
            ViewBag.validationErrors = ViewData.ModelState.GetOrderedErrors(orderedProperties.ToList());
            return View(nameof(Name), model);
        }

        return await Continue(new { siteId = result.ReturnedData.Value });
    }

    [HttpGet("{siteId}/section-106-general-agreement")]
    [WorkflowState(SiteWorkflowState.Section106GeneralAgreement)]
    public async Task<IActionResult> Section106GeneralAgreement([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await _mediator.Send(new GetSiteQuery(siteId), cancellationToken);
        return View("Section106GeneralAgreement", siteModel.Section106 ?? new Section106Dto(siteId, siteModel.Name ?? string.Empty, null));
    }

    [HttpPost("{siteId}/section-106-general-agreement")]
    [WorkflowState(SiteWorkflowState.Section106GeneralAgreement)]
    public async Task<IActionResult> Section106GeneralAgreement([FromRoute] string siteId, Section106Dto model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand(
            new ProvideSection106AgreementCommand(new SiteId(siteId), model.GeneralAgreement),
            nameof(Section106GeneralAgreement),
            _ => model,
            cancellationToken);
    }

    [HttpGet("{siteId}/section-106-affordable-housing")]
    [WorkflowState(SiteWorkflowState.Section106AffordableHousing)]
    public async Task<IActionResult> Section106AffordableHousing([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await _mediator.Send(new GetSiteQuery(siteId), cancellationToken);
        return View("Section106AffordableHousing", siteModel.Section106);
    }

    [HttpPost("{siteId}/section-106-affordable-housing")]
    [WorkflowState(SiteWorkflowState.Section106AffordableHousing)]
    public async Task<IActionResult> Section106AffordableHousing([FromRoute] string siteId, Section106Dto model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand(
            new ProvideSection106AffordableHousingCommand(new SiteId(siteId), model.AffordableHousing),
            nameof(Section106AffordableHousing),
            _ => model,
            cancellationToken);
    }

    [HttpGet("{siteId}/section-106-only-affordable-housing")]
    [WorkflowState(SiteWorkflowState.Section106OnlyAffordableHousing)]
    public async Task<IActionResult> Section106OnlyAffordableHousing([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await _mediator.Send(new GetSiteQuery(siteId), cancellationToken);
        return View("Section106OnlyAffordableHousing", siteModel.Section106);
    }

    [HttpPost("{siteId}/section-106-only-affordable-housing")]
    [WorkflowState(SiteWorkflowState.Section106OnlyAffordableHousing)]
    public async Task<IActionResult> Section106OnlyAffordableHousing([FromRoute] string siteId, Section106Dto model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand(
            new ProvideSection106OnlyAffordableHousingCommand(new SiteId(siteId), model.OnlyAffordableHousing),
            nameof(Section106OnlyAffordableHousing),
            _ => model,
            cancellationToken);
    }

    [HttpGet("{siteId}/section-106-additional-affordable-housing")]
    [WorkflowState(SiteWorkflowState.Section106AdditionalAffordableHousing)]
    public async Task<IActionResult> Section106AdditionalAffordableHousing([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await _mediator.Send(new GetSiteQuery(siteId), cancellationToken);
        return View("Section106AdditionalAffordableHousing", siteModel.Section106);
    }

    [HttpPost("{siteId}/section-106-additional-affordable-housing")]
    [WorkflowState(SiteWorkflowState.Section106AdditionalAffordableHousing)]
    public async Task<IActionResult> Section106AdditionalAffordableHousing([FromRoute] string siteId, Section106Dto model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand(
            new ProvideSection106AdditionalAffordableHousingCommand(new SiteId(siteId), model.AdditionalAffordableHousing),
            nameof(Section106AdditionalAffordableHousing),
            _ => model,
            cancellationToken);
    }

    [HttpGet("{siteId}/section-106-capital-funding-eligibility")]
    [WorkflowState(SiteWorkflowState.Section106CapitalFundingEligibility)]
    public async Task<IActionResult> Section106CapitalFundingEligibility([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await _mediator.Send(new GetSiteQuery(siteId), cancellationToken);
        return View("Section106CapitalFundingEligibility", siteModel.Section106);
    }

    [HttpPost("{siteId}/section-106-capital-funding-eligibility")]
    [WorkflowState(SiteWorkflowState.Section106CapitalFundingEligibility)]
    public async Task<IActionResult> Section106CapitalFundingEligibility([FromRoute] string siteId, Section106Dto model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand(
            new ProvideSection106CapitalFundingEligibilityCommand(new SiteId(siteId), model.CapitalFundingEligibility),
            nameof(Section106CapitalFundingEligibility),
            _ => model,
            cancellationToken);
    }

    [HttpGet("{siteId}/section-106-local-authority-confirmation")]
    [WorkflowState(SiteWorkflowState.Section106LocalAuthorityConfirmation)]
    public async Task<IActionResult> Section106LocalAuthorityConfirmation([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await _mediator.Send(new GetSiteQuery(siteId), cancellationToken);
        return View("Section106LocalAuthorityConfirmation", siteModel.Section106);
    }

    [HttpPost("{siteId}/section-106-local-authority-confirmation")]
    [WorkflowState(SiteWorkflowState.Section106LocalAuthorityConfirmation)]
    public async Task<IActionResult> Section106LocalAuthorityConfirmation([FromRoute] string siteId, Section106Dto model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand(
            new ProvideSection106LocalAuthorityConfirmationCommand(new SiteId(siteId), model.LocalAuthorityConfirmation),
            nameof(Section106LocalAuthorityConfirmation),
            _ => model,
            cancellationToken);
    }

    [HttpGet("{siteId}/section-106-ineligible")]
    [WorkflowState(SiteWorkflowState.Section106Ineligible)]
    public async Task<IActionResult> Section106Ineligible([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await _mediator.Send(new GetSiteQuery(siteId), cancellationToken);
        return View("Section106Ineligible", siteModel.Section106);
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
        [FromQuery] int? page,
        CancellationToken token)
    {
        var result = await _mediator.Send(new SearchLocalAuthoritiesQuery(phrase, new PaginationRequest(page ?? 1)), token);

        if (result.ReturnedData.Page?.TotalItems == 0)
        {
            return RedirectToAction(nameof(LocalAuthorityNotFound), new { siteId, redirect });
        }

        var model = result.ReturnedData;

        model.SiteId = siteId;
        model.Page = new PaginationResult<LocalAuthority>(model.Page!.Items, page ?? 1, model.Page.ItemsPerPage, model.Page.TotalItems);

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

        if (TempData["LocalAuthoritiesErrors"] is IDictionary<string, string> validationErrors)
        {
            ModelState.Merge(validationErrors);
        }

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
            async () =>
            {
                TempData["LocalAuthoritiesErrors"] = ViewBag.validationErrors;

                return await Task.FromResult(RedirectToAction(
                    "LocalAuthorityConfirm",
                    new
                    {
                        siteId,
                        localAuthorityId,
                        localAuthorityName,
                        redirect,
                        phrase = model.ViewModel.Phrase,
                    }));
            },
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
        return View("PlanningStatus", siteModel.PlanningDetails);
    }

    [HttpPost("{siteId}/planning-status")]
    [WorkflowState(SiteWorkflowState.PlanningStatus)]
    public async Task<IActionResult> PlanningStatus(SitePlanningDetails model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand<SitePlanningDetails>(
            new ProvideSitePlanningStatusCommand(this.GetSiteIdFromRoute(), model.PlanningStatus),
            nameof(PlanningStatus),
            savedModel =>
            {
                ViewBag.SiteName = savedModel.Name!;
                return model;
            },
            cancellationToken);
    }

    [HttpGet("{siteId}/planning-details")]
    [WorkflowState(SiteWorkflowState.PlanningDetails)]
    public async Task<IActionResult> PlanningDetails([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await GetSiteDetails(siteId, cancellationToken);
        return View("PlanningDetails", siteModel.PlanningDetails);
    }

    [HttpPost("{siteId}/planning-details")]
    [WorkflowState(SiteWorkflowState.PlanningDetails)]
    public async Task<IActionResult> PlanningDetails(SitePlanningDetails model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand<SitePlanningDetails>(
            new ProvidePlanningDetailsCommand(
                this.GetSiteIdFromRoute(),
                model.ReferenceNumber,
                model.DetailedPlanningApprovalDate ?? DateDetails.Empty(),
                model.RequiredFurtherSteps,
                model.ApplicationForDetailedPlanningSubmittedDate ?? DateDetails.Empty(),
                model.ExpectedPlanningApprovalDate ?? DateDetails.Empty(),
                model.OutlinePlanningApprovalDate ?? DateDetails.Empty(),
                model.IsGrantFundingForAllHomesCoveredByApplication,
                model.PlanningSubmissionDate ?? DateDetails.Empty(),
                model.IsLandRegistryTitleNumberRegistered),
            nameof(PlanningDetails),
            savedModel => model with { PlanningStatus = savedModel.PlanningDetails.PlanningStatus },
            cancellationToken);
    }

    [HttpGet("{siteId}/land-registry")]
    [WorkflowState(SiteWorkflowState.LandRegistry)]
    public async Task<IActionResult> LandRegistry([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await GetSiteDetails(siteId, cancellationToken);
        return View("LandRegistry", siteModel.PlanningDetails);
    }

    [HttpPost("{siteId}/land-registry")]
    [WorkflowState(SiteWorkflowState.LandRegistry)]
    public async Task<IActionResult> LandRegistry(SitePlanningDetails model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand<SitePlanningDetails>(
            new ProvideLandRegistryDetailsCommand(
                this.GetSiteIdFromRoute(),
                model.LandRegistryTitleNumber,
                model.IsGrantFundingForAllHomesCoveredByTitleNumber),
            nameof(LandRegistry),
            _ => model,
            cancellationToken);
    }

    [HttpGet("{siteId}/national-design-guide")]
    [WorkflowState(SiteWorkflowState.NationalDesignGuide)]
    public async Task<IActionResult> NationalDesignGuide([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await _mediator.Send(new GetSiteQuery(siteId), cancellationToken);

        var designGuideModel = new NationalDesignGuidePrioritiesModel
        {
            SiteId = siteId,
            SiteName = siteModel.Name ?? string.Empty,
            DesignPriorities = siteModel.NationalDesignGuidePriorities.ToList(),
        };
        return View("NationalDesignGuide", designGuideModel);
    }

    [HttpPost("{siteId}/national-design-guide")]
    [WorkflowState(SiteWorkflowState.NationalDesignGuide)]
    public async Task<IActionResult> NationalDesignGuide(
        [FromRoute] string siteId,
        NationalDesignGuidePrioritiesModel model,
        CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand(
            new ProvideNationalDesignGuidePrioritiesCommand(
                this.GetSiteIdFromRoute(),
                (IReadOnlyCollection<NationalDesignGuidePriority>)(model.DesignPriorities ?? new List<NationalDesignGuidePriority>())),
            nameof(NationalDesignGuide),
            _ => model,
            cancellationToken);
    }

    [HttpGet("{siteId}/building-for-a-healthy-life")]
    [WorkflowState(SiteWorkflowState.BuildingForHealthyLife)]
    public async Task<IActionResult> BuildingForHealthyLife([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await GetSiteDetails(siteId, cancellationToken);
        return View("BuildingForHealthyLife", siteModel);
    }

    [HttpPost("{siteId}/building-for-a-healthy-life")]
    [WorkflowState(SiteWorkflowState.BuildingForHealthyLife)]
    public async Task<IActionResult> BuildingForHealthyLife(SiteModel model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand<SiteModel>(
            new ProvideBuildingForHealthyLifeCommand(
                this.GetSiteIdFromRoute(),
                model.BuildingForHealthyLife),
            nameof(BuildingForHealthyLife),
            _ => model,
            cancellationToken);
    }

    [HttpGet("{siteId}/number-of-green-lights")]
    [WorkflowState(SiteWorkflowState.NumberOfGreenLights)]
    public async Task<IActionResult> NumberOfGreenLights([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await GetSiteDetails(siteId, cancellationToken);
        return View("NumberOfGreenLights", siteModel);
    }

    [HttpPost("{siteId}/number-of-green-lights")]
    [WorkflowState(SiteWorkflowState.NumberOfGreenLights)]
    public async Task<IActionResult> NumberOfGreenLights(SiteModel model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand<SiteModel>(
            new ProvideNumberOfGreenLightsCommand(
                this.GetSiteIdFromRoute(),
                model.NumberOfGreenLights),
            nameof(NumberOfGreenLights),
            _ => model,
            cancellationToken);
    }

    [HttpGet("{siteId}/land-acquisition-status")]
    [WorkflowState(SiteWorkflowState.LandAcquisitionStatus)]
    public async Task<IActionResult> LandAcquisitionStatus([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await GetSiteDetails(siteId, cancellationToken);
        ViewBag.SiteName = siteModel.Name;
        return View("LandAcquisitionStatus", siteModel);
    }

    [HttpPost("{siteId}/land-acquisition-status")]
    [WorkflowState(SiteWorkflowState.LandAcquisitionStatus)]
    public async Task<IActionResult> LandAcquisitionStatus(SiteModel model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand<SiteModel>(
            new ProvideLandAcquisitionStatusCommand(
                this.GetSiteIdFromRoute(),
                model.LandAcquisitionStatus),
            nameof(LandAcquisitionStatus),
            savedModel => model,
            cancellationToken);
    }

    [HttpGet("{siteId}/tendering-status")]
    [WorkflowState(SiteWorkflowState.TenderingStatus)]
    public async Task<IActionResult> TenderingStatus([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await GetSiteDetails(siteId, cancellationToken);
        return View("TenderingStatus", siteModel.TenderingStatusDetails);
    }

    [HttpPost("{siteId}/tendering-status")]
    [WorkflowState(SiteWorkflowState.TenderingStatus)]
    public async Task<IActionResult> TenderingStatus(SiteTenderingStatusDetails model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand<SiteTenderingStatusDetails>(
            new ProvideTenderingStatusCommand(
                this.GetSiteIdFromRoute(),
                model.TenderingStatus),
            nameof(TenderingStatus),
            _ => model,
            cancellationToken);
    }

    [HttpGet("{siteId}/contractor-details")]
    [WorkflowState(SiteWorkflowState.ContractorDetails)]
    public async Task<IActionResult> ContractorDetails([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await GetSiteDetails(siteId, cancellationToken);
        return View("ContractorDetails", siteModel.TenderingStatusDetails);
    }

    [HttpPost("{siteId}/contractor-details")]
    [WorkflowState(SiteWorkflowState.ContractorDetails)]
    public async Task<IActionResult> ContractorDetails(SiteTenderingStatusDetails model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand<SiteTenderingStatusDetails>(
            new ProvideContractorDetailsCommand(
                this.GetSiteIdFromRoute(),
                model.ContractorName,
                model.IsSmeContractor),
            nameof(ContractorDetails),
            savedModel => model with { TenderingStatus = savedModel.TenderingStatusDetails.TenderingStatus },
            cancellationToken);
    }

    [HttpGet("{siteId}/intention-to-work-with-sme")]
    [WorkflowState(SiteWorkflowState.IntentionToWorkWithSme)]
    public async Task<IActionResult> IntentionToWorkWithSme([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await GetSiteDetails(siteId, cancellationToken);
        return View("IntentionToWorkWithSme", siteModel.TenderingStatusDetails);
    }

    [HttpPost("{siteId}/intention-to-work-with-sme")]
    [WorkflowState(SiteWorkflowState.IntentionToWorkWithSme)]
    public async Task<IActionResult> IntentionToWorkWithSme(SiteTenderingStatusDetails model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand<SiteTenderingStatusDetails>(
            new ProvideIsIntentionToWorkWithSmeCommand(
                this.GetSiteIdFromRoute(),
                model.IsIntentionToWorkWithSme),
            nameof(ContractorDetails),
            savedModel => model with { TenderingStatus = savedModel.TenderingStatusDetails.TenderingStatus },
            cancellationToken);
    }

    [HttpGet("{siteId}/strategic-site")]
    [WorkflowState(SiteWorkflowState.StrategicSite)]
    public async Task<IActionResult> StrategicSite([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await GetSiteDetails(siteId, cancellationToken);
        return View("StrategicSite", siteModel.StrategicSiteDetails);
    }

    [HttpPost("{siteId}/strategic-site")]
    [WorkflowState(SiteWorkflowState.StrategicSite)]
    public async Task<IActionResult> StrategicSite(StrategicSite model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand<StrategicSite>(
            new ProvideStrategicSiteDetailsCommand(
                this.GetSiteIdFromRoute(),
                model.IsStrategicSite,
                model.StrategicSiteName),
            nameof(StrategicSite),
            _ => model,
            cancellationToken);
    }

    [HttpGet("{siteId}/site-type")]
    [WorkflowState(SiteWorkflowState.SiteType)]
    public async Task<IActionResult> SiteType([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await GetSiteDetails(siteId, cancellationToken);
        return View("SiteType", siteModel.SiteTypeDetails);
    }

    [HttpPost("{siteId}/site-type")]
    [WorkflowState(SiteWorkflowState.SiteType)]
    public async Task<IActionResult> SiteType(SiteTypeDetails model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand<SiteTypeDetails>(
            new ProvideSiteTypeDetailsCommand(
                this.GetSiteIdFromRoute(),
                model.SiteType,
                model.IsOnGreenBelt,
                model.IsRegenerationSite),
            nameof(SiteType),
            _ => model,
            cancellationToken);
    }

    [HttpGet("{siteId}/site-use")]
    [WorkflowState(SiteWorkflowState.SiteUse)]
    public async Task<IActionResult> SiteUse([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await GetSiteDetails(siteId, cancellationToken);
        return View("SiteUse", siteModel.SiteUseDetails);
    }

    [HttpPost("{siteId}/site-use")]
    [WorkflowState(SiteWorkflowState.SiteUse)]
    public async Task<IActionResult> SiteUse(SiteUseDetails model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand<SiteUseDetails>(
            new ProvideSiteUseDetailsCommand(
                this.GetSiteIdFromRoute(),
                model.IsPartOfStreetFrontInfill,
                model.IsForTravellerPitchSite),
            nameof(SiteUse),
            _ => model,
            cancellationToken);
    }

    [HttpGet("{siteId}/traveller-pitch-type")]
    [WorkflowState(SiteWorkflowState.TravellerPitchType)]
    public async Task<IActionResult> TravellerPitchType([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await GetSiteDetails(siteId, cancellationToken);
        return View("TravellerPitchType", siteModel.SiteUseDetails);
    }

    [HttpPost("{siteId}/traveller-pitch-type")]
    [WorkflowState(SiteWorkflowState.TravellerPitchType)]
    public async Task<IActionResult> TravellerPitchType(SiteUseDetails model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand<SiteUseDetails>(
            new ProvideTravellerPitchSiteTypeCommand(this.GetSiteIdFromRoute(), model.TravellerPitchSiteType),
            nameof(TravellerPitchType),
            _ => model,
            cancellationToken);
    }

    [HttpGet("{siteId}/check-answers")]
    [WorkflowState(SiteWorkflowState.CheckAnswers)]
    public IActionResult CheckAnswers()
    {
        return View();
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
            async () => await this.ReturnToSitesListOrContinue(async () => await ContinueWithRedirect(new { siteId })),
            async () =>
            {
                var siteDetails = await GetSiteDetails(siteId.Value, cancellationToken);
                var model = createViewModelForError(siteDetails);

                return View(viewName, model);
            },
            cancellationToken);
    }

    private async Task<SiteModel> GetSiteDetails(string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await _mediator.Send(new GetSiteQuery(siteId), cancellationToken);
        ViewBag.SiteName = siteModel.Name!;
        return siteModel;
    }
}
