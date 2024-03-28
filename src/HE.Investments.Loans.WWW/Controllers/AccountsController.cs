using HE.Investments.Account.Shared.Routing;
using HE.Investments.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

namespace HE.Investments.Loans.WWW.Controllers;

[AllowAnonymous]
[Route("accounts")]
public class AccountsController : Controller
{
    private readonly IAccountRoutes _accountRoutes;

    private readonly AccountConfig _accountConfig;

    private readonly IFeatureManager _featureManager;

    public AccountsController(IAccountRoutes accountRoutes, AccountConfig accountConfig, IFeatureManager featureManager)
    {
        _accountRoutes = accountRoutes;
        _accountConfig = accountConfig;
        _featureManager = featureManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        if (await _featureManager.IsEnabledAsync(FeatureFlags.StayInCurrentApplication, cancellationToken))
        {
            return RedirectToAction("Index", "Home");
        }

        return new RedirectResult(_accountConfig.Url);
    }

    [Route("/user-profile")]
    public IActionResult UserProfile(string callback)
    {
        return _accountRoutes.NotCompleteProfile("Loans", callback);
    }

    [Route("/organisation-dashboard")]
    public IActionResult OrganisationDashboard()
    {
        return _accountRoutes.LandingPageForLoggedUser();
    }
}
