using HE.Investments.Account.Shared.Authorization.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[Route("home")]
public class HomeController : Controller
{
    [Route("/")]
    [AuthorizeWithCompletedProfile]
    public IActionResult Index()
    {
        return RedirectToAction("Index", "Application");
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
