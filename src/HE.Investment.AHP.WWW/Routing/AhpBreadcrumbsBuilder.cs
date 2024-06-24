using HE.Investment.AHP.WWW.Controllers;
using HE.Investments.Common.WWW.Routing;

namespace HE.Investment.AHP.WWW.Routing;

public class AhpBreadcrumbsBuilder : BreadcrumbsBuilderBase
{
    private readonly string? _organisationId;

    public AhpBreadcrumbsBuilder(string? organisationId)
    {
        _organisationId = organisationId;
    }

    public static AhpBreadcrumbsBuilder New(string? organisationId)
    {
        return new AhpBreadcrumbsBuilder(organisationId).WithHome();
    }

    public static AhpBreadcrumbsBuilder Empty(string? organisationId)
    {
        return new AhpBreadcrumbsBuilder(organisationId);
    }

    public AhpBreadcrumbsBuilder WithOrganisation(string organisationName)
    {
        AddBreadcrumb(organisationName, nameof(AccountController.Index), GetControllerName(nameof(AccountController)), new { organisationId = _organisationId });

        return this;
    }

    public AhpBreadcrumbsBuilder WithSites(string projectId)
    {
        AddBreadcrumb("Sites", nameof(SiteController.Index), GetControllerName(nameof(SiteController)), new { projectId, organisationId = _organisationId });

        return this;
    }

    public AhpBreadcrumbsBuilder WithSchemes()
    {
        //// TODO: fix after implementing schemes
        AddBreadcrumb("Schemes", cssClass: "govuk-!-padding-right-4");

        return this;
    }

    public AhpBreadcrumbsBuilder WithApplicationsList(string? programmeName = null)
    {
        AddBreadcrumb(programmeName ?? "AHP 21-26 CME", nameof(ApplicationController.Index), GetControllerName(nameof(ApplicationController)), new { organisationId = _organisationId });

        return this;
    }

    public AhpBreadcrumbsBuilder WithApplication(string applicationId, string applicationName)
    {
        AddBreadcrumb(applicationName, nameof(ApplicationController.TaskList), GetControllerName(nameof(ApplicationController)), new { applicationId, organisationId = _organisationId });

        return this;
    }

    public AhpBreadcrumbsBuilder WithConsortiumManagement()
    {
        AddBreadcrumb("Consortium Management", nameof(ConsortiumController.Index), GetControllerName(nameof(ConsortiumController)), new { organisationId = _organisationId });

        return this;
    }

    private AhpBreadcrumbsBuilder WithHome()
    {
        AddBreadcrumb("Home", nameof(HomeController.Index), GetControllerName(nameof(HomeController)), new { organisationId = _organisationId });

        return this;
    }
}
