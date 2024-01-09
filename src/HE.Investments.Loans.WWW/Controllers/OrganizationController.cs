using HE.Investments.Account.Contract.Organisation;
using HE.Investments.Account.Contract.Organisation.Commands;
using HE.Investments.Account.Contract.Organisation.Queries;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Messages;
using HE.Investments.Common.WWW.Models;
using HE.Investments.Common.WWW.TagHelpers.Pagination;
using HE.Investments.Common.WWW.Utils;
using HE.Investments.Loans.Common.Utils.Constants;
using HE.Investments.Loans.Common.Utils.Constants.FormOption;
using HE.Investments.Organisation.CompaniesHouse;
using HE.Investments.Organisation.CompaniesHouse.Contract;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Loans.WWW.Controllers;

[Route("organization")]
[AuthorizeWithoutLinkedOrganisationOnly]
public class OrganizationController : BaseController
{
    private readonly IMediator _mediator;
    private readonly ICompaniesHouseApi _companiesHouseApi;

    public OrganizationController(IMediator mediator, ICompaniesHouseApi companiesHouseApi)
    : base(mediator)
    {
        _mediator = mediator;
        _companiesHouseApi = companiesHouseApi;
    }

    [HttpGet("search")]
    public IActionResult SearchOrganization()
    {
        return View();
    }

    [HttpPost("search")]
    public IActionResult SearchOrganization(OrganisationSearchModel organization)
    {
        return RedirectToAction(nameof(SearchOrganizationResult), new { searchPhrase = organization.Name });
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
            return RedirectToAction(nameof(HomeController.Dashboard), new ControllerName(nameof(HomeController)).WithoutPrefix());
        }

        return RedirectToAction(nameof(SearchOrganization));
    }

    [HttpGet("no-match-found")]
    public IActionResult NoMatchFound()
    {
        return View();
    }

    [HttpGet("dev/search")]
    public async Task<CompaniesHouseSearchResult> SearchOrganizationDemo(string companyName, CancellationToken cancellationToken = default)
    {
        return await _companiesHouseApi.Search(companyName, null, cancellationToken);
    }

    [HttpGet("dev/search-by-company-name")]
    public async Task<CompaniesHouseGetByCompanyNumberResult> SearchOrganizationCompanyDemo(string companyNumber, CancellationToken cancellationToken = default)
    {
        return await _companiesHouseApi.GetByCompanyNumber(companyNumber, cancellationToken);
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

        return await ExecuteCommand(
            command,
            () => RedirectToAction(
                nameof(UserOrganisationController.Index),
                new ControllerName(nameof(UserOrganisationController)).WithoutPrefix()),
            () => View(viewModel),
            cancellationToken);
    }
}
