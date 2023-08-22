using HE.InvestmentLoans.BusinessLogic.ViewModel;
using HE.Investments.Organisation.Contract;
using HE.Investments.Organisation.Services;
using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

[Route("organization")]
public class OrganizationController : Controller
{
    private readonly IOrganisationSearchService _organisationSearchService;

    public OrganizationController(IOrganisationSearchService organisationSearchService)
    {
        _organisationSearchService = organisationSearchService;
    }

    [HttpGet("search")]
    public IActionResult SearchOrganization()
    {
        return View();
    }

    [HttpGet("search-demo")]
    public async Task<OrganisationSearchResult> SearchOrganizationDemo()
    {
        return await _organisationSearchService.Search("pwc");
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
}
