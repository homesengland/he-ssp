using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return RedirectToAction("Index", "Scheme");
    }
}
