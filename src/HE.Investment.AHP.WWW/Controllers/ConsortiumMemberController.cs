using HE.Investment.AHP.Contract.AhpProgramme;
using HE.Investment.AHP.WWW.Models.Common;
using HE.Investment.AHP.WWW.Models.Consortium;
using HE.Investment.AHP.WWW.Models.ConsortiumMember;
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
using HE.Investments.Common.WWW.Controllers;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.Consortium.Shared.UserContext;
using HE.Investments.Organisation.Contract;
using HE.Investments.Organisation.Contract.Queries;
using HE.Investments.Organisation.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[Route("{organisationId}/consortium-member/{consortiumId}")]
[AuthorizeWithCompletedProfile(ConsortiumAccessContext.ViewConsortium)]
public class ConsortiumMemberController : WorkflowController<ConsortiumMemberWorkflowState>
{
    private readonly IMediator _mediator;

    private readonly IConsortiumUserContext _consortiumUserContext;

    public ConsortiumMemberController(IMediator mediator, IConsortiumUserContext consortiumUserContext)
    {
        _mediator = mediator;
        _consortiumUserContext = consortiumUserContext;
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
        var userAccount = await _consortiumUserContext.GetSelectedAccount();
        var consortium = await GetConsortiumDetails(consortiumId, fetchAddress: false, cancellationToken);
        var model = new ManageConsortiumModel(consortium, userAccount.Organisation!.RegisteredCompanyName, userAccount.CanManageConsortium);

        return consortium.LeadPartner.OrganisationId != userAccount.SelectedOrganisationId()
            ? View("MemberDashboard", model)
            : View(model);
    }

    [HttpGet("search-organisation")]
    [WorkflowState(ConsortiumMemberWorkflowState.SearchOrganisation)]
    [AuthorizeWithCompletedProfile(ConsortiumAccessContext.ManageConsortium)]
    public IActionResult SearchOrganisation()
    {
        return View();
    }

    [HttpPost("search-organisation")]
    [WorkflowState(ConsortiumMemberWorkflowState.SearchOrganisation)]
    [AuthorizeWithCompletedProfile(ConsortiumAccessContext.ManageConsortium)]
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
    [AuthorizeWithCompletedProfile(ConsortiumAccessContext.ManageConsortium)]
    public async Task<IActionResult> SearchResult(string consortiumId, string? phrase, int? page, CancellationToken cancellationToken)
    {
        return await SearchOrganisation(consortiumId, phrase, page, null, cancellationToken);
    }

    [HttpPost("search-result")]
    [WorkflowState(ConsortiumMemberWorkflowState.SearchResult)]
    [AuthorizeWithCompletedProfile(ConsortiumAccessContext.ManageConsortium)]
    public async Task<IActionResult> SearchResult(string consortiumId, SearchOrganisationResultModel model, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<SearchOrganisationResultModel>(
            _mediator,
            new AddOrganisationToConsortiumCommand(
                ConsortiumId.From(consortiumId),
                model.SelectedMember.IsProvided() ? new OrganisationIdentifier(model.SelectedMember) : null),
            async () => await Continue(new { consortiumId }),
            async () => await SearchOrganisation(consortiumId, model.Phrase, model.Result.CurrentPage, model.SelectedMember, cancellationToken),
            cancellationToken);
    }

    [HttpGet("search-no-results")]
    [WorkflowState(ConsortiumMemberWorkflowState.SearchNoResults)]
    [AuthorizeWithCompletedProfile(ConsortiumAccessContext.ManageConsortium)]
    public IActionResult SearchNoResults()
    {
        return View();
    }

    [HttpGet("add-organisation")]
    [WorkflowState(ConsortiumMemberWorkflowState.AddOrganisation)]
    [AuthorizeWithCompletedProfile(ConsortiumAccessContext.ManageConsortium)]
    public IActionResult AddOrganisation()
    {
        return View(new AddOrganisationModel(null, null, null, null, null, null));
    }

