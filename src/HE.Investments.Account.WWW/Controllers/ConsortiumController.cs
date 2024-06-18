using HE.Investments.Account.WWW.Routing;
using HE.Investments.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

namespace HE.Investments.Account.WWW.Controllers;

[Route("{organisationId}/consortium")]
public class ConsortiumController : Controller
{
    private readonly ProgrammeUrlConfig _programmeUrlConfig;

    private readonly IFeatureManager _featureManager;

    public ConsortiumController(ProgrammeUrlConfig programmeUrlConfig, IFeatureManager featureManager)
    {
        _programmeUrlConfig = programmeUrlConfig;
        _featureManager = featureManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        if (await _featureManager.IsEnabledAsync(FeatureFlags.StayInCurrentApplication, cancellationToken))
        {
            return RedirectToAction("Index", "Home");
        }

        return new RedirectResult($"{_programmeUrlConfig.Ahp}/consortium");
    }
}
