using HE.Investment.AHP.Contract.FinancialDetails;
using HE.Investment.AHP.Domain.FinancialDetails;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable CA1716
namespace HE.Investment.AHP.WWW.Views.Shared.Components.FinancialDetailsFormHeader;
#pragma warning restore CA1716

public class FinancialDetailsFormHeader : ViewComponent
{
    public IViewComponentResult Invoke(
        FinancialDetailsWorkflowState currentPage,
        string applicationId,
        string? homeTypeId = null,
        string? title = null,
        string? caption = null,
        string? customBackLinkUrl = null)
    {
        return View(
            "FinancialDetailsFormHeader",
            (Title: title, Caption: caption, ApplicationId: applicationId, CurrentPage: currentPage, CustomBackLinkUrl: customBackLinkUrl));
    }
}
