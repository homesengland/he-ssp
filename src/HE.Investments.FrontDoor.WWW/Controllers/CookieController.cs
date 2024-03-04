using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.FrontDoor.WWW.Controllers;

public class CookieController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
