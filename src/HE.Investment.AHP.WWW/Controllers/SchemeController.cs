using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Controllers;

// TODO: Add authorization attribute when implemented
[Route("scheme")]
public class SchemeController : Controller
{
    [HttpGet("start")]
    public IActionResult Index()
    {
        // TODO: Fetch site name when implemented
        var siteName = "Church road";
        return View("Index", siteName);
    }
}
