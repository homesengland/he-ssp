using HE.Investments.Account.Shared.Routing;
using HE.Investments.Account.WWW.Controllers;
using HE.Investments.Common.WWW.Utils;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Account.WWW.Routing;

public class AccountRoutes : IAccountRoutes
{
    public IActionResult NotCompleteProfile(string? callbackProgramme = null, string? callbackRoute = null)
    {
        return new RedirectToActionResult(
            nameof(UserController.GetProfileDetails),
            new ControllerName(nameof(UserController)).WithoutPrefix(),
            new { callbackApplication = callbackProgramme, callbackRoute });
    }

    public IActionResult NotLinkedOrganisation()
    {
        return new RedirectToActionResult(
            nameof(OrganisationController.SearchOrganization),
            new ControllerName(nameof(OrganisationController)).WithoutPrefix(),
            null);
    }

    public IActionResult LandingPageForNotLoggedUser()
    {
        return new RedirectToActionResult(
            nameof(HomeController.Index),
            new ControllerName(nameof(HomeController)).WithoutPrefix(),
            null);
    }

    public IActionResult LandingPageForLoggedUser()
    {
        return new RedirectToActionResult(
            nameof(UserOrganisationController.Index),
            new ControllerName(nameof(UserOrganisationController)).WithoutPrefix(),
            null);
    }
}
