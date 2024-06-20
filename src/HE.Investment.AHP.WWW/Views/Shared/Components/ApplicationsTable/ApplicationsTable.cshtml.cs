using System.Globalization;
using HE.Investment.AHP.Contract.Project;
using HE.Investment.AHP.WWW.Controllers;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.WWW.Components;
using HE.Investments.Common.WWW.Components.ApplicationStatusTagComponent;
using HE.Investments.Common.WWW.Components.Link;
using HE.Investments.Common.WWW.Components.Table;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Helpers;
using HE.Investments.Common.WWW.Utils;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.ApplicationsTable;

public class ApplicationsTable : ViewComponent
{
    public Task<IViewComponentResult> InvokeAsync(PaginationResult<ApplicationProjectModel> applications, string projectId)
    {
        var tableHeaders = new List<TableHeaderViewModel>
        {
            new("Name", CellWidth.OneThird),
            new("Units", CellWidth.OneEighth),
            new("Grant", CellWidth.OneEighth),
            new("Status", CellWidth.OneFifth),
        };

        var organisationId = HttpContext.GetOrganisationIdFromRoute();
        var applicationsPage = applications.Items.Select(x =>
            {
                var tableItems = new List<TableValueViewModel>
                {
                    new(Component: CreateLinkComponent(x, organisationId)),
                    new(x.Unit?.ToString(CultureInfo.InvariantCulture)),
                    new(x.Grant.DisplayPounds()),
                    new(Component: CreateApplicationStatusComponent(x)),
                };

                return new TableRowViewModel(x.Id.Value, tableItems);
            })
            .ToList();

        var rows = new PaginationResult<TableRowViewModel>(applicationsPage, applications.CurrentPage, applications.ItemsPerPage, applications.TotalItems);

        return Task.FromResult<IViewComponentResult>(View("ApplicationsTable", (tableHeaders, rows, projectId)));
    }

    private static DynamicComponentViewModel CreateLinkComponent(ApplicationProjectModel application, OrganisationId? organisationId)
    {
        return new DynamicComponentViewModel(
            nameof(Link),
            new
            {
                text = application.Name,
                controller = new ControllerName(nameof(ApplicationController)).WithoutPrefix(),
                action = nameof(ApplicationController.TaskList),
                values = new { applicationId = application.Id.Value, organisationId },
                isStrong = true,
            });
    }

    private static DynamicComponentViewModel CreateApplicationStatusComponent(ApplicationProjectModel application)
    {
        return new DynamicComponentViewModel(nameof(ApplicationStatusTagComponent), new { applicationStatus = application.Status });
    }
}
