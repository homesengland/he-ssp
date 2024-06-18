using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common;
using HE.Investments.Common.Contract.Enum;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.FrontDoor.WWW.Config;
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
            ApplicationType.Loans => $"{_programmeUrlConfig.StartLoanApplication}?fdProjectId={fdProjectId}?organisationId={Request.GetOrganisationIdFromRoute()}",
            ApplicationType.Ahp => $"{_programmeUrlConfig.StartAhpProject}?fdProjectId={fdProjectId}?organisationId={Request.GetOrganisationIdFromRoute()}",
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
