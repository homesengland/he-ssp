using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.FrontDoor.WWW.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

namespace HE.Investments.FrontDoor.WWW.Controllers;

[Route("redirect-to-another-application")]
[AuthorizeWithCompletedProfile]
public class ConvertProjectController : Controller
{
    private readonly ProgrammeUrlConfig _programmeUrlConfig;

    private readonly IFeatureManager _featureManager;

    public ConvertProjectController(ProgrammeUrlConfig programmeUrl, IFeatureManager featureManager)
    {
        _programmeUrlConfig = programmeUrl;
        _featureManager = featureManager;
    }

    [Route("")]
    [HttpGet]
    public async Task<IActionResult> Redirect([FromQuery] string fdProjectId, [FromQuery] ApplicationType applicationType, CancellationToken cancellationToken)
    {
        if (await _featureManager.IsEnabledAsync(FeatureFlags.StayInCurrentApplication, cancellationToken))
        {
            return RedirectToAction("Index", "Projects");
        }

        var urlWithRouteData = applicationType switch
        {
            ApplicationType.Loans => $"{_programmeUrlConfig.Loans}?fdProjectId={fdProjectId}",
            ApplicationType.Ahp => $"{_programmeUrlConfig.Ahp}?fdProjectId={fdProjectId}",
            _ => string.Empty,
        };

        return Redirect(urlWithRouteData);
    }
}
