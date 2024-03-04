using HE.Investments.Account.Shared.Authorization.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.FrontDoor.WWW.Controllers;

[AuthorizeWithCompletedProfile]
[Route("projects")]
public class ProjectsController : Controller
{
    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }
}
