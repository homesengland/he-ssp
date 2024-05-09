using HE.Investments.Account.WWW.Config;
using HE.Investments.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

namespace HE.Investments.Account.WWW.Controllers;

[Route("consortium")]
public class ConsortiumController : Controller
{
    private readonly AhpConfig _ahpConfig;

    private readonly IFeatureManager _featureManager;

    public ConsortiumController(AhpConfig ahpConfig, IFeatureManager featureManager)
    {
        _ahpConfig = ahpConfig;
        _featureManager = featureManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        if (await _featureManager.IsEnabledAsync(FeatureFlags.StayInCurrentApplication, cancellationToken))
        {
            return RedirectToAction("Index", "Home");
        }

        return new RedirectResult(_ahpConfig.ConsortiumManagementUrl);
    }
}
