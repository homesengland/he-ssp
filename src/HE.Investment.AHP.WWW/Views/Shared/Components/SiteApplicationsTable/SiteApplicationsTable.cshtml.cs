using System.Globalization;
using HE.Investment.AHP.Contract.Site;
using HE.Investment.AHP.WWW.Controllers;
using HE.Investments.AHP.ProjectDashboard.Contract.Site;
using HE.Investments.Common.Contract;
using HE.Investments.Common.Extensions;
using HE.Investments.Common.Messages;
using HE.Investments.Common.WWW.Components;
using HE.Investments.Common.WWW.Components.ApplicationStatusTagComponent;
using HE.Investments.Common.WWW.Components.Link;
using HE.Investments.Common.WWW.Components.Table;
using HE.Investments.Common.WWW.Enums;
using HE.Investments.Common.WWW.Extensions;
using HE.Investments.Common.WWW.Utils;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.SiteApplicationsTable;

public class SiteApplicationsTable : ViewComponent
{
    public Task<IViewComponentResult> InvokeAsync(SiteId siteId, IList<ApplicationSiteModel> applications)
    {
        var tableHeaders = new List<TableHeaderViewModel>
        {
            new("Application name", CellWidth.OneThird),
            new("Tenure"),
            new("No of homes", CellWidth.OneFifth),
            new("Status", CellWidth.OneFifth),
        };

        var organisationId = Request.GetOrganisationIdFromRoute();
        var applicationRows = applications.Select(x =>
            {
                var tableItems = new List<TableValueViewModel>
                {
                    new(Component: CreateLinkComponent(organisationId, x)),
                    new(x.Tenure?.GetDescription()),
                    new(x.NumberOfHomes?.ToString(CultureInfo.InvariantCulture) ?? GenericMessages.NotProvided),
                    new(Component: CreateApplicationStatusComponent(x)),
                };

                return new TableRowViewModel(x.Id.Value, tableItems);
            })
            .ToList();

        return Task.FromResult<IViewComponentResult>(View("SiteApplicationsTable", (siteId, tableHeaders, applicationRows)));
    }

    private static DynamicComponentViewModel CreateLinkComponent(OrganisationId? organisationId, ApplicationSiteModel application)
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

    private static DynamicComponentViewModel CreateApplicationStatusComponent(ApplicationSiteModel application)
    {
        return new DynamicComponentViewModel(nameof(ApplicationStatusTagComponent), new { applicationStatus = application.Status });
    }
}
