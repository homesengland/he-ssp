using He.AspNetCore.Mvc.Gds.Components.Extensions;
using HE.Investment.AHP.Contract.Common.Enums;
using HE.Investment.AHP.Contract.Common.Queries;
using HE.Investment.AHP.Contract.PrefillData.Queries;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Contract.Site.Commands.Mmc;
using HE.Investment.AHP.Contract.Site.Commands.PlanningDetails;
using HE.Investment.AHP.Contract.Site.Commands.Section106;
using HE.Investment.AHP.Contract.Site.Commands.TenderingStatus;
using HE.Investment.AHP.Contract.Site.Enums;
using HE.Investment.AHP.Contract.Site.Queries;
using HE.Investment.AHP.WWW.Extensions;
using HE.Investment.AHP.WWW.Models.Site;
using HE.Investment.AHP.WWW.Models.Site.Factories;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Contract.Queries;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Constants;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Validators;
using HE.Investments.Common.WWW.Controllers;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Models;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.FrontDoor.Shared.Project;
using HE.Investments.Organisation.LocalAuthorities.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using LocalAuthority = HE.Investments.Common.Contract.LocalAuthority;

namespace HE.Investment.AHP.WWW.Controllers;

[Route("site")]
[AuthorizeWithCompletedProfile]
public class SiteController : WorkflowController<SiteWorkflowState>
{
    private readonly IMediator _mediator;

    private readonly IAccountAccessContext _accountAccessContext;

    private readonly ISiteSummaryViewModelFactory _siteSummaryViewModelFactory;

