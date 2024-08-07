using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.FrontDoor.WWW.Config;
using HE.UtilsService.BannerNotification.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

namespace HE.Investments.FrontDoor.WWW.Controllers;

[Route("{organisationId}/redirect-to-another-application")]
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
        var urlWithRouteData = applicationType switch
        {
            ApplicationType.Loans => _programmeUrlConfig.StartLoanApplication
                                         .Replace("{organisationId}", Request.GetOrganisationIdFromRoute()!.ToString()) +
                                     $"?fdProjectId={fdProjectId}",
            ApplicationType.Ahp => _programmeUrlConfig.StartAhpProject
                                       .Replace("{organisationId}", Request.GetOrganisationIdFromRoute()!.ToString()) +
                                   $"?fdProjectId={fdProjectId}",
            _ => string.Empty,
        };

        if (await _featureManager.IsEnabledAsync(FeatureFlags.StayInCurrentApplication, cancellationToken))
        {
            return applicationType switch
            {
                ApplicationType.Loans => View(("Loans", urlWithRouteData)),
                ApplicationType.Ahp => View(("AHP", urlWithRouteData)),
                _ => View(("Not supported", string.Empty)),
            };
        }

        return Redirect(urlWithRouteData);
    }
}
