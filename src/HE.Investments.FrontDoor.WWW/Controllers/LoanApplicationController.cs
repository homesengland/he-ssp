using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Common;
using HE.Investments.FrontDoor.WWW.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

namespace HE.Investments.FrontDoor.WWW.Controllers;

[Route("apply-for-loan-application")]
[AuthorizeWithCompletedProfile]
public class LoanApplicationController : Controller
{
    private readonly LoanApplicationConfig _loanApplicationConfig;

    private readonly IFeatureManager _featureManager;

    public LoanApplicationController(LoanApplicationConfig loanApplication, IFeatureManager featureManager)
    {
        _loanApplicationConfig = loanApplication;
        _featureManager = featureManager;
    }

    [Route("redirect-to-loans")]
    [Route("")]
    [HttpGet]
    public async Task<IActionResult> RedirectToLoans([FromQuery] string fdProjectId, CancellationToken cancellationToken)
    {
        if (!await _featureManager.IsEnabledAsync(FeatureFlags.RedirectToLoanApplication, cancellationToken))
        {
            return RedirectToAction("Index", "Projects");
        }

        var urlWithRouteData = $"{_loanApplicationConfig.StartNewLoanApplicationUrl}?fdProjectId={fdProjectId}";

        return Redirect(urlWithRouteData);
    }
}
