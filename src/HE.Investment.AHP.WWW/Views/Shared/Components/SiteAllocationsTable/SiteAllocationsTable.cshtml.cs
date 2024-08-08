using System.Globalization;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Controllers;
using HE.Investments.AHP.ProjectDashboard.Contract.Site;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.WWW.Components;
using HE.Investments.Common.WWW.Components.Link;
using HE.Investments.Common.WWW.Components.Table;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Utils;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.SiteAllocationsTable;

public class SiteAllocationsTable : ViewComponent
{
    public Task<IViewComponentResult> InvokeAsync(SiteId siteId, IList<AllocationSiteModel> allocations)
    {
        var tableHeaders = new List<TableHeaderViewModel>
        {
            new("Allocation name", CellWidth.OneThird),
            new("Tenure"),
            new("No of homes", CellWidth.OneFifth),
            new("Status", CellWidth.OneFifth, IsHidden: true),
        };

        var organisationId = Request.GetOrganisationIdFromRoute();
        var applicationRows = allocations.Select(x =>
            {
                var tableItems = new List<TableValueViewModel>
                {
                    new(Component: CreateLinkComponent(organisationId, x)),
                    new(x.Tenure.GetDescription()),
                    new(x.NumberOfHomes.ToString(CultureInfo.InvariantCulture)),
                };

                return new TableRowViewModel(x.Id.Value, tableItems);
            })
            .ToList();

        return Task.FromResult<IViewComponentResult>(View("SiteAllocationsTable", (siteId, tableHeaders, applicationRows)));
    }

    private static DynamicComponentViewModel CreateLinkComponent(OrganisationId? organisationId, AllocationSiteModel allocation)
    {
        return new DynamicComponentViewModel(
            nameof(Link),
            new
            {
                text = allocation.Name,
                controller = new ControllerName(nameof(AllocationController)).WithoutPrefix(),
                action = nameof(AllocationController.TaskList),
                values = new { allocationId = allocation.Id.Value, organisationId },
                isStrong = true,
            });
    }
}
