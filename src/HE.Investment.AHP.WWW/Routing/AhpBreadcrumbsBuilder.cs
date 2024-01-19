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
#pragma warning disable S1135 // Track uses of "TODO" tags
        //// TODO: fix after implementing sites
#pragma warning restore S1135 // Track uses of "TODO" tags

        AddBreadcrumb("Sites");

        return this;
    }

    public AhpBreadcrumbsBuilder WithSchemes()
    {
#pragma warning disable S1135 // Track uses of "TODO" tags
        //// TODO: fix after implementing schemes
#pragma warning restore S1135 // Track uses of "TODO" tags
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
