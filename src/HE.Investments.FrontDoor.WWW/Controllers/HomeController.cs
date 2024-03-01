using HE.Investments.Account.Shared.Authorization.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.FrontDoor.WWW.Controllers;

[Route("home")]
public class HomeController : Controller
{
    [Route("/")]
    [AuthorizeWithCompletedProfile]
    public IActionResult Index()
    {
        return View();
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
