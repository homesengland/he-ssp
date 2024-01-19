using HE.Investment.AHP.WWW.Controllers;
using HE.Investments.Common.WWW.Routing;

namespace HE.Investment.AHP.WWW.Routing;

public class AhpBreadcrumbsBuilder : BreadcrumbsBuilderBase
{
    public static AhpBreadcrumbsBuilder New()
    {
        return new AhpBreadcrumbsBuilder().WithHome();
    }

    public AhpBreadcrumbsBuilder WithSites()
    {
        //// TODO: fix after implementing sites

        AddBreadcrumb("Sites");

        return this;
    }

    public AhpBreadcrumbsBuilder WithSchemes()
    {
        //// TODO: fix after implementing schemes
        AddBreadcrumb("Schemes", cssClass: "govuk-!-padding-right-4");

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
