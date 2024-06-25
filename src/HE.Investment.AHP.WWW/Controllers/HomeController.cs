using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.Authorization.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[Route("{organisationId}/home")]
[Route("home")]
public class HomeController : Controller
{
    private readonly IAccountUserContext _accountUserContext;

    public HomeController(IAccountUserContext accountUserContext)
    {
        _accountUserContext = accountUserContext;
    }

    [Route("/")]
    [AuthorizeWithCompletedProfile]
    public async Task<IActionResult> Index()
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var organisationId = userAccount.SelectedOrganisationId();
        return RedirectToAction("Index", "Application", new { organisationId });
    }

    [HttpGet("error")]
    public IActionResult Error()
    {
        return View();
    }

    [HttpGet("page-not-found")]
    public IActionResult PageNotFound()
    {
        return View();
    }
}
