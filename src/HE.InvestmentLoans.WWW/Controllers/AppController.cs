using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;

[Route("app")]
public class AppController : Controller
{
    private readonly IHostEnvironment _hostEnvironment;

    public AppController(IHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
    }

    [HttpGet("env")]
    public string Env()
    {
        return _hostEnvironment.EnvironmentName;
    }
}
