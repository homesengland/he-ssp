using HE.Investments.Account.Shared.Routing;
using HE.Investments.Account.WWW.Controllers;
using HE.Investments.Common.WWW.Utils;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Account.WWW.Routing;

public class AccountRoutes : IAccountRoutes
{
    public IActionResult NotCompleteProfile()
    {
        return new RedirectToActionResult(
            nameof(UserController.GetProfileDetails),
            new ControllerName(nameof(UserController)).WithoutPrefix(),
            null);
    }

    public IActionResult NotLinkedOrganisation()
    {
        return new RedirectToActionResult(
            nameof(OrganisationController.SearchOrganization),
            new ControllerName(nameof(OrganisationController)).WithoutPrefix(),
            null);
    }

    public IActionResult NotLoggedUser()
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
