using HE.InvestmentLoans.BusinessLogic.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

[Route("organization")]
public class OrganizationController : Controller
{
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
}
