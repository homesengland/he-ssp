using HE.Investments.Account.Shared.Authorization.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

[AuthorizeWithCompletedProfile]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return RedirectToAction("Index", "Application");
    }
}
