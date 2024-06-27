using HE.Investment.AHP.Contract.Common.Queries;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Contract.Site.Queries;
using HE.Investment.AHP.Contract.SitePartners;
using HE.Investment.AHP.WWW.Extensions;
using HE.Investment.AHP.WWW.Models.Common;
using HE.Investment.AHP.WWW.Models.SitePartners;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.WWW.Controllers;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.Consortium.Shared.Authorization;
using HE.Investments.Consortium.Shared.UserContext;
using HE.Investments.Organisation.Contract;
using HE.Investments.Organisation.Contract.Commands;
using HE.Investments.Organisation.Contract.Queries;
using HE.Investments.Organisation.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[ConsortiumAuthorize(ConsortiumAccessContext.Edit)]
[Route("{organisationId}/site-partners")]
public class SitePartnersController : SiteControllerBase<SitePartnersWorkflowState>
{
    private readonly IMediator _mediator;

    public SitePartnersController(IMediator mediator)
        : base(mediator)
    {
        _mediator = mediator;
    }

    [ConsortiumAuthorize]
    [HttpGet("{siteId}/back")]
    public async Task<IActionResult> Back([FromRoute] string siteId, SitePartnersWorkflowState currentPage, string partnerId)
    {
        return await Back(currentPage, new { siteId, partnerId, isBack = true });
    }

    [HttpGet("{siteId}/start")]
    [WorkflowState(SitePartnersWorkflowState.FlowStarted)]
    public async Task<IActionResult> StartSitePartnersFlow([FromRoute] string siteId, [FromQuery] string? workflow, [FromQuery] bool isBack)
    {
        return isBack
            ? this.OrganisationRedirectToAction("StartSitePartnersFlow", "Site", new { siteId, isBack })
            : await Continue(new { siteId, workflow });
    }

    [HttpGet("{siteId}/developing-partner")]
    [WorkflowState(SitePartnersWorkflowState.DevelopingPartner)]
    public async Task<IActionResult> DevelopingPartner([FromRoute] string siteId, [FromQuery] int? page, CancellationToken cancellationToken)
    {
        return View(await GetSelectPartnerModel(siteId, page, cancellationToken));
    }

    [HttpGet("{siteId}/developing-partner-confirm/{partnerId}")]
    [WorkflowState(SitePartnersWorkflowState.DevelopingPartnerConfirm)]
    public async Task<IActionResult> DevelopingPartnerConfirm([FromRoute] string siteId, [FromRoute] string partnerId, CancellationToken cancellationToken)
    {
        return View(await GetConfirmPartnerModel(siteId, partnerId, x => x.DevelopingPartner?.OrganisationId, cancellationToken));
    }

    [HttpPost("{siteId}/developing-partner-confirm/{partnerId}")]
    [WorkflowState(SitePartnersWorkflowState.DevelopingPartnerConfirm)]
    public async Task<IActionResult> DevelopingPartnerConfirm(
        [FromRoute] string siteId,
        [FromRoute] string partnerId,
        [FromForm] bool? isConfirmed,
        [FromQuery] string? workflow,
        CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<(OrganisationDetails Organisation, bool? IsConfirmed)>(
            _mediator,
            new ProvideDevelopingPartnerCommand(SiteId.From(siteId), OrganisationId.From(partnerId), isConfirmed),
            async () => await this.ReturnToSitesListOrContinue(
                (await GetSiteDetails(siteId, cancellationToken)).ProjectId,
                async () => isConfirmed == true
                    ? await ContinueWithWorkflow(new { siteId })
                    : this.OrganisationRedirectToAction("DevelopingPartner", routeValues: new { siteId, workflow })),
            async () =>
            {
                var (organisation, _) = await GetConfirmPartnerModel(siteId, partnerId, x => x.DevelopingPartner?.OrganisationId, cancellationToken);
                return View((organisation, isConfirmed));
            },
            cancellationToken);
    }

    [HttpGet("{siteId}/owner-of-the-land")]
    [WorkflowState(SitePartnersWorkflowState.OwnerOfTheLand)]
    public async Task<IActionResult> OwnerOfTheLand([FromRoute] string siteId, [FromQuery] int? page, CancellationToken cancellationToken)
    {
        return View(await GetSelectPartnerModel(siteId, page, cancellationToken, SitePartnersWorkflowState.DevelopingPartnerConfirm));
    }

