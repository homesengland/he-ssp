using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;
public class GuidanceController : Controller
{
    [HttpGet("/guidance-what")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("/guidance-eligibility")]
    public IActionResult Eligibility()
    {
        return View();
    }
}
