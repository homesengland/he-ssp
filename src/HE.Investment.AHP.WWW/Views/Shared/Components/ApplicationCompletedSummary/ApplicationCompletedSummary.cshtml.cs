using HE.Investment.AHP.WWW.Models.Application;
using HE.Investments.Common.WWW.Components.SectionSummary;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.ApplicationCompletedSummary;

public class ApplicationCompletedSummary : ViewComponent
{
    public Task<IViewComponentResult> InvokeAsync(ApplicationSubmitModel model)
    {
        var items = new Dictionary<string, string>()
        {
            { "Site name", model.SiteName },
            { "Scheme name and tenure", $"{model.ApplicationName}: {model.Tenure}" },
            { "Number of homes", model.NumberOfHomes },
            { "Funding requested", model.FundingRequested },
            { "Scheme cost", model.TotalSchemeCost },
        };

        return Task.FromResult<IViewComponentResult>(View("ApplicationCompletedSummary", items));
    }
}
