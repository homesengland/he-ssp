using HE.Investments.Account.Shared.Routing;
using HE.Investments.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

namespace HE.Investment.AHP.WWW.Controllers;

[Route("account")]
public class AccountController : Controller
{
    private readonly AccountConfig _accountConfig;

    private readonly IFeatureManager _featureManager;

    public AccountController(AccountConfig accountConfig, IFeatureManager featureManager)
    {
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
}
