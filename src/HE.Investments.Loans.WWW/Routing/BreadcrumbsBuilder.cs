using HE.Investments.Common.WWW.Routing;
using HE.Investments.Loans.WWW.Controllers;

namespace HE.Investments.Loans.WWW.Routing;

public class BreadcrumbsBuilder : BreadcrumbsBuilderBase
{
    public static BreadcrumbsBuilder New() => new();

    public BreadcrumbsBuilder WithOrganisations()
    {
#pragma warning disable S1135 // Track uses of "TODO" tags
        //// TODO: Fix link for AHP when organisations list will be ready
#pragma warning restore S1135 // Track uses of "TODO" tags
        AddBreadcrumb("Your Organisations");

        return this;
    }

    public BreadcrumbsBuilder WithOrganisation(string name)
    {
#pragma warning disable S1135 // Track uses of "TODO" tags
        //// TODO: Fix link - for AHP should point to organisation with specific Id
#pragma warning restore S1135 // Track uses of "TODO" tags
        AddBreadcrumb(name, nameof(UserOrganisationController.Index), GetControllerName(nameof(UserOrganisationController)));

        return this;
    }

    public BreadcrumbsBuilder WithLuhbfApplications()
    {
        AddBreadcrumb("LUHBF applications", nameof(HomeController.Dashboard), GetControllerName(nameof(HomeController)));

        return this;
    }

    public BreadcrumbsBuilder WithApplication(string name, Guid id)
    {
        AddBreadcrumb(name, nameof(LoanApplicationV2Controller.ApplicationDashboard), GetControllerName(nameof(LoanApplicationV2Controller)), new { id });

        return this;
    }
}
