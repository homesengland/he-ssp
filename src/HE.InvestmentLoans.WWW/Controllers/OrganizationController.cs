using HE.InvestmentLoans.Contract.Organization;
using HE.Investments.Organisation.CompaniesHouse;
using HE.Investments.Organisation.CompaniesHouse.Contract;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

[Route("organization")]
public class OrganizationController : Controller
{
    private readonly ICompaniesHouseApi _companiesHouseApi;
    private readonly IMediator _mediator;

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
    public IActionResult SearchOrganizationPost(OrganizationViewModel organization)
    {
        return RedirectToAction(nameof(SelectOrganization), new { searchPhrase = organization.Name });
    }

    [HttpGet("select")]
    public async Task<IActionResult> SelectOrganization([FromQuery] string searchPhrase, [FromQuery] int page)
    {
        var response = await _mediator.Send(new SearchOrganizationsQuery(searchPhrase, page, 10));

        return View(response.Result);
    }

    [HttpPost("select")]
    public IActionResult SelectOrganizationPost(OrganizationViewModel organization)
    {
        return View();
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
