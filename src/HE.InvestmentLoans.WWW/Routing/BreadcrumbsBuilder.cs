using HE.InvestmentLoans.WWW.Controllers;
using HE.Investments.Common.WWW.Utils;

namespace HE.InvestmentLoans.WWW.Routing;

public class BreadcrumbsBuilder
{
    private readonly List<Breadcrumb> _breadcrumbs;

    private BreadcrumbsBuilder()
    {
        _breadcrumbs = new List<Breadcrumb>();
    }

    public static BreadcrumbsBuilder New() => new();

    public BreadcrumbsBuilder WithOrganisations()
    {
        // TODO: Fix link for AHP when organisations list will be ready
        _breadcrumbs.Add(new Breadcrumb("Your Organisations"));

        return this;
    }

    public BreadcrumbsBuilder WithOrganisation(string name)
    {
        // TODO: Fix link - for AHP should point to organisation with specific Id
        _breadcrumbs.Add(new Breadcrumb(name, nameof(UserOrganisationController.Index), GetControllerName(nameof(UserOrganisationController))));

        return this;
    }

    public BreadcrumbsBuilder WithLuhbfApplications()
    {
        _breadcrumbs.Add(new Breadcrumb("LUHBF applications", nameof(HomeController.Dashboard), GetControllerName(nameof(HomeController))));

        return this;
    }

    public BreadcrumbsBuilder WithApplication(string name, Guid id)
    {
        _breadcrumbs.Add(new Breadcrumb(name, nameof(LoanApplicationV2Controller.ApplicationDashboard), GetControllerName(nameof(LoanApplicationV2Controller)), new { id }));

        return this;
    }

    public IList<Breadcrumb> Build()
    {
        return _breadcrumbs;
    }

    private string GetControllerName(string fullTypeName)
    {
        return new ControllerName(fullTypeName).WithoutPrefix();
    }
}
