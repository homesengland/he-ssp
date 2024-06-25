using HE.Investments.Account.WWW.Controllers;
using HE.Investments.Common.WWW.Routing;

namespace HE.Investments.Account.WWW.Utils;

public class BreadcrumbsBuilder : BreadcrumbsBuilderBase
{
    public static BreadcrumbsBuilder New() => new();

    public BreadcrumbsBuilder WithOrganisations()
    {
        AddBreadcrumb("Your Organisations", nameof(UserOrganisationsController.List), GetControllerName(nameof(UserOrganisationsController)));
        return this;
    }

    public BreadcrumbsBuilder WithOrganisation(string name, string? organisationId)
    {
        AddBreadcrumb(name, nameof(UserOrganisationController.Index), GetControllerName(nameof(UserOrganisationController)), new { organisationId });

        return this;
    }
}
