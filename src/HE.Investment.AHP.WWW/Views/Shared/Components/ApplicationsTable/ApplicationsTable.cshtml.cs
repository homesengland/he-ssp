using System.Globalization;
using HE.Investment.AHP.Contract.Application;
using HE.Investment.AHP.WWW.Controllers;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Messages;
using HE.Investments.Common.WWW.Components;
using HE.Investments.Common.WWW.Components.ApplicationStatusTagComponent;
using HE.Investments.Common.WWW.Components.Link;
using HE.Investments.Common.WWW.Components.Table;
using HE.Investments.Common.WWW.Helpers;
using HE.Investments.Common.WWW.TagHelpers;
using HE.Investments.Common.WWW.Utils;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.ApplicationsTable;

public class ApplicationsTable : ViewComponent
{
    public Task<IViewComponentResult> InvokeAsync(PaginationResult<ApplicationBasicDetails> applications)
    {
        var tableHeaders = new List<TableHeaderViewModel>
        {
            new("Name", CellWidth.OneThird),
            new("Units", CellWidth.OneEighth),
            new("Grant", CellWidth.OneEighth),
            new("Local authority", CellWidth.OneFifth),
            new("Status", CellWidth.OneFifth),
        };

        var applicationsPage = applications.Items.Select(x =>
            {
                var tableItems = new List<TableValueViewModel>
                {
                    new(Component: CreateLinkComponent(x)),
                    new(x.Unit?.ToString(CultureInfo.InvariantCulture)),
                    new(x.Grant.DisplayPounds()),
                    new(x.LocalAuthority ?? GenericMessages.NotProvided),
                    new(Component: CreateApplicationStatusComponent(x)),
                };

                return new TableRowViewModel(x.Id.Value, tableItems);
            })
            .ToList();

        var rows = new PaginationResult<TableRowViewModel>(applicationsPage, applications.CurrentPage, applications.ItemsPerPage, applications.TotalItems);

        return Task.FromResult<IViewComponentResult>(View("ApplicationsTable", (tableHeaders, rows)));
    }

    private static DynamicComponentViewModel CreateLinkComponent(ApplicationBasicDetails application)
    {
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

    private static DynamicComponentViewModel CreateApplicationStatusComponent(ApplicationBasicDetails application)
    {
        return new DynamicComponentViewModel(nameof(ApplicationStatusTagComponent), new { applicationStatus = application.Status });
    }
}
