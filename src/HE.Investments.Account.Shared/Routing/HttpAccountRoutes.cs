using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Account.Shared.Routing;

public class HttpAccountRoutes : IAccountRoutes
{
    private readonly AccountConfig _config;

    public HttpAccountRoutes(AccountConfig config)
    {
        _config = config;
    }

    public IActionResult NotCompleteProfile(string? callbackProgramme = null, string? callbackRoute = null)
    {
        var query = BuildCallbackQuery(callbackProgramme, callbackRoute);
        return new RedirectResult($"{_config.Url}/{UserAccountEndpoints.ProfileDetails}{query}");
    }

    public IActionResult NotLinkedOrganisation()
    {
        return new RedirectResult($"{_config.Url}/{OrganisationAccountEndpoints.SearchOrganisation}");
    }

    public IActionResult LandingPageForNotLoggedUser()
    {
        return new RedirectResult(_config.Url);
    }

    public IActionResult LandingPageForLoggedUser()
    {
        return new RedirectResult($"{_config.Url}/{UserOrganisationAccountEndpoints.Dashboard}");
    }

    private static string? BuildCallbackQuery(string? callbackProgramme = null, string? callbackRoute = null)
    {
        if (string.IsNullOrEmpty(callbackRoute))
        {
            return null;
        }

        if (string.IsNullOrEmpty(callbackProgramme))
        {
            return $"?callback={callbackRoute}";
        }

        return $"?programme={callbackProgramme}&callback={callbackRoute}";
    }
}
