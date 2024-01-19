using HE.Investments.Account.WWW.Controllers;
using HE.Investments.Common.WWW.Routing;

namespace HE.Investments.Account.WWW.Utils;

public class BreadcrumbsBuilder : BreadcrumbsBuilderBase
{
    public static BreadcrumbsBuilder New() => new();

    public BreadcrumbsBuilder WithOrganisations()
    {
#pragma warning disable S1135 // Track uses of "TODO" tags
        // TODO: Fix link for AHP when organisations list will be ready
#pragma warning restore S1135 // Track uses of "TODO" tags
        AddBreadcrumb("Your Organisations");

        return this;
    }

    public BreadcrumbsBuilder WithOrganisation(string name)
    {
#pragma warning disable S1135 // Track uses of "TODO" tags
        // TODO: Fix link - for AHP should point to organisation with specific Id
#pragma warning restore S1135 // Track uses of "TODO" tags

        AddBreadcrumb(name, nameof(UserOrganisationController.Index), GetControllerName(nameof(UserOrganisationController)));

        return this;
    }
}