    [HttpGet("{siteId}/owner-of-the-land-confirm/{partnerId}")]
    [WorkflowState(SitePartnersWorkflowState.OwnerOfTheLandConfirm)]
    public async Task<IActionResult> OwnerOfTheLandConfirm([FromRoute] string siteId, [FromRoute] string partnerId, CancellationToken cancellationToken)
    {
        return View(await GetConfirmPartnerModel(siteId, partnerId, x => x.OwnerOfTheLand?.OrganisationId, cancellationToken));
    }

    [HttpPost("{siteId}/owner-of-the-land-confirm/{partnerId}")]
    [WorkflowState(SitePartnersWorkflowState.OwnerOfTheLandConfirm)]
    public async Task<IActionResult> OwnerOfTheLandConfirm(
        [FromRoute] string siteId,
        [FromRoute] string partnerId,
        [FromForm] bool? isConfirmed,
        [FromQuery] string? workflow,
        CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<(OrganisationDetails Organisation, bool? IsConfirmed)>(
            _mediator,
            new ProvideOwnerOfTheLandCommand(SiteId.From(siteId), OrganisationId.From(partnerId), isConfirmed),
            async () => await this.ReturnToSitesListOrContinue(
                (await GetSiteDetails(siteId, cancellationToken)).ProjectId,
                async () => isConfirmed == true
                    ? await ContinueWithWorkflow(new { siteId })
                    : this.OrganisationRedirectToAction("OwnerOfTheLand", routeValues: new { siteId, workflow })),
            async () =>
            {
                var (organisation, _) = await GetConfirmPartnerModel(siteId, partnerId, x => x.OwnerOfTheLand?.OrganisationId, cancellationToken);
                return View((organisation, isConfirmed));
            },
            cancellationToken);
    }

    [HttpGet("{siteId}/owner-of-the-homes")]
    [WorkflowState(SitePartnersWorkflowState.OwnerOfTheHomes)]
    public async Task<IActionResult> OwnerOfTheHomes([FromRoute] string siteId, [FromQuery] int? page, CancellationToken cancellationToken)
    {
        return View(await GetSelectPartnerModel(siteId, page, cancellationToken, SitePartnersWorkflowState.OwnerOfTheLandConfirm));
    }

    [HttpGet("{siteId}/owner-of-the-homes-confirm/{partnerId}")]
    [WorkflowState(SitePartnersWorkflowState.OwnerOfTheHomesConfirm)]
    public async Task<IActionResult> OwnerOfTheHomesConfirm([FromRoute] string siteId, [FromRoute] string partnerId, CancellationToken cancellationToken)
    {
        return View(await GetConfirmPartnerModel(siteId, partnerId, x => x.OwnerOfTheHomes?.OrganisationId, cancellationToken));
    }

    [HttpPost("{siteId}/owner-of-the-homes-confirm/{partnerId}")]
    [WorkflowState(SitePartnersWorkflowState.OwnerOfTheHomesConfirm)]
    public async Task<IActionResult> OwnerOfTheHomesConfirm(
        [FromRoute] string siteId,
        [FromRoute] string partnerId,
        [FromForm] bool? isConfirmed,
        [FromQuery] string? workflow,
        CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<(OrganisationDetails Organisation, bool? IsConfirmed)>(
            _mediator,
            new ProvideOwnerOfTheHomesCommand(SiteId.From(siteId), OrganisationId.From(partnerId), isConfirmed),
            async () => await this.ReturnToSitesListOrContinue(
                (await GetSiteDetails(siteId, cancellationToken)).ProjectId,
                async () => isConfirmed == true
                    ? await ContinueWithWorkflow(new { siteId })
                    : this.OrganisationRedirectToAction("OwnerOfTheHomes", routeValues: new { siteId, workflow })),
            async () =>
            {
                var (organisation, _) = await GetConfirmPartnerModel(siteId, partnerId, x => x.OwnerOfTheHomes?.OrganisationId, cancellationToken);
                return View((organisation, isConfirmed));
            },
            cancellationToken);
    }