    [HttpPost("add-organisation")]
    [WorkflowState(ConsortiumMemberWorkflowState.AddOrganisation)]
    [AuthorizeWithCompletedProfile(ConsortiumAccessContext.ManageConsortium)]
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
    [AuthorizeWithCompletedProfile(ConsortiumAccessContext.ManageConsortium)]
    public async Task<IActionResult> AddMembers(string consortiumId, CancellationToken cancellationToken)
    {
        return View(await GetConsortiumDetails(consortiumId, fetchAddress: true, cancellationToken));
    }

    [HttpPost("add-members")]
    [WorkflowState(ConsortiumMemberWorkflowState.AddMembers)]
    [AuthorizeWithCompletedProfile(ConsortiumAccessContext.ManageConsortium)]
    public async Task<IActionResult> AddMembers(string consortiumId, [FromForm] AreAllMembersAdded areAllMembersAdded, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<ConsortiumDetails>(
            _mediator,
            new AddMembersCommand(ConsortiumId.From(consortiumId), areAllMembersAdded),
            () => Task.FromResult<IActionResult>(areAllMembersAdded == AreAllMembersAdded.Yes
                ? this.OrganisationRedirectToAction("Index", routeValues: new { consortiumId })
                : this.OrganisationRedirectToAction("SearchOrganisation", routeValues: new { consortiumId })),
            async () => View(await _mediator.Send(new GetConsortiumDetailsQuery(ConsortiumId.From(consortiumId), FetchAddress: true), cancellationToken)),
            cancellationToken);
    }

    [HttpGet("remove-member/{memberId}")]
    [WorkflowState(ConsortiumMemberWorkflowState.RemoveMember)]
    [AuthorizeWithCompletedProfile(ConsortiumAccessContext.ManageConsortium)]
    public async Task<IActionResult> RemoveMember(string consortiumId, string memberId, CancellationToken cancellationToken)
    {
        return View(await GetConsortiumMemberDetails(consortiumId, memberId, fetchAddress: false, cancellationToken));
    }

    [HttpPost("remove-member/{memberId}")]
    [WorkflowState(ConsortiumMemberWorkflowState.RemoveMember)]
    [AuthorizeWithCompletedProfile(ConsortiumAccessContext.ManageConsortium)]
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
    [WorkflowState(ConsortiumMemberWorkflowState.ContactHomesEngland)]
    [AuthorizeWithCompletedProfile(ConsortiumAccessContext.ViewConsortium)]
    public async Task<IActionResult> ContactHomesEngland(string consortiumId, CancellationToken cancellationToken)
    {
        var ahpProgramme = await _mediator.Send(new GetTheAhpProgrammeQuery(), cancellationToken);

        return View(new ConsortiumSelectedProgrammeModel(consortiumId, ahpProgramme));
    }

    protected override async Task<IStateRouting<ConsortiumMemberWorkflowState>> Routing(ConsortiumMemberWorkflowState currentState, object? routeData = null)
    {
        var consortiumId = Request.GetRouteValue("consortiumId")
                           ?? routeData?.GetPropertyValue<string>("consortiumId")
                           ?? string.Empty;
        var consortium = await GetConsortiumDetails(consortiumId, false, CancellationToken.None);
        var userAccount = await _consortiumUserContext.GetSelectedAccount();

        return new ConsortiumMemberWorkflow(consortium, userAccount.Organisation?.OrganisationId, currentState);
    }

    private async Task<ConsortiumDetails> GetConsortiumDetails(string consortiumId, bool fetchAddress, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetConsortiumDetailsQuery(ConsortiumId.From(consortiumId), fetchAddress), cancellationToken);
    }

    private async Task<OrganisationDetails> GetConsortiumMemberDetails(
        string consortiumId,
        string memberId,
        bool fetchAddress,
        CancellationToken cancellationToken)
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
            return this.OrganisationRedirectToAction("SearchNoResults", "ConsortiumMember", new { consortiumId });
        }

        return View(
            "SearchResult",
            new SearchOrganisationResultModel(phrase ?? string.Empty, $"search-result?phrase={phrase}", result.Page, selectedItem ?? string.Empty));
    }
}
