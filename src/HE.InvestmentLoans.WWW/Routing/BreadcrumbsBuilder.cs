using HE.InvestmentLoans.WWW.Controllers;
using HE.Investments.Common.WWW.Routing;

namespace HE.InvestmentLoans.WWW.Routing;

public class BreadcrumbsBuilder : BreadcrumbsBuilderBase
{
    public static BreadcrumbsBuilder New() => new();

    public BreadcrumbsBuilder WithOrganisations()
    {
        // TODO: Fix link for AHP when organisations list will be ready
        AddBreadcrumb("Your Organisations");

        return this;
    }

    public BreadcrumbsBuilder WithOrganisation(string name)
    {
        // TODO: Fix link - for AHP should point to organisation with specific Id
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