    [HttpGet("{siteId}/unregistered-body-search")]
    [WorkflowState(SitePartnersWorkflowState.UnregisteredBodySearch)]
    public async Task<IActionResult> UnregisteredBodySearch([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        await GetSiteDetails(siteId, cancellationToken);
        return View("UnregisteredBodySearch");
    }

    [HttpPost("{siteId}/unregistered-body-search")]
    [WorkflowState(SitePartnersWorkflowState.UnregisteredBodySearch)]
    public async Task<IActionResult> UnregisteredBodySearch(
        [FromRoute] string siteId,
        string? phrase,
        [FromQuery] string? workflow,
        CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<ProvideSearchOrganisationPhraseCommand>(
            _mediator,
            new ProvideSearchOrganisationPhraseCommand(SiteId.From(siteId), phrase),
            async () => await Continue(new { siteId, phrase, workflow }),
            async () =>
            {
                await GetSiteDetails(siteId, cancellationToken);
                return View("UnregisteredBodySearch", phrase);
            },
            cancellationToken);
    }

    [HttpGet("{siteId}/unregistered-body-search-result")]
    [WorkflowState(SitePartnersWorkflowState.UnregisteredBodySearchResult)]
    public async Task<IActionResult> UnregisteredBodySearchResult(
        [FromRoute] string siteId,
        string? phrase,
        int? page,
        [FromQuery] string? workflow,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new SearchOrganisationQuery(phrase ?? string.Empty, new PaginationRequest(page ?? 0)), cancellationToken);
        if (result.Page.TotalItems == 0)
        {
            return this.OrganisationRedirectToAction("UnregisteredBodySearchNoResults", "SitePartners", new { siteId, workflow });
        }

        return View(
            "UnregisteredBodySearchResult",
            new SearchOrganisationResultModel(
                phrase ?? string.Empty,
                $"unregistered-body-search-result?phrase={phrase}&workflow={workflow}",
                result.Page,
                string.Empty));
    }

    [HttpGet("{siteId}/unregistered-body-search-no-results")]
    [WorkflowState(SitePartnersWorkflowState.UnregisteredBodySearchNoResults)]
    public async Task<IActionResult> UnregisteredBodySearchNoResults([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        await GetSiteDetails(siteId, cancellationToken);
        return View("UnregisteredBodySearchNoResults");
    }

    [HttpGet("{siteId}/unregistered-body-create-manual")]
    [WorkflowState(SitePartnersWorkflowState.UnregisteredBodyCreateManual)]
    public async Task<IActionResult> UnregisteredBodyCreateManual([FromRoute] string siteId, CancellationToken cancellationToken)
    {
        await GetSiteBasicDetails(siteId, cancellationToken);
        return View(new AddOrganisationModel(null, null, null, null, null, null));
    }

    [HttpPost("{siteId}/unregistered-body-create-manual")]
    [WorkflowState(SitePartnersWorkflowState.UnregisteredBodyCreateManual)]
    public async Task<IActionResult> UnregisteredBodyCreateManual(
        [FromRoute] string siteId,
        AddOrganisationModel model,
        [FromQuery] string? workflow,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new CreateManualOrganisationCommand(model.Name, model.AddressLine1, model.AddressLine2, model.TownOrCity, model.County, model.Postcode),
            cancellationToken);
        var siteBasicDetails = await GetSiteBasicDetails(siteId, cancellationToken);
        if (result.HasValidationErrors)
        {
            this.AddOrderedErrors<CreateManualOrganisationCommand>(result);

            return View("UnregisteredBodyCreateManual", model);
        }

        return await this.ReturnToSitesListOrContinue(
            siteBasicDetails.ProjectId,
            async () => await Continue(new { siteId, organisationIdentifier = result.ReturnedData.Id.Value, workflow }));
    }

    [HttpGet("{siteId}/unregistered-body-confirm/{partnerId}")]
    [WorkflowState(SitePartnersWorkflowState.UnregisteredBodyConfirm)]
    public async Task<IActionResult> UnregisteredBodyConfirm([FromRoute] string siteId, [FromRoute] string partnerId, CancellationToken cancellationToken)
    {
        return View(await GetConfirmPartnerModel(siteId, partnerId, x => x.OwnerOfTheHomes?.OrganisationId, cancellationToken));
    }

