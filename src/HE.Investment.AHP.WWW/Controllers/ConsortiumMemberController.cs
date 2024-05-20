using HE.Investment.AHP.Domain.UserContext;
using HE.Investment.AHP.WWW.Models.Consortium;
using HE.Investment.AHP.WWW.Models.ConsortiumMember;
using HE.Investment.AHP.WWW.Views.Shared.Components.OrganisationDetailsComponent;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Contract.Commands;
using HE.Investments.AHP.Consortium.Contract.Enums;
using HE.Investments.AHP.Consortium.Contract.Queries;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Components;
using HE.Investments.Common.WWW.Controllers;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Models;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.Organisation.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[Route("consortium-member/{consortiumId}")]
[AuthorizeWithCompletedProfile(AhpAccessContext.ViewConsortium)]
public class ConsortiumMemberController : WorkflowController<ConsortiumMemberWorkflowState>
{
    private readonly IMediator _mediator;

    private readonly IAhpUserContext _ahpUserContext;

    private readonly IAhpAccessContext _ahpAccessContext;

    public ConsortiumMemberController(IMediator mediator, IAhpUserContext ahpUserContext, IAhpAccessContext ahpAccessContext)
    {
        _mediator = mediator;
        _ahpUserContext = ahpUserContext;
        _ahpAccessContext = ahpAccessContext;
    }

    [HttpGet("back")]
    public async Task<IActionResult> Back([FromRoute] string consortiumId, ConsortiumMemberWorkflowState currentPage)
    {
        return await Back(currentPage, new { consortiumId });
    }

    [HttpGet]
    [WorkflowState(ConsortiumMemberWorkflowState.Index)]
    public async Task<IActionResult> Index(string consortiumId, CancellationToken cancellationToken)
    {
        var userAccount = await _ahpUserContext.GetSelectedAccount();
        var consortium = await GetConsortiumDetails(consortiumId, fetchAddress: false, cancellationToken);
        var model = new ManageConsortiumModel(consortium, userAccount.Organisation!.RegisteredCompanyName, userAccount.CanManageConsortium);

        return consortium.LeadPartner.OrganisationId != userAccount.SelectedOrganisationId()
            ? View("MemberDashboard", model)
            : View(model);
    }

    [HttpGet("search-organisation")]
    [WorkflowState(ConsortiumMemberWorkflowState.SearchOrganisation)]
    [AuthorizeWithCompletedProfile(AhpAccessContext.ManageConsortium)]
    public IActionResult SearchOrganisation()
    {
        return View();
    }

    [HttpPost("search-organisation")]
    [WorkflowState(ConsortiumMemberWorkflowState.SearchOrganisation)]
    [AuthorizeWithCompletedProfile(AhpAccessContext.ManageConsortium)]
    public async Task<IActionResult> SearchOrganisation(string consortiumId, string? phrase, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<ProvideSearchOrganisationPhraseCommand>(
            _mediator,
            new ProvideSearchOrganisationPhraseCommand(ConsortiumId.From(consortiumId), phrase),
            async () => await Continue(new { consortiumId, phrase }),
            () => Task.FromResult<IActionResult>(View(phrase)),
            cancellationToken);
    }

    [HttpGet("search-result")]
    [WorkflowState(ConsortiumMemberWorkflowState.SearchResult)]
    [AuthorizeWithCompletedProfile(AhpAccessContext.ManageConsortium)]
    public async Task<IActionResult> SearchResult(string consortiumId, string? phrase, int? page, CancellationToken cancellationToken)
    {
        return await SearchOrganisation(consortiumId, phrase, page, null, cancellationToken);
    }

    [HttpPost("search-result")]
    [WorkflowState(ConsortiumMemberWorkflowState.SearchResult)]
    [AuthorizeWithCompletedProfile(AhpAccessContext.ManageConsortium)]
    public async Task<IActionResult> SearchResult(string consortiumId, SearchOrganisationResultModel model, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<SearchOrganisationResultModel>(
            _mediator,
            new AddOrganisationToConsortiumCommand(ConsortiumId.From(consortiumId), model.SelectedMember.IsProvided() ? new OrganisationIdentifier(model.SelectedMember!) : null),
            async () => await Continue(new { consortiumId }),
            async () => await SearchOrganisation(consortiumId, model.Phrase, model.Page.CurrentPage, model.SelectedMember, cancellationToken),
            cancellationToken);
    }

    [HttpGet("search-no-results")]
    [WorkflowState(ConsortiumMemberWorkflowState.SearchNoResults)]
    [AuthorizeWithCompletedProfile(AhpAccessContext.ManageConsortium)]
    public IActionResult SearchNoResults()
    {
        return View();
    }

    [HttpGet("add-organisation")]
    [WorkflowState(ConsortiumMemberWorkflowState.AddOrganisation)]
    [AuthorizeWithCompletedProfile(AhpAccessContext.ManageConsortium)]
    public IActionResult AddOrganisation()
    {
        return View(new AddOrganisationModel(null, null, null, null, null, null));
    }

    [HttpPost("add-organisation")]
    [WorkflowState(ConsortiumMemberWorkflowState.AddOrganisation)]
    [AuthorizeWithCompletedProfile(AhpAccessContext.ManageConsortium)]
    public async Task<IActionResult> AddOrganisation(string consortiumId, AddOrganisationModel model, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<AddOrganisationModel>(
            _mediator,
            new AddManualOrganisationToConsortiumCommand(
                ConsortiumId.From(consortiumId),
                model.Name,
                model.AddressLine1,
                model.AddressLine2,
                model.TownOrCity,
                model.County,
                model.Postcode),
            async () => await Continue(new { consortiumId }),
            () => Task.FromResult<IActionResult>(View("AddOrganisation", model)),
            cancellationToken);
    }

