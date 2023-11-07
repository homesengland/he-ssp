using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Common.Utils.Constants.FormOption;
using HE.InvestmentLoans.Contract.Organization;
using HE.InvestmentLoans.Contract.Organization.Commands;
using HE.InvestmentLoans.Contract.Organization.ValueObjects;
using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Common.WWW.Controllers;
using HE.Investments.Common.WWW.Models;
using HE.Investments.Common.WWW.Utils;
using HE.Investments.Organisation.CompaniesHouse.Contract;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Account.WWW.Controllers;

[Route("organisation")]
public class OrganisationController : Controller
{
    private readonly IMediator _mediator;

    public OrganisationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("search")]
    public IActionResult SearchOrganization()
    {
        return View();
    }

    [HttpPost("search")]
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
            await _mediator.Send(new LinkContactWithOrganizationCommand(new CompaniesHouseNumber(organizationNumberOrId)));
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
            onSuccess: () => RedirectToAction(
                nameof(UserOrganisationController.Index),
                new ControllerName(nameof(UserOrganisationController)).WithoutPrefix()),
            onError: () => View(viewModel),
            cancellationToken);
    }
}
