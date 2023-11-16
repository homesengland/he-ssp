using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Account.Shared.Routing;

public class HttpAccountRoutes : IAccountRoutes
{
    private readonly AccountConfig _config;

    public HttpAccountRoutes(AccountConfig config)
    {
        _config = config;
    }

    public IActionResult NotCompleteProfile()
    {
        return new RedirectResult($"{_config.Url}/{UserAccountEndpoints.ProfileDetails}");
    }

    public IActionResult NotLinkedOrganisation()
    {
        return new RedirectResult($"{_config.Url}/{OrganisationAccountEndpoints.SearchOrganization}");
    }

    public IActionResult LandingPageForNotLoggedUser()
    {
        throw new NotImplementedException("Redirection to guidance page should be here");
    }

    public IActionResult LandingPageForLoggedUser()
    {
        return new RedirectResult($"{_config.Url}/{UserOrganisationAccountEndpoints.Dashboard}");
    }
}
