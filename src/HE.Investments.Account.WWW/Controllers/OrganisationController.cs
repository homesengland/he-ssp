using HE.Investments.Account.Contract.Organisation;
using HE.Investments.Account.Contract.Organisation.Commands;
using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Account.Shared.Routing;
using HE.Investments.Common.Contract.Constants;
using HE.Investments.Common.Contract.Pagination;
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
    public IActionResult SearchOrganisation(OrganisationSearchModel organisation)
    {
        return RedirectToAction(nameof(SearchOrganisationResult), new { searchPhrase = organisation.Name });
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
    public async Task<IActionResult> ConfirmOrganisation(string organisationNumberOrId)
    {
        var response = await _mediator.Send(new GetOrganisationQuery(organisationNumberOrId));

        return View("ConfirmYourSelection", new ConfirmModel<OrganisationBasicDetails> { ViewModel = response });
    }

    [HttpPost("{organisationNumberOrId}/confirm")]
    public async Task<IActionResult> ConfirmOrganisationPost(string organisationNumberOrId, ConfirmModel<OrganisationBasicDetails> model)
    {
        if (string.IsNullOrEmpty(model.Response))
        {
            ModelState.AddModelError(nameof(model.Response), ValidationErrorMessage.ChooseYourAnswer);
            model.ViewModel = await _mediator.Send(new GetOrganisationQuery(organisationNumberOrId));
            return View("ConfirmYourSelection", model);
        }

        if (model.Response == CommonResponse.Yes)
        {
            await _mediator.Send(new LinkContactWithOrganisationCommand(organisationNumberOrId));
            return RedirectToAction(null);
        }

        return RedirectToAction(nameof(SearchOrganisation));
    }

    [HttpGet("no-match-found")]
    public IActionResult NoMatchFound()
    {
        return View();
    }

    [HttpGet("create")]
    public IActionResult CreateOrganisation()
    {
        return View(new OrganisationDetailsViewModel());
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateOrganisation(OrganisationDetailsViewModel viewModel, CancellationToken cancellationToken)
    {
        var command = new CreateAndLinkOrganisationCommand(
            viewModel.Name,
            viewModel.AddressLine1,
            viewModel.AddressLine2,
            viewModel.TownOrCity,
            viewModel.County,
            viewModel.Postcode);

        return await this.ExecuteCommand<OrganisationDetailsViewModel>(
            _mediator,
            command,
            onSuccess: () => Task.FromResult<IActionResult>(RedirectToAction(
                nameof(UserOrganisationController.Index),
                new ControllerName(nameof(UserOrganisationController)).WithoutPrefix())),
            onError: () => Task.FromResult<IActionResult>(View(viewModel)),
            cancellationToken);
    }
}
