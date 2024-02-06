using HE.Investments.Common.WWW.Routing;
using HE.Investments.Loans.WWW.Controllers;

namespace HE.Investments.Loans.WWW.Routing;

public class BreadcrumbsBuilder : BreadcrumbsBuilderBase
{
    public static BreadcrumbsBuilder New() => new();

    public BreadcrumbsBuilder WithOrganisations()
    {
        AddBreadcrumb("Your Organisations", nameof(AccountsController.OrganisationDashboard), GetControllerName(nameof(AccountsController)));

        return this;
    }

    public BreadcrumbsBuilder WithOrganisation(string name)
    {
        //// TODO: Fix link - should point to organisation with specific Id
        AddBreadcrumb(name, nameof(AccountsController.OrganisationDashboard), GetControllerName(nameof(AccountsController)));

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
