using HE.Investments.Account.Shared.Authorization.Attributes;
using HE.Investments.Account.Shared.Routing;
using HE.Investments.Common;
using HE.Investments.Common.WWW.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

namespace HE.Investments.FrontDoor.WWW.Controllers;

[Route("{organisationId}/account")]
[AuthorizeWithCompletedProfile]
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
    public async Task<IActionResult> Index([FromRoute] string organisationId, CancellationToken cancellationToken)
    {
        if (await _featureManager.IsEnabledAsync(FeatureFlags.StayInCurrentApplication, cancellationToken))
        {
            return this.OrganisationRedirectToAction("Index", "Projects");
        }

        return new RedirectResult($"{_accountConfig.Url}/{organisationId}/user-organisation");
    }
}
