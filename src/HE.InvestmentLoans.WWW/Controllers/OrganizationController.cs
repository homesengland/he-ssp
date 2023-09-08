using HE.InvestmentLoans.Common.Utils.Constants;
using HE.InvestmentLoans.Contract.Organization;
using HE.InvestmentLoans.Contract.Organization.ValueObjects;
using HE.InvestmentLoans.WWW.Attributes;
using HE.InvestmentLoans.WWW.Utils.ValueObjects;
using HE.Investments.Organisation.CompaniesHouse;
using HE.Investments.Organisation.CompaniesHouse.Contract;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

[Route("organization")]
[AuthorizeWithoutLinkedOrganiztionOnly]
public class OrganizationController : Controller
{
    private readonly IMediator _mediator;
    private readonly ICompaniesHouseApi _companiesHouseApi;

    public OrganizationController(IMediator mediator, ICompaniesHouseApi companiesHouseApi)
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
    public IActionResult SearchOrganization(OrganizationViewModel organization)
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

    [HttpPost("select")]
    public async Task<IActionResult> SelectOrganization(OrganizationViewModel organization)
    {
        await _mediator.Send(new LinkContactWithOrganizationCommand(new CompaniesHouseNumber(organization.SelectedOrganization)));

        return RedirectToAction(nameof(HomeController.Dashboard), new ControllerName(nameof(HomeController)).WithoutPrefix());
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
}
