using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Account.WWW.Controllers;

[Route("home")]
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
