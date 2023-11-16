using HE.Investments.Account.Shared.Authorization.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Account.WWW.Controllers;

[Route("home")]
[AuthorizeWithCompletedProfile]
public class HomeController : Controller
{
    [HttpGet("/")]
    public IActionResult Index()
    {
        return RedirectToAction("Index", "UserOrganisation");
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
