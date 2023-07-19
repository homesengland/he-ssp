using HE.InvestmentLoans.WWW.Config;
using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

public class ErrorController : Controller
{
    [Route(RouteConstants.ErrorPageNotFound)]
    public IActionResult PageNotFound()
    {
        return View();
    }

    [Route(RouteConstants.ErrorServiceUnavailable)]
    public IActionResult ServiceUnavailable()
    {
        return View();
    }

    [Route(RouteConstants.ErrorProblemWithTheService)]
    public IActionResult ProblemWithTheService()
    {
        return View();
    }
}
