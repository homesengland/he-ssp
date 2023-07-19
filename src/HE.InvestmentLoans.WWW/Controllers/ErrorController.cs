using HE.InvestmentLoans.Common.Models.Error;
using HE.InvestmentLoans.Common.Services.Interfaces;
using HE.InvestmentLoans.WWW.Config;
using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

public class ErrorController : Controller
{
    private readonly ICacheService _cacheService;

    public ErrorController(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    [Route(RouteConstants.ErrorPageNotFound)]
    public IActionResult PageNotFound(string key)
    {
        var data = _cacheService.GetValue<ErrorModel>($"{RouteConstants.ErrorPageNotFound}?key={key}");

        return View(data);
    }

    [Route(RouteConstants.ErrorProblemWithTheService)]
    public IActionResult ProblemWithTheService(string key)
    {
        var data = _cacheService.GetValue<ErrorModel>($"{RouteConstants.ErrorProblemWithTheService}?key={key}");

        return View(data);
    }
}
