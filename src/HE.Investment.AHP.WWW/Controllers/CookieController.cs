using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

public class CookieController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
