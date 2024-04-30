using HE.Investment.AHP.WWW.Models.ConsortiumMember;
using HE.Investment.AHP.WWW.Views.Shared.Components.OrganisationDetailsComponent;
using HE.Investment.AHP.WWW.Workflows;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.AHP.Consortium.Contract;
using HE.Investments.AHP.Consortium.Contract.Commands;
using HE.Investments.AHP.Consortium.Contract.Enums;
using HE.Investments.AHP.Consortium.Contract.Queries;
using HE.Investments.Common.Contract.Exceptions;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Contract.Validators;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Components;
using HE.Investments.Common.WWW.Controllers;
using HE.Investments.Common.WWW.Models;
using HE.Investments.Common.WWW.Routing;
using HE.Investments.Organisation.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrganisationId = HE.Investments.Account.Shared.User.ValueObjects.OrganisationId;

namespace HE.Investment.AHP.WWW.Controllers;

[Route("consortium-member/{consortiumId}")]
[AuthorizeWithCompletedProfile]
public class ConsortiumMemberController : WorkflowController<ConsortiumMemberWorkflowState>
{
    private readonly IMediator _mediator;

    public ConsortiumMemberController(IMediator mediator)
    {
        _mediator = mediator;
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
        return View(await GetConsortiumDetails(consortiumId, fetchAddress: false, cancellationToken));
    }

    [HttpGet("search-organisation")]
    [WorkflowState(ConsortiumMemberWorkflowState.SearchOrganisation)]
    public IActionResult SearchOrganisation()
    {
        return View();
    }

    [HttpPost("search-organisation")]
    [WorkflowState(ConsortiumMemberWorkflowState.SearchOrganisation)]
    public async Task<IActionResult> SearchOrganisation(string consortiumId, string? phrase, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<ProvideSearchOrganisationPhraseCommand>(
            _mediator,
            new ProvideSearchOrganisationPhraseCommand(new ConsortiumId(consortiumId), phrase),
            async () => await Continue(new { consortiumId, phrase }),
            () => Task.FromResult<IActionResult>(View(phrase)),
            cancellationToken);
    }

    [HttpGet("search-result")]
    [WorkflowState(ConsortiumMemberWorkflowState.SearchResult)]
    public async Task<IActionResult> SearchResult(string consortiumId, string? phrase, int? page, CancellationToken cancellationToken)
    {
        return await SearchOrganisation(consortiumId, phrase, page, null, cancellationToken);
    }

    [HttpPost("search-result")]
    [WorkflowState(ConsortiumMemberWorkflowState.SearchResult)]
    public async Task<IActionResult> SearchResult(string consortiumId, SearchOrganisationResultModel model, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<SearchOrganisationResultModel>(
            _mediator,
            new AddOrganisationToConsortiumCommand(new ConsortiumId(consortiumId), model.SelectedMember.IsProvided() ? new OrganisationIdentifier(model.SelectedMember!) : null),
            async () => await Continue(new { consortiumId }),
            async () => await SearchOrganisation(consortiumId, model.Phrase, model.Page.CurrentPage, model.SelectedMember, cancellationToken),
            cancellationToken);
    }

    [HttpGet("search-no-results")]
    [WorkflowState(ConsortiumMemberWorkflowState.SearchNoResults)]
    public IActionResult SearchNoResults()
    {
        return View();
    }

    [HttpGet("add-organisation")]
    [WorkflowState(ConsortiumMemberWorkflowState.AddOrganisation)]
    public IActionResult AddOrganisation()
    {
        return View(new AddOrganisationModel(null, null, null, null, null, null));
    }

    [HttpPost("add-organisation")]
    [WorkflowState(ConsortiumMemberWorkflowState.AddOrganisation)]
    public async Task<IActionResult> AddOrganisation(string consortiumId, AddOrganisationModel model, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<AddOrganisationModel>(
            _mediator,
            new AddManualOrganisationToConsortiumCommand(
                new ConsortiumId(consortiumId),
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
    public async Task<IActionResult> AddMembers(string consortiumId, CancellationToken cancellationToken)
    {
        return View(await GetConsortiumDetails(consortiumId, fetchAddress: true, cancellationToken));
    }

    [HttpPost("add-members")]
    [WorkflowState(ConsortiumMemberWorkflowState.AddMembers)]
    public async Task<IActionResult> AddMembers(string consortiumId, [FromForm] AreAllMembersAdded areAllMembersAdded, CancellationToken cancellationToken)
    {
        if (areAllMembersAdded == AreAllMembersAdded.Yes)
        {
            return RedirectToAction("Index", new { consortiumId });
        }

        if (areAllMembersAdded == AreAllMembersAdded.No)
        {
            return RedirectToAction("SearchOrganisation", new { consortiumId });
        }

        this.AddOrderedErrors<string>(new OperationResult().AddValidationError(
            nameof(AreAllMembersAdded),
            "Select whether you have you added all members to this consortium"));
        return View(await _mediator.Send(new GetConsortiumDetailsQuery(new ConsortiumId(consortiumId), FetchAddress: true), cancellationToken));
    }

    [HttpGet("remove-member/{memberId}")]
    [WorkflowState(ConsortiumMemberWorkflowState.RemoveMember)]
    public async Task<IActionResult> RemoveMember(string consortiumId, string memberId, CancellationToken cancellationToken)
    {
        return View(await GetConsortiumMemberDetails(consortiumId, memberId, fetchAddress: false, cancellationToken));
    }

    [HttpPost("remove-member/{memberId}")]
    [WorkflowState(ConsortiumMemberWorkflowState.RemoveMember)]
    public async Task<IActionResult> RemoveMember(string consortiumId, string memberId, [FromForm] bool? isConfirmed, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<OrganisationDetails>(
            _mediator,
            new RemoveOrganisationFromConsortiumCommand(new ConsortiumId(consortiumId), new OrganisationId(memberId), isConfirmed),
            async () => await Continue(new { consortiumId }),
            async () => View("RemoveMember", await GetConsortiumMemberDetails(consortiumId, memberId, fetchAddress: false, cancellationToken)),
            cancellationToken);
    }

    protected override async Task<IStateRouting<ConsortiumMemberWorkflowState>> Routing(ConsortiumMemberWorkflowState currentState, object? routeData = null)
    {
        return await Task.FromResult<IStateRouting<ConsortiumMemberWorkflowState>>(new ConsortiumMemberWorkflow(currentState));
    }

    private async Task<ConsortiumDetails> GetConsortiumDetails(string consortiumId, bool fetchAddress, CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetConsortiumDetailsQuery(new ConsortiumId(consortiumId), fetchAddress), cancellationToken);
    }

    private async Task<OrganisationDetails> GetConsortiumMemberDetails(string consortiumId, string memberId, bool fetchAddress, CancellationToken cancellationToken)
    {
        var consortium = await _mediator.Send(new GetConsortiumDetailsQuery(new ConsortiumId(consortiumId), fetchAddress), cancellationToken);
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
