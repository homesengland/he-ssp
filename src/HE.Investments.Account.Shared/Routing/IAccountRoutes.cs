using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Account.Shared.Routing;

public interface IAccountRoutes
{
    IActionResult NotCompleteProfile();

    IActionResult NotLinkedOrganisation();

    IActionResult LandingPageForNotLoggedUser();

    IActionResult LandingPageForLoggedUser();
}
