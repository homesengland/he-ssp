using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

public class CookieController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
