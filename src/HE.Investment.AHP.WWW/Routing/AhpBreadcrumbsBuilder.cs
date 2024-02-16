using HE.Investment.AHP.WWW.Controllers;
using HE.Investments.Account.Shared;
using HE.Investments.Common.WWW.Routing;

namespace HE.Investment.AHP.WWW.Routing;

public class AhpBreadcrumbsBuilder : BreadcrumbsBuilderBase
{
    public static AhpBreadcrumbsBuilder New()
    {
        return new AhpBreadcrumbsBuilder().WithHome();
    }

    public static AhpBreadcrumbsBuilder Empty()
    {
        return new AhpBreadcrumbsBuilder();
    }

    public AhpBreadcrumbsBuilder WithOrganisation(string organisationName)
    {
        AddBreadcrumb(organisationName, nameof(AccountController.Index), GetControllerName(nameof(AccountController)));

        return this;
    }

    public AhpBreadcrumbsBuilder WithSites()
    {
        AddBreadcrumb("Sites", nameof(SiteController.Index), GetControllerName(nameof(SiteController)));

        return this;
    }

    public AhpBreadcrumbsBuilder WithSchemes()
    {
        //// TODO: fix after implementing schemes
        AddBreadcrumb("Schemes", cssClass: "govuk-!-padding-right-4");

        return this;
    }

    public AhpBreadcrumbsBuilder WithApplicationsList()
    {
        AddBreadcrumb("AHP 21-26 CME", nameof(ApplicationController.Index), GetControllerName(nameof(ApplicationController)));

        return this;
    }

    public AhpBreadcrumbsBuilder WithApplication(string applicationId, string applicationName)
    {
        AddBreadcrumb(applicationName, nameof(ApplicationController.TaskList), GetControllerName(nameof(ApplicationController)), new { applicationId });

        return this;
    }

    private AhpBreadcrumbsBuilder WithHome()
    {
        AddBreadcrumb("Home", nameof(HomeController.Index), GetControllerName(nameof(HomeController)));

        return this;
    }
}
