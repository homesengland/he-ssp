using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Loans.WWW.Controllers;

public class CookieController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
