using HE.Investments.Account.Contract.Organisation;
using HE.Investments.Account.Contract.Organisation.Commands;
using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Account.Shared.Routing;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Controllers;
using HE.Investments.Common.WWW.Models;
using HE.Investments.Common.WWW.Utils;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Account.WWW.Controllers;

[Route(OrganisationAccountEndpoints.Controller)]
[Authorize]
public class OrganisationController : Controller
{
    private readonly IMediator _mediator;

    public OrganisationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet(OrganisationAccountEndpoints.SearchOrganisationSuffix)]
    public IActionResult SearchOrganisation()
    {
        return View();
    }

    [HttpPost(OrganisationAccountEndpoints.SearchOrganisationSuffix)]
    public async Task<IActionResult> SearchOrganisation(OrganisationSearchModel model, CancellationToken cancellationToken)
    {
        return await this.ExecuteCommand<OrganisationSearchModel>(
            _mediator,
            new ProvideOrganisationSearchPhraseCommand(model.Name),
            async () => await Task.FromResult(RedirectToAction(nameof(SearchOrganisationResult), new { searchPhrase = model.Name })),
            () => Task.FromResult<IActionResult>(View("SearchOrganisation", model)),
            cancellationToken);
    }

    [HttpGet("search/result")]
    public async Task<IActionResult> SearchOrganisationResult([FromQuery] string searchPhrase, [FromQuery] int page)
    {
        var response = await _mediator.Send(new SearchOrganisationsQuery(searchPhrase, page, DefaultPagination.PageSize));

        if (response.Result.TotalOrganisations == 0)
        {
            return RedirectToAction(nameof(NoMatchFound));
        }

        return View(response.Result);
    }

    [HttpGet("{organisationNumberOrId}/confirm")]
    public async Task<IActionResult> ConfirmOrganisation(string organisationNumberOrId, [FromQuery] string searchPhrase)
    {
        var response = await _mediator.Send(new GetOrganisationQuery(organisationNumberOrId));

        return View("ConfirmYourSelection", new ConfirmModel<OrganisationBasicDetails> { ViewModel = response, SearchPhrase = searchPhrase });
    }

    [HttpPost("{organisationNumberOrId}/confirm")]
    public async Task<IActionResult> ConfirmOrganisationPost(string organisationNumberOrId, ConfirmModel<OrganisationBasicDetails> model, CancellationToken cancellationToken)
    {
        if (model.IsConfirmed.IsProvided() && !model.IsConfirmed!.Value)
        {
            return RedirectToAction("SearchOrganisationResult", "Organisation", new { model.SearchPhrase });
        }

        return await this.ExecuteCommand<ConfirmModel<OrganisationBasicDetails>>(
            _mediator,
            new LinkContactWithOrganisationCommand(organisationNumberOrId, model.IsConfirmed),
            async () => await Task.FromResult(RedirectToAction("List", "UserOrganisations")),
            async () => await Task.FromResult(View("ConfirmYourSelection", model)),
            cancellationToken);
    }

    [HttpGet("no-match-found")]
    public IActionResult NoMatchFound()
    {
        return View();
    }

    [HttpGet("create")]
    public IActionResult CreateOrganisation()
    {
        return View(new OrganisationDetails());
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateOrganisation(OrganisationDetails model, CancellationToken cancellationToken)
    {
        var command = new CreateAndLinkOrganisationCommand(
            model.Name,
            model.AddressLine1,
            model.AddressLine2,
            model.TownOrCity,
            model.County,
            model.Postcode);

        return await this.ExecuteCommand<OrganisationDetails>(
            _mediator,
            command,
            onSuccess: () => Task.FromResult<IActionResult>(RedirectToAction(
                nameof(UserOrganisationsController.List),
                new ControllerName(nameof(UserOrganisationsController)).WithoutPrefix())),
            onError: () => Task.FromResult<IActionResult>(View(model)),
            cancellationToken);
    }
}