    [HttpGet("add-members")]
    [WorkflowState(ConsortiumMemberWorkflowState.AddMembers)]
    [AuthorizeWithCompletedProfile(AhpAccessContext.ManageConsortium)]
    public async Task<IActionResult> AddMembers(string consortiumId, CancellationToken cancellationToken)
    {
        return View(await GetConsortiumDetails(consortiumId, fetchAddress: true, cancellationToken));
    }

    [HttpPost("add-members")]
    [WorkflowState(ConsortiumMemberWorkflowState.AddMembers)]
    [AuthorizeWithCompletedProfile(AhpAccessContext.ManageConsortium)]
    public async Task<IActionResult> AddMembers(string consortiumId, [FromForm] AreAllMembersAdded areAllMembersAdded, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<ConsortiumDetails>(
            _mediator,
            new AddMembersCommand(ConsortiumId.From(consortiumId), areAllMembersAdded),
            () => Task.FromResult<IActionResult>(areAllMembersAdded == AreAllMembersAdded.Yes
                ? RedirectToAction("Index", new { consortiumId })
                : RedirectToAction("SearchOrganisation", new { consortiumId })),
            async () => View(await _mediator.Send(new GetConsortiumDetailsQuery(ConsortiumId.From(consortiumId), FetchAddress: true), cancellationToken)),
            cancellationToken);
    }

    [HttpGet("remove-member/{memberId}")]
    [WorkflowState(ConsortiumMemberWorkflowState.RemoveMember)]
    [AuthorizeWithCompletedProfile(AhpAccessContext.ManageConsortium)]
    public async Task<IActionResult> RemoveMember(string consortiumId, string memberId, CancellationToken cancellationToken)
    {
        return View(await GetConsortiumMemberDetails(consortiumId, memberId, fetchAddress: false, cancellationToken));
    }

    [HttpPost("remove-member/{memberId}")]
    [WorkflowState(ConsortiumMemberWorkflowState.RemoveMember)]
    [AuthorizeWithCompletedProfile(AhpAccessContext.ManageConsortium)]
    public async Task<IActionResult> RemoveMember(string consortiumId, string memberId, [FromForm] bool? isConfirmed, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<OrganisationDetails>(
            _mediator,
            new RemoveOrganisationFromConsortiumCommand(ConsortiumId.From(consortiumId), OrganisationId.From(memberId), isConfirmed),
            async () => await Continue(new { consortiumId }),
            async () => View("RemoveMember", await GetConsortiumMemberDetails(consortiumId, memberId, fetchAddress: false, cancellationToken)),
            cancellationToken);
    }

    [HttpGet("contact-homes-england")]
    [AuthorizeWithCompletedProfile(AhpAccessContext.ViewConsortium)]
    public async Task<IActionResult> ContactHomesEngland(string consortiumId, CancellationToken cancellationToken)
    {
        if (await _ahpAccessContext.IsConsortiumLeadPartner())
        {
            return RedirectToAction("PageNotFound", "Home");
        }

        var availableProgrammes = await _mediator.Send(new GetAvailableProgrammesQuery(), cancellationToken);

        return View(new ConsortiumSelectedProgrammeModel(consortiumId, availableProgrammes.FirstOrDefault()!));
    }

    protected override async Task<IStateRouting<ConsortiumMemberWorkflowState>> Routing(ConsortiumMemberWorkflowState currentState, object? routeData = null)
    {
        var consortiumId = Request.GetRouteValue("consortiumId")
                           ?? routeData?.GetPropertyValue<string>("consortiumId")
                           ?? string.Empty;
        var consortium = await GetConsortiumDetails(consortiumId, false, CancellationToken.None);

        return new ConsortiumMemberWorkflow(consortium, currentState);
    }

    private async Task<ConsortiumDetails> GetConsortiumDetails(string consortiumId, bool fetchAddress, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetConsortiumDetailsQuery(ConsortiumId.From(consortiumId), fetchAddress), cancellationToken);
    }

    private async Task<OrganisationDetails> GetConsortiumMemberDetails(string consortiumId, string memberId, bool fetchAddress, CancellationToken cancellationToken)
    {
        var consortium = await _mediator.Send(new GetConsortiumDetailsQuery(ConsortiumId.From(consortiumId), fetchAddress), cancellationToken);
        var member = consortium.Members.SingleOrDefault(x => x.OrganisationId.ToString() == memberId);

        return member?.Details ?? throw new NotFoundException($"Cannot find member with id {memberId}");
    }

    private async Task<IActionResult> SearchOrganisation(
        string consortiumId,
        string? phrase,
        int? page,
        string? selectedItem,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new SearchOrganisationQuery(phrase ?? string.Empty, new PaginationRequest(page ?? 0)), cancellationToken);
        if (result.Page.TotalItems == 0)
        {
            return RedirectToAction("SearchNoResults", "ConsortiumMember", new { consortiumId });
        }

        var model = new SearchOrganisationResultModel(
            consortiumId,
            phrase ?? string.Empty,
            $"search-result?phrase={phrase}",
            new PaginationResult<ExtendedSelectListItem>(
                result.Page.Items.Select(x => new ExtendedSelectListItem(
                        x.Name,
                        x.OrganisationId ?? x.CompanyHouseNumber ?? string.Empty,
                        false,
                        itemContent: new DynamicComponentViewModel(
                            nameof(OrganisationDetailsComponent),
                            new { street = x.Street, city = x.City, postalCode = x.PostalCode, providerCode = (string?)null })))
                    .ToList(),
                result.Page.CurrentPage,
                result.Page.ItemsPerPage,
                result.Page.TotalItems),
            selectedItem);

        return View("SearchResult", model);
    }
}
