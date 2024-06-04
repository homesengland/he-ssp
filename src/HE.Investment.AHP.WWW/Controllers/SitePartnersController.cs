using HE.Investment.AHP.Contract.Common.Queries;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.Contract.Site.Commands;
using HE.Investment.AHP.Contract.SitePartners;
using HE.Investment.AHP.WWW.Extensions;
using HE.Investment.AHP.WWW.Models.Common;
using HE.Investment.AHP.WWW.Models.SitePartners;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.WWW.Controllers;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.Organisation.Contract;
using HE.Investments.Organisation.Contract.Commands;
using HE.Investments.Organisation.Contract.Queries;
using HE.Investments.Organisation.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[Route("site-partners")]
[AuthorizeWithCompletedProfile]
public class SitePartnersController : SiteControllerBase<SitePartnersWorkflowState>
{
    private readonly IMediator _mediator;

    public SitePartnersController(IMediator mediator)
        : base(mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{siteId}/back")]
    public async Task<IActionResult> Back([FromRoute] string siteId, SitePartnersWorkflowState currentPage)
    {
        return await Back(currentPage, new { siteId, isBack = true });
    }

    [HttpGet("{siteId}/start")]
    [WorkflowState(SitePartnersWorkflowState.FlowStarted)]
    public async Task<IActionResult> StartSitePartnersFlow([FromRoute] string siteId, [FromQuery] string? workflow, [FromQuery] bool isBack)
    {
        return isBack
            ? RedirectToAction("StartSitePartnersFlow", "Site", new { siteId, isBack })
            : await Continue(new { siteId, workflow });
    }

    [HttpGet("{siteId}/developing-partner")]
    [WorkflowState(SitePartnersWorkflowState.DevelopingPartner)]
    public async Task<IActionResult> DevelopingPartner([FromRoute] string siteId, [FromQuery] int? page, CancellationToken cancellationToken)
    {
        return View(await GetSelectPartnerModel(siteId, page, cancellationToken));
    }

    [HttpGet("{siteId}/developing-partner-confirm/{organisationId}")]
    [WorkflowState(SitePartnersWorkflowState.DevelopingPartnerConfirm)]
    public async Task<IActionResult> DevelopingPartnerConfirm([FromRoute] string siteId, [FromRoute] string organisationId, CancellationToken cancellationToken)
    {
        return View(await GetConfirmPartnerModel(siteId, organisationId, x => x.DevelopingPartner?.OrganisationId, cancellationToken));
    }

    [HttpPost("{siteId}/developing-partner-confirm/{organisationId}")]
    [WorkflowState(SitePartnersWorkflowState.DevelopingPartnerConfirm)]
    public async Task<IActionResult> DevelopingPartnerConfirm([FromRoute] string siteId, [FromRoute] string organisationId, [FromForm] bool? isConfirmed, [FromQuery] string? workflow, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<(OrganisationDetails Organisation, bool? IsConfirmed)>(
            _mediator,
            new ProvideDevelopingPartnerCommand(SiteId.From(siteId), OrganisationId.From(organisationId), isConfirmed),
            async () => await this.ReturnToSitesListOrContinue(
                (await GetSiteDetails(siteId, cancellationToken)).ProjectId,
                async () => isConfirmed == true ? await ContinueWithWorkflow(new { siteId }) : RedirectToAction("DevelopingPartner", new { siteId, workflow })),
            async () =>
            {
                var (organisation, _) = await GetConfirmPartnerModel(siteId, organisationId, x => x.DevelopingPartner?.OrganisationId, cancellationToken);
                return View((organisation, isConfirmed));
            },
            cancellationToken);
    }

    [HttpGet("{siteId}/owner-of-the-land")]
    [WorkflowState(SitePartnersWorkflowState.OwnerOfTheLand)]
    public async Task<IActionResult> OwnerOfTheLand([FromRoute] string siteId, [FromQuery] int? page, CancellationToken cancellationToken)
    {
        return View(await GetSelectPartnerModel(siteId, page, cancellationToken));
    }

    [HttpGet("{siteId}/owner-of-the-land-confirm/{organisationId}")]
    [WorkflowState(SitePartnersWorkflowState.OwnerOfTheLandConfirm)]
    public async Task<IActionResult> OwnerOfTheLandConfirm([FromRoute] string siteId, [FromRoute] string organisationId, CancellationToken cancellationToken)
    {
        return View(await GetConfirmPartnerModel(siteId, organisationId, x => x.OwnerOfTheLand?.OrganisationId, cancellationToken));
    }

    [HttpPost("{siteId}/owner-of-the-land-confirm/{organisationId}")]
    [WorkflowState(SitePartnersWorkflowState.OwnerOfTheLandConfirm)]
    public async Task<IActionResult> OwnerOfTheLandConfirm([FromRoute] string siteId, [FromRoute] string organisationId, [FromForm] bool? isConfirmed, [FromQuery] string? workflow, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<(OrganisationDetails Organisation, bool? IsConfirmed)>(
            _mediator,
            new ProvideOwnerOfTheLandCommand(SiteId.From(siteId), OrganisationId.From(organisationId), isConfirmed),
            async () => await this.ReturnToSitesListOrContinue(
                (await GetSiteDetails(siteId, cancellationToken)).ProjectId,
                async () => isConfirmed == true ? await ContinueWithWorkflow(new { siteId }) : RedirectToAction("OwnerOfTheLand", new { siteId, workflow })),
            async () =>
            {
                var (organisation, _) = await GetConfirmPartnerModel(siteId, organisationId, x => x.OwnerOfTheLand?.OrganisationId, cancellationToken);
                return View((organisation, isConfirmed));
            },
            cancellationToken);
    }

    [HttpGet("{siteId}/owner-of-the-homes")]
    [WorkflowState(SitePartnersWorkflowState.OwnerOfTheHomes)]
    public async Task<IActionResult> OwnerOfTheHomes([FromRoute] string siteId, [FromQuery] int? page, CancellationToken cancellationToken)
    {
        return View(await GetSelectPartnerModel(siteId, page, cancellationToken));
    }

    [HttpGet("{siteId}/owner-of-the-homes-confirm/{organisationId}")]
    [WorkflowState(SitePartnersWorkflowState.OwnerOfTheHomesConfirm)]
    public async Task<IActionResult> OwnerOfTheHomesConfirm([FromRoute] string siteId, [FromRoute] string organisationId, CancellationToken cancellationToken)
    {
        return View(await GetConfirmPartnerModel(siteId, organisationId, x => x.OwnerOfTheHomes?.OrganisationId, cancellationToken));
    }

    [HttpPost("{siteId}/owner-of-the-homes-confirm/{organisationId}")]
    [WorkflowState(SitePartnersWorkflowState.OwnerOfTheHomesConfirm)]
    public async Task<IActionResult> OwnerOfTheHomesConfirm([FromRoute] string siteId, [FromRoute] string organisationId, [FromForm] bool? isConfirmed, [FromQuery] string? workflow, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<(OrganisationDetails Organisation, bool? IsConfirmed)>(
            _mediator,
            new ProvideOwnerOfTheHomesCommand(SiteId.From(siteId), OrganisationId.From(organisationId), isConfirmed),
            async () => await this.ReturnToSitesListOrContinue(
                (await GetSiteDetails(siteId, cancellationToken)).ProjectId,
                async () => isConfirmed == true ? await ContinueWithWorkflow(new { siteId }) : RedirectToAction("OwnerOfTheHomes", new { siteId, workflow })),
            async () =>
            {
                var (organisation, _) = await GetConfirmPartnerModel(siteId, organisationId, x => x.OwnerOfTheHomes?.OrganisationId, cancellationToken);
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
            return RedirectToAction("UnregisteredBodySearchNoResults", "SitePartners", new { siteId, workflow });
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
            await GetSiteBasicDetails(siteId, cancellationToken);

            return View("UnregisteredBodyCreateManual", model);
        }

        return await this.ReturnToSitesListOrContinue(
            siteBasicDetails.ProjectId,
            async () => await Continue(new { siteId, organisationId = result.ReturnedData.Id.Value, workflow }));
    }

    [HttpGet("{siteId}/unregistered-body-confirm/{organisationId}")]
    [WorkflowState(SitePartnersWorkflowState.UnregisteredBodyConfirm)]
    public async Task<IActionResult> UnregisteredBodyConfirm([FromRoute] string siteId, [FromRoute] string organisationId, CancellationToken cancellationToken)
    {
        return View(await GetConfirmPartnerModel(siteId, organisationId, x => x.OwnerOfTheHomes?.OrganisationId, cancellationToken));
    }

    [HttpPost("{siteId}/unregistered-body-confirm/{organisationIdentifier}")]
    [WorkflowState(SitePartnersWorkflowState.UnregisteredBodyConfirm)]
    public async Task<IActionResult> UnregisteredBodyConfirm([FromRoute] string siteId, [FromRoute] string organisationIdentifier, [FromForm] bool? isConfirmed, [FromQuery] string? workflow, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<(OrganisationDetails Organisation, bool? IsConfirmed)>(
            _mediator,
            new ProvideUnregisteredBodyOwnerOfTheHomesCommand(SiteId.From(siteId), new OrganisationIdentifier(organisationIdentifier), isConfirmed),
            async () => await this.ReturnToSitesListOrContinue(
                (await GetSiteDetails(siteId, cancellationToken)).ProjectId,
                async () => isConfirmed == true ? await ContinueWithWorkflow(new { siteId }) : RedirectToAction("UnregisteredBodySearch", new { siteId, workflow })),
            async () =>
            {
                var (organisation, _) = await GetConfirmPartnerModel(siteId, organisationIdentifier, x => x.OwnerOfTheHomes?.OrganisationId, cancellationToken);
                return View((organisation, isConfirmed));
            },
            cancellationToken);
    }

    [HttpGet("{siteId}/finish")]
    [WorkflowState(SitePartnersWorkflowState.FlowFinished)]
    public async Task<IActionResult> FinishSitePartnersFlow([FromRoute] string siteId, [FromQuery] string? workflow, [FromQuery] bool isBack)
    {
        return isBack
            ? await Back(new { siteId, isBack })
            : RedirectToAction("FinishSitePartnersFlow", "Site", new { siteId, workflow });
    }

    protected override async Task<IStateRouting<SitePartnersWorkflowState>> Routing(SitePartnersWorkflowState currentState, object? routeData = null)
    {
        var siteId = Request.GetRouteValue("siteId")!;
        var site = await GetSiteDetails(siteId, CancellationToken.None);

        return await Task.FromResult<IStateRouting<SitePartnersWorkflowState>>(new SitePartnersWorkflow(currentState, site));
    }

    private async Task<SelectPartnerModel> GetSelectPartnerModel(string siteId, int? page, CancellationToken cancellationToken)
    {
        var site = await GetSiteBasicDetails(siteId, cancellationToken);
        var partners = await _mediator.Send(new GetConsortiumMembersQuery(new PaginationRequest(page ?? 1)), cancellationToken);

        return new SelectPartnerModel(site.Id, site.Name, partners);
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
