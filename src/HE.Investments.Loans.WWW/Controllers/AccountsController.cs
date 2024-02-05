using HE.Investments.Account.Shared.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Loans.WWW.Controllers;

[AllowAnonymous]
[Route("accounts")]
public class AccountsController : Controller
{
    private readonly IAccountRoutes _accountRoutes;

    public AccountsController(IAccountRoutes accountRoutes)
    {
        _accountRoutes = accountRoutes;
    }

    [Route("/user-profile")]
    public IActionResult UserProfile(string callback)
    {
        // TODO: verify
        return _accountRoutes.NotCompleteProfile("Loans", callback);
    }

    [Route("/organisation-dashboard")]
    public IActionResult OrganisationDashboard()
    {
        return _accountRoutes.LandingPageForLoggedUser();
    }
}
