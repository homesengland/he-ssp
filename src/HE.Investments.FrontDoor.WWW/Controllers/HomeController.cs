using HE.Investments.Account.Shared;
using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.FrontDoor.Domain.Project.Repository;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.FrontDoor.WWW.Controllers;

[Route("home")]
public class HomeController : Controller
{
    private readonly IProjectRepository _projectRepository;

    private readonly IAccountUserContext _accountUserContext;

    public HomeController(IProjectRepository projectRepository, IAccountUserContext accountUserContext)
    {
        _projectRepository = projectRepository;
        _accountUserContext = accountUserContext;
    }

    [Route("/")]
    [AuthorizeWithCompletedProfile]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var userAccount = await _accountUserContext.GetSelectedAccount();
        var projects = await _projectRepository.GetProjects(userAccount, cancellationToken);
        return View(projects);
    }

    [HttpGet("error")]
    public IActionResult Error()
    {
        return View();
    }

    [HttpGet("page-not-found")]
    public IActionResult PageNotFound()
    {
        return View();
    }
}