    public SiteController(IMediator mediator, IAccountAccessContext accountAccessContext, ISiteSummaryViewModelFactory siteSummaryViewModelFactory)
    {
        _mediator = mediator;
        _accountAccessContext = accountAccessContext;
        _siteSummaryViewModelFactory = siteSummaryViewModelFactory;
    }

    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] int? page, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetSiteListQuery(new PaginationRequest(page ?? 1)), cancellationToken);
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
    public async Task<IActionResult> Details(string siteId, [FromQuery] int? page, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetSiteDetailsQuery(SiteId.From(siteId), new PaginationRequest(page ?? 1)), cancellationToken);
        return View("Details", response);
    }

    [HttpGet("{siteId}/continue-answering")]
    public async Task<IActionResult> ContinueAnswering(string siteId, CancellationToken cancellationToken)
    {
        var summary = await CreateSiteSummary(cancellationToken, useWorkflowRedirection: false);
        return this.ContinueSectionAnswering(summary, () => RedirectToAction("CheckAnswers", new { siteId }));
    }

    [HttpGet("start")]
    [WorkflowState(SiteWorkflowState.Start)]
    public async Task<IActionResult> Start(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetSiteListQuery(new PaginationRequest(1, 1)), cancellationToken);
        var backUrl = response.Page.TotalItems > 0 ? Url.Action("Select") : Url.Action("Start", "Application");

        return View("Start", new StartSiteModel(backUrl));
    }

    [HttpGet("name")]
    [WorkflowState(SiteWorkflowState.Name)]
    public async Task<IActionResult> Name([FromQuery] string? siteId, [FromQuery] string? fdProjectId, [FromQuery] string? fdSiteId, CancellationToken cancellationToken)
    {
        return View("Name", await CreateSiteNameModel(siteId, fdProjectId, fdSiteId, cancellationToken));
    }

    [HttpPost("name")]
    [WorkflowState(SiteWorkflowState.Name)]
    public async Task<IActionResult> NamePost(
        [FromQuery] string? siteId,
        [FromQuery] string? fdProjectId,
        [FromQuery] string? fdSiteId,
        SiteModel model,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ProvideNameCommand(
                SiteId.Create(siteId ?? model.Id),
                FrontDoorProjectId.Create(fdProjectId),
                FrontDoorSiteId.Create(fdSiteId),
                model.Name),
            cancellationToken);

        if (result.HasValidationErrors)
        {
            ModelState.Clear();
            ModelState.AddValidationErrors(result);

            var orderedProperties = new List<string> { nameof(SiteModel.Name) };
            ViewBag.validationErrors = ViewData.ModelState.GetOrderedErrors(orderedProperties.ToList());
            return View(nameof(Name), model);
        }

        return await ContinueWithWorkflow(new { siteId = result.ReturnedData.Value });
    }

    [HttpGet("{siteId}/section-106-general-agreement")]
    [WorkflowState(SiteWorkflowState.Section106GeneralAgreement)]
    public async Task<IActionResult> Section106GeneralAgreement([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await _mediator.Send(new GetSiteQuery(siteId), cancellationToken);
        return View("Section106GeneralAgreement", siteModel.Section106 ?? new Section106Dto(siteId, siteModel.Name, null));
    }

    [HttpPost("{siteId}/section-106-general-agreement")]
    [WorkflowState(SiteWorkflowState.Section106GeneralAgreement)]
    public async Task<IActionResult> Section106GeneralAgreement([FromRoute] string siteId, Section106Dto model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand(
            new ProvideSection106AgreementCommand(SiteId.From(siteId), model.GeneralAgreement),
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
            new ProvideSection106AffordableHousingCommand(SiteId.From(siteId), model.AffordableHousing),
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
            new ProvideSection106OnlyAffordableHousingCommand(SiteId.From(siteId), model.OnlyAffordableHousing),
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
            new ProvideSection106AdditionalAffordableHousingCommand(SiteId.From(siteId), model.AdditionalAffordableHousing),
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
            new ProvideSection106CapitalFundingEligibilityCommand(SiteId.From(siteId), model.CapitalFundingEligibility),
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
            new ProvideSection106LocalAuthorityConfirmationCommand(SiteId.From(siteId), model.LocalAuthorityConfirmation),
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
    public async Task<IActionResult> LocalAuthoritySearch(string siteId, CancellationToken cancellationToken)
    {
        await GetSiteBasicDetails(siteId, cancellationToken);
        var prefillData = await _mediator.Send(new GetAhpSitePrefillDataQuery(SiteId.From(siteId)), cancellationToken);
        return View(nameof(LocalAuthoritySearch), new LocalAuthorities { SiteId = siteId, Phrase = prefillData.LocalAuthorityName });
    }

    [HttpPost("{siteId}/local-authority/search")]
    [WorkflowState(SiteWorkflowState.LocalAuthoritySearch)]
    public async Task<IActionResult> LocalAuthoritySearch(string siteId, [FromQuery] string? workflow, LocalAuthorities model, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<LocalAuthorities>(
            _mediator,
            new ProvideLocalAuthoritySearchPhraseCommand(SiteId.From(siteId), model.Phrase),
            async () => await this.ReturnToSitesListOrContinue(() => Task.FromResult<IActionResult>(RedirectToAction("LocalAuthorityResult", new { siteId, phrase = model.Phrase, workflow }))),
            () => Task.FromResult<IActionResult>(View("LocalAuthoritySearch", model)),
            cancellationToken);
    }

    [HttpGet("{siteId}/local-authority/search/result")]
    [WorkflowState(SiteWorkflowState.LocalAuthorityResult)]
    public async Task<IActionResult> LocalAuthorityResult(
        string siteId,
        string phrase,
        [FromQuery] string workflow,
        [FromQuery] int? page,
        CancellationToken cancellationToken)
    {
        await GetSiteBasicDetails(siteId, cancellationToken);
        var result = await _mediator.Send(new SearchLocalAuthoritiesQuery(phrase, new PaginationRequest(page ?? 0)), cancellationToken);

        if (result.ReturnedData.Page?.TotalItems == 0)
        {
            return RedirectToAction(nameof(LocalAuthorityNotFound), new { siteId, workflow });
        }

        var model = result.ReturnedData;

        model.SiteId = siteId;
        model.Page = new PaginationResult<LocalAuthority>(model.Page!.Items, page ?? 0, model.Page.ItemsPerPage, model.Page.TotalItems);

        return View(model);
    }

    [HttpGet("{siteId}/local-authority/not-found")]
    public async Task<IActionResult> LocalAuthorityNotFound(string siteId, CancellationToken cancellationToken)
    {
        await GetSiteBasicDetails(siteId, cancellationToken);
        return View(nameof(LocalAuthorityNotFound), new LocalAuthorities { SiteId = siteId });
    }

    [HttpGet("{siteId}/local-authority/{localAuthorityCode}/confirm")]
    [WorkflowState(SiteWorkflowState.LocalAuthorityConfirm)]
    public async Task<IActionResult> LocalAuthorityConfirm(string siteId, string localAuthorityCode, string? phrase, CancellationToken cancellationToken)
    {
        var siteBasicDetails = await GetSiteBasicDetails(siteId, cancellationToken);
        var localAuthority = await _mediator.Send(new GetLocalAuthorityQuery(LocalAuthorityCode.From(localAuthorityCode)), cancellationToken);

        var localAuthorities = new LocalAuthorities
        {
            SiteId = siteId,
            LocalAuthorityCode = localAuthorityCode,
            LocalAuthorityName = localAuthority.Name,
            Phrase = phrase,
        };

        var model = new ConfirmModel<LocalAuthorities>(localAuthorities)
        {
            Response = siteBasicDetails.LocalAuthorityName == localAuthority.Name ? CommonResponse.Yes : string.Empty,
        };

        return View(model);
    }

    [HttpPost("{siteId}/local-authority/{localAuthorityCode}/confirm")]
    [WorkflowState(SiteWorkflowState.LocalAuthorityConfirm)]
    public async Task<IActionResult> LocalAuthorityConfirm(
        string siteId,
        ConfirmModel<LocalAuthorities> model,
        [FromQuery] string workflow,
        CancellationToken cancellationToken)
    {
        if (model.Response == YesNoType.No.ToString() && !Request.IsSaveAndReturnAction())
        {
            return RedirectToAction(nameof(LocalAuthoritySearch), new { siteId, workflow });
        }

        return await ExecuteSiteCommand<ConfirmModel<LocalAuthorities>>(
            new ProvideLocalAuthorityCommand(SiteId.From(siteId), model.ViewModel.LocalAuthorityCode, model.ViewModel.LocalAuthorityName, model.Response),
            nameof(LocalAuthorityConfirm),
            _ => model,
            cancellationToken);
    }

    [HttpGet("{siteId}/local-authority/reset")]
    [WorkflowState(SiteWorkflowState.LocalAuthorityReset)]
    public async Task<IActionResult> LocalAuthorityReset(string siteId, [FromQuery] string workflow, CancellationToken token)
    {
        await _mediator.Send(
            new ProvideLocalAuthorityCommand(SiteId.From(siteId), null, null, null),
            token);

        return await ContinueWithWorkflow(new { siteId, workflow });
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
        ViewBag.SiteName = siteModel.Name;
        return View("PlanningStatus", siteModel.PlanningDetails);
    }

    [HttpPost("{siteId}/planning-status")]
    [WorkflowState(SiteWorkflowState.PlanningStatus)]
    public async Task<IActionResult> PlanningStatus(SitePlanningDetails model, [FromQuery] string? workflow, CancellationToken cancellationToken)
    {
        var siteId = this.GetSiteIdFromRoute();

        return await this.ExecuteCommand<SitePlanningDetails>(
            _mediator,
            new ProvideSitePlanningStatusCommand(this.GetSiteIdFromRoute(), model.PlanningStatus),
            async () => await this.ReturnToSitesListOrContinue(() => Task.FromResult<IActionResult>(RedirectToAction("PlanningDetails", new { siteId, workflow }))),
            async () =>
            {
                var siteDetails = await GetSiteDetails(siteId.Value, cancellationToken);
                ViewBag.SiteName = siteDetails.Name;

                return View(model);
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
        return await ExecuteSiteCommand(
            new ProvidePlanningDetailsCommand(
                this.GetSiteIdFromRoute(),
                model.ReferenceNumber,
                model.IsDetailedPlanningApprovalDateActive,
                model.DetailedPlanningApprovalDate ?? DateDetails.Empty(),
                model.RequiredFurtherSteps,
                model.IsApplicationForDetailedPlanningSubmittedDateActive,
                model.ApplicationForDetailedPlanningSubmittedDate ?? DateDetails.Empty(),
                model.IsExpectedPlanningApprovalDateActive,
                model.ExpectedPlanningApprovalDate ?? DateDetails.Empty(),
                model.IsOutlinePlanningApprovalDateActive,
                model.OutlinePlanningApprovalDate ?? DateDetails.Empty(),
                model.IsGrantFundingForAllHomesCoveredByApplication,
                model.IsPlanningSubmissionDateActive,
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
            SiteName = siteModel.Name,
            DesignPriorities = siteModel.NationalDesignGuidePriorities.ToList(),
        };
        return View("NationalDesignGuide", designGuideModel);
    }

    [HttpPost("{siteId}/national-design-guide")]
    [WorkflowState(SiteWorkflowState.NationalDesignGuide)]
    public async Task<IActionResult> NationalDesignGuide(
        NationalDesignGuidePrioritiesModel model,
        CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand(
            new ProvideNationalDesignGuidePrioritiesCommand(
                this.GetSiteIdFromRoute(),
                (IReadOnlyCollection<NationalDesignGuidePriority>)(model.DesignPriorities ?? [])),
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

    [HttpGet("{siteId}/developing-partner")]
    [WorkflowState(SiteWorkflowState.DevelopingPartner)]
    public async Task<IActionResult> DevelopingPartner([FromRoute] string siteId, [FromQuery] int? page, CancellationToken cancellationToken)
    {
        return View(await GetSelectPartnerModel(siteId, page, cancellationToken));
    }

    [HttpGet("{siteId}/developing-partner-confirm/{organisationId}")]
    [WorkflowState(SiteWorkflowState.DevelopingPartnerConfirm)]
    public async Task<IActionResult> DevelopingPartnerConfirm([FromRoute] string siteId, [FromRoute] string organisationId, CancellationToken cancellationToken)
    {
        return View(await GetConfirmPartnerModel(siteId, organisationId, x => x.DevelopingPartner?.OrganisationId, cancellationToken));
    }

    [HttpPost("{siteId}/developing-partner-confirm/{organisationId}")]
    [WorkflowState(SiteWorkflowState.DevelopingPartnerConfirm)]
    public async Task<IActionResult> DevelopingPartnerConfirm([FromRoute] string siteId, [FromRoute] string organisationId, [FromForm] bool? isConfirmed, [FromQuery] string? workflow, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<(OrganisationDetails Organisation, bool? IsConfirmed)>(
            _mediator,
            new ProvideDevelopingPartnerCommand(SiteId.From(siteId), OrganisationId.From(organisationId), isConfirmed),
            async () => await this.ReturnToSitesListOrContinue(async () =>
                isConfirmed == true ? await ContinueWithWorkflow(new { siteId }) : RedirectToAction("DevelopingPartner", new { siteId, workflow })),
            async () =>
            {
                var (organisation, _) = await GetConfirmPartnerModel(siteId, organisationId, x => x.DevelopingPartner?.OrganisationId, cancellationToken);
                return View((organisation, isConfirmed));
            },
            cancellationToken);
    }

    [HttpGet("{siteId}/owner-of-the-land")]
    [WorkflowState(SiteWorkflowState.OwnerOfTheLand)]
    public async Task<IActionResult> OwnerOfTheLand([FromRoute] string siteId, [FromQuery] int? page, CancellationToken cancellationToken)
    {
        return View(await GetSelectPartnerModel(siteId, page, cancellationToken));
    }

    [HttpGet("{siteId}/owner-of-the-land-confirm/{organisationId}")]
    [WorkflowState(SiteWorkflowState.OwnerOfTheLandConfirm)]
    public async Task<IActionResult> OwnerOfTheLandConfirm([FromRoute] string siteId, [FromRoute] string organisationId, CancellationToken cancellationToken)
    {
        return View(await GetConfirmPartnerModel(siteId, organisationId, x => x.OwnerOfTheLand?.OrganisationId, cancellationToken));
    }

    [HttpPost("{siteId}/owner-of-the-land-confirm/{organisationId}")]
    [WorkflowState(SiteWorkflowState.OwnerOfTheLandConfirm)]
    public async Task<IActionResult> OwnerOfTheLandConfirm([FromRoute] string siteId, [FromRoute] string organisationId, [FromForm] bool? isConfirmed, [FromQuery] string? workflow, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<(OrganisationDetails Organisation, bool? IsConfirmed)>(
            _mediator,
            new ProvideOwnerOfTheLandCommand(SiteId.From(siteId), OrganisationId.From(organisationId), isConfirmed),
            async () => await this.ReturnToSitesListOrContinue(async () =>
                isConfirmed == true ? await ContinueWithWorkflow(new { siteId }) : RedirectToAction("OwnerOfTheLand", new { siteId, workflow })),
            async () =>
            {
                var (organisation, _) = await GetConfirmPartnerModel(siteId, organisationId, x => x.OwnerOfTheLand?.OrganisationId, cancellationToken);
                return View((organisation, isConfirmed));
            },
            cancellationToken);
    }

    [HttpGet("{siteId}/owner-of-the-homes")]
    [WorkflowState(SiteWorkflowState.OwnerOfTheHomes)]
    public async Task<IActionResult> OwnerOfTheHomes([FromRoute] string siteId, [FromQuery] int? page, CancellationToken cancellationToken)
    {
        return View(await GetSelectPartnerModel(siteId, page, cancellationToken));
    }

    [HttpGet("{siteId}/owner-of-the-homes-confirm/{organisationId}")]
    [WorkflowState(SiteWorkflowState.OwnerOfTheHomesConfirm)]
    public async Task<IActionResult> OwnerOfTheHomesConfirm([FromRoute] string siteId, [FromRoute] string organisationId, CancellationToken cancellationToken)
    {
        return View(await GetConfirmPartnerModel(siteId, organisationId, x => x.OwnerOfTheHomes?.OrganisationId, cancellationToken));
    }

    [HttpPost("{siteId}/owner-of-the-homes-confirm/{organisationId}")]
    [WorkflowState(SiteWorkflowState.OwnerOfTheHomesConfirm)]
    public async Task<IActionResult> OwnerOfTheHomesConfirm([FromRoute] string siteId, [FromRoute] string organisationId, [FromForm] bool? isConfirmed, [FromQuery] string? workflow, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<(OrganisationDetails Organisation, bool? IsConfirmed)>(
            _mediator,
            new ProvideOwnerOfTheHomesCommand(SiteId.From(siteId), OrganisationId.From(organisationId), isConfirmed),
            async () => await this.ReturnToSitesListOrContinue(async () =>
                isConfirmed == true ? await ContinueWithWorkflow(new { siteId }) : RedirectToAction("OwnerOfTheHomes", new { siteId, workflow })),
            async () =>
            {
                var (organisation, _) = await GetConfirmPartnerModel(siteId, organisationId, x => x.OwnerOfTheHomes?.OrganisationId, cancellationToken);
                return View((organisation, isConfirmed));
            },
            cancellationToken);
    }

    [HttpGet("{siteId}/land-acquisition-status")]
    [WorkflowState(SiteWorkflowState.LandAcquisitionStatus)]
    public async Task<IActionResult> LandAcquisitionStatus([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await GetSiteDetails(siteId, cancellationToken);
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
            _ => model,
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

    [HttpGet("{siteId}/rural-classification")]
    [WorkflowState(SiteWorkflowState.RuralClassification)]
    public async Task<IActionResult> RuralClassification([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await GetSiteDetails(siteId, cancellationToken);
        return View("RuralClassification", siteModel.RuralClassification);
    }

    [HttpPost("{siteId}/rural-classification")]
    [WorkflowState(SiteWorkflowState.RuralClassification)]
    public async Task<IActionResult> RuralClassification(SiteRuralClassification model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand<SiteRuralClassification>(
            new ProvideSiteRuralClassificationCommand(this.GetSiteIdFromRoute(), model.IsWithinRuralSettlement, model.IsRuralExceptionSite),
            nameof(RuralClassification),
            _ => model,
            cancellationToken);
    }

    [HttpGet("{siteId}/mmc-using")]
    [WorkflowState(SiteWorkflowState.MmcUsing)]
    public async Task<IActionResult> MmcUsing([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await GetSiteDetails(siteId, cancellationToken);
        return View("MmcUsing", siteModel.ModernMethodsOfConstruction);
    }

    [HttpPost("{siteId}/mmc-using")]
    [WorkflowState(SiteWorkflowState.MmcUsing)]
    public async Task<IActionResult> MmcUsing(SiteModernMethodsOfConstruction model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand<SiteModernMethodsOfConstruction>(
            new ProvideSiteUsingModernMethodsOfConstructionCommand(this.GetSiteIdFromRoute(), model.UsingModernMethodsOfConstruction),
            nameof(MmcUsing),
            _ => model,
            cancellationToken);
    }

    [HttpGet("{siteId}/mmc-future-adoption")]
    [WorkflowState(SiteWorkflowState.MmcFutureAdoption)]
    public async Task<IActionResult> MmcFutureAdoption([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await GetSiteDetails(siteId, cancellationToken);
        return View("MmcFutureAdoption", siteModel.ModernMethodsOfConstruction);
    }

    [HttpPost("{siteId}/mmc-future-adoption")]
    [WorkflowState(SiteWorkflowState.MmcFutureAdoption)]
    public async Task<IActionResult> MmcFutureAdoption(SiteModernMethodsOfConstruction model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand<SiteModernMethodsOfConstruction>(
            new ProvideModernMethodsConstructionFutureAdoptionCommand(this.GetSiteIdFromRoute(), model.FutureAdoptionPlans, model.FutureAdoptionExpectedImpact),
            nameof(MmcFutureAdoption),
            _ => model,
            cancellationToken);
    }

    [HttpGet("{siteId}/mmc-information")]
    [WorkflowState(SiteWorkflowState.MmcInformation)]
    public async Task<IActionResult> MmcInformation([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await GetSiteDetails(siteId, cancellationToken);
        return View("MmcInformation", siteModel.ModernMethodsOfConstruction);
    }

    [HttpPost("{siteId}/mmc-information")]
    [WorkflowState(SiteWorkflowState.MmcInformation)]
    public async Task<IActionResult> MmcInformation(SiteModernMethodsOfConstruction model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand<SiteModernMethodsOfConstruction>(
            new ProvideModernMethodsConstructionInformationCommand(this.GetSiteIdFromRoute(), model.InformationBarriers, model.InformationImpact),
            nameof(MmcInformation),
            _ => model,
            cancellationToken);
    }

    [HttpGet("{siteId}/mmc-categories")]
    [WorkflowState(SiteWorkflowState.MmcCategories)]
    public async Task<IActionResult> MmcCategories([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await GetSiteDetails(siteId, cancellationToken);
        return View("MmcCategories", siteModel.ModernMethodsOfConstruction);
    }

    [HttpPost("{siteId}/mmc-categories")]
    [WorkflowState(SiteWorkflowState.MmcCategories)]
    public async Task<IActionResult> MmcCategories(SiteModernMethodsOfConstruction model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand<SiteModernMethodsOfConstruction>(
            new ProvideModernMethodsConstructionCategoriesCommand(this.GetSiteIdFromRoute(), model.ModernMethodsConstructionCategories),
            nameof(MmcCategories),
            _ => model,
            cancellationToken);
    }

    [HttpGet("{siteId}/mmc-3d-category")]
    [WorkflowState(SiteWorkflowState.Mmc3DCategory)]
    public async Task<IActionResult> Mmc3DCategory([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await GetSiteDetails(siteId, cancellationToken);
        return View("Mmc3DCategory", siteModel.ModernMethodsOfConstruction);
    }

    [HttpPost("{siteId}/mmc-3d-category")]
    [WorkflowState(SiteWorkflowState.Mmc3DCategory)]
    public async Task<IActionResult> Mmc3DCategory(SiteModernMethodsOfConstruction model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand<SiteModernMethodsOfConstruction>(
            new ProvideModernMethodsConstruction3DSubcategoriesCommand(this.GetSiteIdFromRoute(), model.ModernMethodsConstruction3DSubcategories),
            nameof(Mmc3DCategory),
            _ => model,
            cancellationToken);
    }

    [HttpGet("{siteId}/mmc-2d-category")]
    [WorkflowState(SiteWorkflowState.Mmc2DCategory)]
    public async Task<IActionResult> Mmc2DCategory([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await GetSiteDetails(siteId, cancellationToken);
        return View("Mmc2DCategory", siteModel.ModernMethodsOfConstruction);
    }

    [HttpPost("{siteId}/mmc-2d-category")]
    [WorkflowState(SiteWorkflowState.Mmc2DCategory)]
    public async Task<IActionResult> Mmc2DCategory(SiteModernMethodsOfConstruction model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand<SiteModernMethodsOfConstruction>(
            new ProvideModernMethodsConstruction2DSubcategoriesCommand(this.GetSiteIdFromRoute(), model.ModernMethodsConstruction2DSubcategories),
            nameof(Mmc2DCategory),
            _ => model,
            cancellationToken);
    }

    [HttpGet("{siteId}/environmental-impact")]
    [WorkflowState(SiteWorkflowState.EnvironmentalImpact)]
    public async Task<IActionResult> EnvironmentalImpact([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await GetSiteDetails(siteId, cancellationToken);
        return View("EnvironmentalImpact", siteModel);
    }

    [HttpPost("{siteId}/environmental-impact")]
    [WorkflowState(SiteWorkflowState.EnvironmentalImpact)]
    public async Task<IActionResult> EnvironmentalImpact(SiteModel model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand<SiteModel>(
            new ProvideSiteEnvironmentalImpactCommand(this.GetSiteIdFromRoute(), model.EnvironmentalImpact),
            nameof(EnvironmentalImpact),
            _ => model,
            cancellationToken);
    }

    [HttpGet("{siteId}/procurements")]
    [WorkflowState(SiteWorkflowState.Procurements)]
    public async Task<IActionResult> Procurements([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await GetSiteDetails(siteId, cancellationToken);
        return View("Procurements", siteModel);
    }

    [HttpPost("{siteId}/procurements")]
    [WorkflowState(SiteWorkflowState.Procurements)]
    public async Task<IActionResult> Procurements(SiteModel model, CancellationToken cancellationToken)
    {
        return await ExecuteSiteCommand<SiteModel>(
            new ProvideSiteProcurementsCommand(this.GetSiteIdFromRoute(), model.SiteProcurements),
            nameof(Procurements),
            _ => model,
            cancellationToken);
    }

    [HttpGet("{siteId}/check-answers")]
    [WorkflowState(SiteWorkflowState.CheckAnswers)]
    public async Task<IActionResult> CheckAnswers(CancellationToken cancellationToken)
    {
        return View("CheckAnswers", await CreateSiteSummary(cancellationToken));
    }

    [HttpPost("{siteId}/check-answers")]
    [WorkflowState(SiteWorkflowState.CheckAnswers)]
    public async Task<IActionResult> Complete([FromRoute] string siteId, [FromForm] IsSectionCompleted isSectionCompleted, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<SiteSummaryViewModel>(
            _mediator,
            new CompleteSiteCommand(SiteId.From(siteId), isSectionCompleted),
            () => Task.FromResult<IActionResult>(RedirectToAction("Index")),
            async () => View("CheckAnswers", await CreateSiteSummary(cancellationToken, isSectionCompleted)),
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

        return CreateChangedFlowWorkflow(
            new SiteWorkflow(currentState, siteModel),
            currentState,
            changedState => new SiteWorkflow(changedState, siteModel, true));
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
            async () => await this.ReturnToSitesListOrContinue(async () => await ContinueWithWorkflow(new { siteId })),
            async () =>
            {
                var siteDetails = await GetSiteDetails(siteId.Value, cancellationToken);
                var model = createViewModelForError(siteDetails);

                return View(viewName, model);
            },
            cancellationToken);
    }

    private async Task<SiteModel> CreateSiteNameModel(string? siteId, string? fdProjectId, string? fdSiteId, CancellationToken cancellationToken)
    {
        if (siteId.IsProvided())
        {
            return await _mediator.Send(new GetSiteQuery(siteId!), cancellationToken);
        }

        if (fdProjectId.IsProvided() && fdSiteId.IsProvided())
        {
            var prefillData = await _mediator.Send(
                new GetNewAhpSitePrefillDataQuery(FrontDoorProjectId.From(fdProjectId!), FrontDoorSiteId.From(fdSiteId!)),
                cancellationToken);
            return new SiteModel { Name = prefillData.SiteName ?? string.Empty };
        }

        return new SiteModel();
    }

    private async Task<SiteModel> GetSiteDetails(string siteId, CancellationToken cancellationToken)
    {
        var siteModel = await _mediator.Send(new GetSiteQuery(siteId), cancellationToken);
        ViewBag.SiteName = siteModel.Name;
        return siteModel;
    }

    private async Task<SiteBasicModel> GetSiteBasicDetails(string siteId, CancellationToken cancellationToken)
    {
        var siteBasicModel = await _mediator.Send(new GetSiteBasicDetailsQuery(siteId), cancellationToken);
        ViewBag.SiteName = siteBasicModel.Name;
        return siteBasicModel;
    }

    private async Task<SiteSummaryViewModel> CreateSiteSummary(
        CancellationToken cancellationToken,
        IsSectionCompleted? isSectionCompleted = null,
        bool useWorkflowRedirection = true)
    {
        var siteId = this.GetSiteIdFromRoute();
        var siteDetails = await GetSiteDetails(siteId.Value, cancellationToken);
        var isEditable = await _accountAccessContext.CanEditApplication();
        var sections = _siteSummaryViewModelFactory.CreateSiteSummary(siteDetails, Url, isEditable, useWorkflowRedirection);

        return new SiteSummaryViewModel(
            siteId.Value,
            isSectionCompleted ?? (siteDetails.Status == SiteStatus.Completed ? IsSectionCompleted.Yes : IsSectionCompleted.Undefied),
            sections.ToList(),
            isEditable);
    }

    private async Task<SelectPartnerModel> GetSelectPartnerModel(string siteId, int? page, CancellationToken cancellationToken)
    {
        var site = await GetSiteBasicDetails(siteId, cancellationToken);
        var partners = await _mediator.Send(new GetConsortiumMembersQuery(new PaginationRequest(page ?? 1)), cancellationToken);

        return new SelectPartnerModel(site.Id, site.Name, partners);
    }

    private async Task<(OrganisationDetails Organisation, bool? IsConfirmed)> GetConfirmPartnerModel(
        string siteId,
        string organisationId,
        Func<SiteModel, string?> getSelectedPartnerId,
        CancellationToken cancellationToken)
    {
        var site = await GetSiteDetails(siteId, cancellationToken);
        var organisationDetails = await _mediator.Send(new GetOrganisationDetailsQuery(OrganisationId.From(organisationId)), cancellationToken);
        var currentlySelectedPartner = getSelectedPartnerId(site);

        return (organisationDetails, organisationDetails.OrganisationId == currentlySelectedPartner ? true : null);
    }
}
