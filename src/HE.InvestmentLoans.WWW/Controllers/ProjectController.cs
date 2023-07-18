using Microsoft.AspNetCore.Mvc;

namespace HE.InvestmentLoans.WWW.Controllers;
public class ProjectController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Add()
    {
        // loanApp = load
        // add (pass loaded loanApp)
        return View();
    }
}