    [HttpPost("{siteId}/unregistered-body-confirm/{organisationIdentifier}")]
    [WorkflowState(SitePartnersWorkflowState.UnregisteredBodyConfirm)]
    public async Task<IActionResult> UnregisteredBodyConfirm(
        [FromRoute] string siteId,
        [FromRoute] string organisationIdentifier,
        [FromForm] bool? isConfirmed,
        [FromQuery] string? workflow,
        CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<(OrganisationDetails Organisation, bool? IsConfirmed)>(
            _mediator,
            new ProvideUnregisteredBodyOwnerOfTheHomesCommand(SiteId.From(siteId), new OrganisationIdentifier(organisationIdentifier), isConfirmed),
            async () => await this.ReturnToSitesListOrContinue(
                (await GetSiteDetails(siteId, cancellationToken)).ProjectId,
                async () => isConfirmed == true
                    ? await ContinueWithWorkflow(new { siteId })
                    : this.OrganisationRedirectToAction("UnregisteredBodySearch", routeValues: new { siteId, workflow })),
            async () =>
            {
                var (organisation, _) = await GetConfirmPartnerModel(siteId, organisationIdentifier, x => x.OwnerOfTheHomes?.OrganisationId, cancellationToken);
                return View((organisation, isConfirmed));
            },
            cancellationToken);
    }

    [HttpGet("{siteId}/finish")]
    [WorkflowState(SitePartnersWorkflowState.FlowFinished)]
    public async Task<IActionResult> FinishSitePartnersFlow([FromRoute] string siteId, [FromQuery] string? workflow, [FromQuery] bool isBack, CancellationToken cancellationToken)
    {
        if (isBack)
        {
            var site = await GetSiteDetails(siteId, cancellationToken);
            return await Back(new { siteId, isBack, partnerId = site.OwnerOfTheHomes?.OrganisationId });
        }

        return this.OrganisationRedirectToAction("FinishSitePartnersFlow", "Site", new { siteId, workflow });
    }

    protected override async Task<IStateRouting<SitePartnersWorkflowState>> Routing(SitePartnersWorkflowState currentState, object? routeData = null)
    {
        var siteId = Request.GetRouteValue("siteId")!;
        var site = await GetSiteDetails(siteId, CancellationToken.None);

        return await Task.FromResult<IStateRouting<SitePartnersWorkflowState>>(new SitePartnersWorkflow(currentState, site));
    }

    private async Task<SelectPartnerModel> GetSelectPartnerModel(string siteId, int? page, CancellationToken cancellationToken, SitePartnersWorkflowState? previousState = null)
    {
        var site = await GetSiteDetails(siteId, cancellationToken);
        var previousPagePartnerId = previousState != null ? GetPreviousPartnerId(previousState.Value, site) : null;

        var partners = await _mediator.Send(new GetConsortiumMembersQuery(new PaginationRequest(page ?? 1)), cancellationToken);
        return new SelectPartnerModel(siteId, site.Name, partners, previousPagePartnerId);
    }

    private string? GetPreviousPartnerId(SitePartnersWorkflowState state, SiteModel model)
    {
        return state switch
        {
            SitePartnersWorkflowState.DevelopingPartnerConfirm => model.DevelopingPartner?.OrganisationId,
            SitePartnersWorkflowState.OwnerOfTheLandConfirm => model.OwnerOfTheLand?.OrganisationId,
            SitePartnersWorkflowState.OwnerOfTheHomesConfirm => model.OwnerOfTheHomes?.OrganisationId,
            _ => throw new ArgumentOutOfRangeException($"Invalid {nameof(SitePartnersWorkflowState)} {state}"),
        };
    }

    private async Task<(OrganisationDetails Organisation, bool? IsConfirmed)> GetConfirmPartnerModel(
        string siteId,
        string organisationIdentifier,
        Func<SiteModel, string?> getSelectedPartnerId,
        CancellationToken cancellationToken)
    {
        var site = await GetSiteDetails(siteId, cancellationToken);
        var organisationDetails = await _mediator.Send(new GetOrganisationDetailsQuery(new OrganisationIdentifier(organisationIdentifier)), cancellationToken);
        var currentlySelectedPartner = getSelectedPartnerId(site);

        return (
            organisationDetails,
            !string.IsNullOrEmpty(organisationDetails.OrganisationId) && !string.IsNullOrEmpty(currentlySelectedPartner) &&
            OrganisationId.From(organisationDetails.OrganisationId) == OrganisationId.From(currentlySelectedPartner)
                ? true
                : null);
    }
}
