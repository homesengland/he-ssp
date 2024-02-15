using System.Globalization;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Controllers;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.WWW.Components;
using HE.Investments.Common.WWW.Components.ApplicationStatusTagComponent;
using HE.Investments.Common.WWW.Components.Link;
using HE.Investments.Common.WWW.Components.Table;
using HE.Investments.Common.WWW.Utils;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.SiteApplicationsTable;

public class SiteApplicationsTable : ViewComponent
{
    public Task<IViewComponentResult> InvokeAsync(SiteId siteId, PaginationResult<ApplicationSiteModel> applications)
    {
        var tableHeaders = new List<TableHeaderViewModel>
        {
            new("Application name", CellWidth.OneThird),
            new("Application tenure"),
            new("No of homes"),
            new("Status"),
        };

        var applicationsPage = applications.Items.Select(x =>
            {
                var tableItems = new List<TableValueViewModel>
                {
                    new(Component: CreateLinkComponent(x)),
                    new(x.Tenure.GetDescription()),
                    new(x.NumberOfHomes?.ToString(CultureInfo.InvariantCulture) ?? GenericMessages.NotProvided),
                    new(Component: CreateApplicationStatusComponent(x)),
                };

                return new TableRowViewModel(tableItems);
            })
            .ToList();

        var rows = new PaginationResult<TableRowViewModel>(applicationsPage, applications.CurrentPage, applications.ItemsPerPage, applications.TotalItems);

        return Task.FromResult<IViewComponentResult>(View("SiteApplicationsTable", (siteId, tableHeaders, rows)));
    }

    private static DynamicComponentViewModel CreateLinkComponent(ApplicationSiteModel application)
    {
        // TODO: data-testId: "application-@application.Id.Value"
        return new DynamicComponentViewModel(
            nameof(Link),
            new
            {
                text = application.Name,
                controller = new ControllerName(nameof(ApplicationController)).WithoutPrefix(),
                action = nameof(ApplicationController.TaskList),
                values = new { applicationId = application.Id.Value },
                isStrong = true,
            });
    }

    private static DynamicComponentViewModel CreateApplicationStatusComponent(ApplicationSiteModel application)
    {
        return new DynamicComponentViewModel(nameof(ApplicationStatusTagComponent), new { applicationStatus = application.Status });
    }
}
