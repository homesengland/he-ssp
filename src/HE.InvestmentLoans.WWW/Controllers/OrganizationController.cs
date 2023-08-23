using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.Investments.Organisation.CompaniesHouse;
using HE.Investments.Organisation.CompaniesHouse.Contract;
using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

[Route("organization")]
public class OrganizationController : Controller
{
    private readonly ICompaniesHouseApi _companiesHouseApi;

    public OrganizationController(ICompaniesHouseApi companiesHouseApi)
    {
        _companiesHouseApi = companiesHouseApi;
    }

    [HttpGet("search")]
    public IActionResult SearchOrganization()
    {
        return View();
    }

    [HttpPost("select")]
    public IActionResult SelectOrganization()
    {
        var model = new OrganizationViewModel();
        return View(model);
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
