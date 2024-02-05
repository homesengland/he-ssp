using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Account.Shared.Routing;

public interface IAccountRoutes
{
    IActionResult NotCompleteProfile(string? callbackProgramme = null, string? callbackRoute = null);

    IActionResult NotLinkedOrganisation();

    IActionResult LandingPageForNotLoggedUser();

    IActionResult LandingPageForLoggedUser();
}
