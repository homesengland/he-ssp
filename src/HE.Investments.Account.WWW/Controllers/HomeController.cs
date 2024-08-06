using HE.Investments.Common.User;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Account.WWW.Controllers;

[Route("home")]
public class HomeController : Controller
{
    private readonly IUserContext _userContext;

    public HomeController(IUserContext userContext)
    {
        _userContext = userContext;
    }

    [HttpGet("/")]
    public IActionResult Index()
    {
        if (_userContext.IsAuthenticated)
        {
            return RedirectToAction("List", "UserOrganisations");
        }

        return RedirectToAction("InformationAboutYourAccount", "User");
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

    [HttpGet("contact")]
    public IActionResult Contact()
    {
        return View();
    }
}
