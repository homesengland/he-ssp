using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Controllers;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Contract.Pagination;
using HE.Investments.Common.Messages;
using HE.Investments.Common.WWW.Components;
using HE.Investments.Common.WWW.Components.Link;
using HE.Investments.Common.WWW.Components.Table;
using HE.Investments.Common.WWW.Enums;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Utils;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.SitesTable;

public class SitesTable : ViewComponent
{
    public Task<IViewComponentResult> InvokeAsync(PaginationResult<SiteBasicModel> sites, string projectId)
    {
        var tableHeaders = new List<TableHeaderViewModel>
        {
            new("Name", CellWidth.OneThird),
            new("Local authority", CellWidth.OneThird),
            new("Status", CellWidth.OneThird),
        };

        var organisationId = HttpContext.GetOrganisationIdFromRoute();
        var applicationsPage = sites.Items.Select(x =>
            {
                var tableItems = new List<TableValueViewModel>
                {
                    new(Component: CreateLinkComponent(x, organisationId)),
                    new(x.LocalAuthorityName ?? GenericMessages.NotProvided),
                    new(Component: CreateSiteStatusComponent(x)),
                };

                return new TableRowViewModel(x.Id.Value, tableItems);
            })
            .ToList();

        var rows = new PaginationResult<TableRowViewModel>(applicationsPage, sites.CurrentPage, sites.ItemsPerPage, sites.TotalItems);

        return Task.FromResult<IViewComponentResult>(View("SitesTable", (tableHeaders, rows, projectId)));
    }

    private static DynamicComponentViewModel CreateLinkComponent(SiteBasicModel site, OrganisationId? organisationId)
    {
        return new DynamicComponentViewModel(
            nameof(Link),
            new
            {
                text = site.Name,
                controller = new ControllerName(nameof(SiteController)).WithoutPrefix(),
                action = nameof(SiteController.Details),
                values = new { siteId = site.Id, organisationId },
                isStrong = true,
            });
    }

    private static DynamicComponentViewModel CreateSiteStatusComponent(SiteBasicModel site)
    {
        return new DynamicComponentViewModel(nameof(SiteStatusTag.SiteStatusTag), new { siteStatus = site.Status });
    }
}
