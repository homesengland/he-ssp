using System.Globalization;
using HE.Investment.AHP.WWW.Controllers;
using HE.Investments.AHP.ProjectDashboard.Contract.Project;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Components;
using HE.Investments.Common.WWW.Components.Link;
using HE.Investments.Common.WWW.Components.Table;
using HE.Investments.Common.WWW.Enums;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Utils;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.AllocationsTable;

public class AllocationsTable : ViewComponent
{
    public Task<IViewComponentResult> InvokeAsync(List<AllocationProjectModel> allocations, string projectId, int currentPage)
    {
        var tableHeaders = new List<TableHeaderViewModel>
        {
            new("Name", CellWidth.OneThird),
            new("Homes", CellWidth.OneEighth),
            new("Tenure", CellWidth.OneQuarter),
            new("Local authority", CellWidth.OneQuarter),
        };

        var organisationId = HttpContext.GetOrganisationIdFromRoute();
        var allocationsPage = allocations
            .Select(x =>
            {
                var tableItems = new List<TableValueViewModel>
                {
                    new(Component: CreateLinkComponent(x, organisationId)),
                    new(x.HouseToDeliver.ToString(CultureInfo.InvariantCulture)),
                    new(x.Tenure.GetDescription()),
                    new(x.LocalAuthorityName),
                };

                return new TableRowViewModel(x.Id, tableItems);
            })
            .ToList();

        var rows = new PaginationResult<TableRowViewModel>(allocationsPage, currentPage, DefaultPagination.PageSize, allocations.Count);

        return Task.FromResult<IViewComponentResult>(View("AllocationsTable", (tableHeaders, rows, projectId)));
    }

    private static DynamicComponentViewModel CreateLinkComponent(AllocationProjectModel allocation, OrganisationId? organisationId)
    {
        return new DynamicComponentViewModel(
            nameof(Link),
            new
            {
                text = allocation.Name,
                controller = new ControllerName(nameof(AllocationController)).WithoutPrefix(),
                action = nameof(AllocationController.Overview),
                values = new { allocationId = allocation.Id, organisationId },
                isStrong = true,
            });
    }
}
