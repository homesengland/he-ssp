using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common;
using HE.Investments.Common.WWW.Controllers;
using HE.Investments.Loans.WWW.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

namespace HE.Investments.Loans.WWW.Controllers;

[Route("apply-for-support/{organisationId}")]
[AuthorizeWithCompletedProfile]
public class FrontDoorController : Controller
{
    private readonly FrontDoorConfig _frontDoorConfig;

    private readonly IFeatureManager _featureManager;

    public FrontDoorController(FrontDoorConfig frontDoorConfig, IFeatureManager featureManager)
    {
        _frontDoorConfig = frontDoorConfig;
        _featureManager = featureManager;
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> StartNewProject([FromRoute] string organisationId, CancellationToken cancellationToken)
    {
        if (await _featureManager.IsEnabledAsync(FeatureFlags.StayInCurrentApplication, cancellationToken))
        {
            return this.OrganisationRedirectToAction("AboutLoan", "LoanApplicationV2");
        }

        return new RedirectResult(_frontDoorConfig.StartNewProjectUrl.Replace("{organisationId}", organisationId));
    }

    [HttpGet]
    [Route("return-to-project-check-answers")]
    public async Task<IActionResult> BackToCheckAnswers([FromQuery] string fdProjectId, [FromRoute] string organisationId, CancellationToken cancellationToken)
    {
        if (await _featureManager.IsEnabledAsync(FeatureFlags.StayInCurrentApplication, cancellationToken))
        {
            return this.OrganisationRedirectToAction("Index", "Home");
        }

        var redirectUrl = _frontDoorConfig.ProjectCheckAnswers.Replace("{projectId}", fdProjectId).Replace("{organisationId}", organisationId);
        return new RedirectResult(redirectUrl);
    }
}
