using HE.Investment.AHP.WWW.Models.Application;
using HE.Investments.Common.WWW.Components.Table;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.ApplicationSubmitTable;

public class ApplicationSubmitTable : ViewComponent
{
    public Task<IViewComponentResult> InvokeAsync(ApplicationSubmitModel model)
    {
        var tableHeaders = new List<TableHeaderViewModel>
        {
            new("Site name", CellWidth.OneSixth),
            new("Scheme name and tenure"),
            new("Number of homes", CellWidth.OneFifth),
            new("Funding requested", CellWidth.OneFifth),
            new("Scheme cost", CellWidth.OneSixth),
        };

        var rows = new List<TableRowViewModel>
        {
            new(
                model.ApplicationId,
                new List<TableValueViewModel>
                {
                    new(model.SiteName),
                    new($"{model.ApplicationName}: {model.Tenure}"),
                    new(model.NumberOfHomes),
                    new(model.FundingRequested),
                    new(model.TotalSchemeCost),
                }),
        };

        return Task.FromResult<IViewComponentResult>(View("ApplicationSubmitTable", (tableHeaders, rows)));
    }
}
