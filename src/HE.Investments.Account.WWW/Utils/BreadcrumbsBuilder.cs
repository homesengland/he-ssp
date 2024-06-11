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

    public BreadcrumbsBuilder WithOrganisation(string name)
    {
        // TODO: Fix link - for AHP should point to organisation with specific Id
        AddBreadcrumb(name, nameof(UserOrganisationController.Index), GetControllerName(nameof(UserOrganisationController)));

        return this;
    }
}
