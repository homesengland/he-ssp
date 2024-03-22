using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common;
using HE.Investments.Loans.WWW.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

namespace HE.Investments.Loans.WWW.Controllers;

[Route("apply-for-support")]
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
    public async Task<IActionResult> StartNewProject(CancellationToken cancellationToken)
    {
        if (await _featureManager.IsEnabledAsync(FeatureFlags.UseLocalLoansStartApplication, cancellationToken))
        {
            return RedirectToAction("AboutLoan", "LoanApplicationV2");
        }

        return new RedirectResult(_frontDoorConfig.StartNewProjectUrl);
    }

    [HttpGet]
    [Route("return-to-project-check-answers")]
    public async Task<IActionResult> BackToCheckAnswers([FromQuery] string fdProjectId, CancellationToken cancellationToken)
    {
        if (!await _featureManager.IsEnabledAsync(FeatureFlags.RedirectToProjectCheckAnswers, cancellationToken))
        {
            return RedirectToAction("Index", "Home");
        }

        var redirectUrl = _frontDoorConfig.ProjectCheckAnswers.Replace("{projectId}", fdProjectId);
        return new RedirectResult(redirectUrl);
    }
}
