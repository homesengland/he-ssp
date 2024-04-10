using HE.Investments.Account.Contract.Organisation;
using HE.Investments.Account.Contract.Organisation.Commands;
using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Account.Shared.Routing;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.WWW.Controllers;
using HE.Investments.Common.WWW.Models;
using HE.Investments.Common.WWW.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Account.WWW.Controllers;

[Route(OrganisationAccountEndpoints.Controller)]
[AuthorizeWithoutLinkedOrganisationOnly]
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
    public async Task<IActionResult> ConfirmOrganisationPost(string organisationNumberOrId, ConfirmModel<OrganisationBasicDetails> model, [FromQuery] string searchPhrase)
    {
        if (model.IsConfirmed.IsNotProvided())
        {
            ModelState.AddModelError(nameof(model.IsConfirmed), ValidationErrorMessage.ChooseYourAnswer);
            model.ViewModel = await _mediator.Send(new GetOrganisationQuery(organisationNumberOrId));
            model.SearchPhrase = searchPhrase;
            return View("ConfirmYourSelection", model);
        }

        if (!model.IsConfirmed!.Value)
        {
            return RedirectToAction("SearchOrganisationResult", "Organisation", new { searchPhrase });
        }

        await _mediator.Send(new LinkContactWithOrganisationCommand(organisationNumberOrId));
        return RedirectToAction("UserOrganisationsList", "UserOrganisation");
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
                nameof(UserOrganisationController.UserOrganisationsList),
                new ControllerName(nameof(UserOrganisationController)).WithoutPrefix())),
            onError: () => Task.FromResult<IActionResult>(View(model)),
            cancellationToken);
    }
}
