using HE.Investments.Account.Contract.Organisation;
using HE.Investments.Account.Contract.Organisation.Commands;
using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Account.Shared.Routing;
using HE.Investments.Common.Messages;
using HE.Investments.Common.Utils.Pagination;
using HE.Investments.Common.WWW.Controllers;
using HE.Investments.Common.WWW.Models;
using HE.Investments.Common.WWW.Utils;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
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

    [HttpGet(OrganisationAccountEndpoints.SearchOrganizationSuffix)]
    public IActionResult SearchOrganization()
    {
        return View();
    }

    [HttpPost(OrganisationAccountEndpoints.SearchOrganizationSuffix)]
    public IActionResult SearchOrganization(OrganisationSearchModel organisation)
    {
        return RedirectToAction(nameof(SearchOrganizationResult), new { searchPhrase = organisation.Name });
    }

    [HttpGet("search/result")]
    public async Task<IActionResult> SearchOrganizationResult([FromQuery] string searchPhrase, [FromQuery] int page)
    {
        var response = await _mediator.Send(new SearchOrganizationsQuery(searchPhrase, page, DefaultPagination.PageSize));

        if (response.Result.TotalOrganizations == 0)
        {
            return RedirectToAction(nameof(NoMatchFound));
        }

        return View(response.Result);
    }

    [HttpGet("{organizationNumberOrId}/confirm")]
    public async Task<IActionResult> ConfirmOrganization(string organizationNumberOrId)
    {
        var response = await _mediator.Send(new GetOrganizationQuery(organizationNumberOrId));

        return View("ConfirmYourSelection", new ConfirmModel<OrganizationBasicDetails> { ViewModel = response });
    }

    [HttpPost("{organizationNumberOrId}/confirm")]
    public async Task<IActionResult> ConfirmOrganizationPost(string organizationNumberOrId, ConfirmModel<OrganizationBasicDetails> model)
    {
        if (string.IsNullOrEmpty(model.Response))
        {
            ModelState.AddModelError(nameof(model.Response), ValidationErrorMessage.ChooseYourAnswer);
            model.ViewModel = await _mediator.Send(new GetOrganizationQuery(organizationNumberOrId));
            return View("ConfirmYourSelection", model);
        }

        if (model.Response == CommonResponse.Yes)
        {
            await _mediator.Send(new LinkContactWithOrganizationCommand(organizationNumberOrId));
            return RedirectToAction(null);
        }

        return RedirectToAction(nameof(SearchOrganization));
    }

    [HttpGet("no-match-found")]
    public IActionResult NoMatchFound()
    {
        return View();
    }

    [HttpGet("create")]
    public IActionResult CreateOrganisation()
    {
        return View();
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

        return await this.ExecuteCommand(
            _mediator,
            command,
            onSuccess: () => Task.FromResult<IActionResult>(RedirectToAction(
                nameof(UserOrganisationController.Index),
                new ControllerName(nameof(UserOrganisationController)).WithoutPrefix())),
            onError: () => Task.FromResult<IActionResult>(View(viewModel)),
            cancellationToken);
    }
}
