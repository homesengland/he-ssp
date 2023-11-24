using HE.Investment.AHP.Domain.HomeTypes;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable CA1716
namespace HE.Investment.AHP.WWW.Views.Shared.Components.HomeTypeFormHeader;
#pragma warning restore CA1716

public class HomeTypeFormHeader : ViewComponent
{
    public IViewComponentResult Invoke(
        HomeTypesWorkflowState currentPage,
        string applicationId,
        string? homeTypeId = null,
        string? title = null,
        string? caption = null)
    {
        return View("HomeTypeFormHeader", (Title: title, Caption: caption, ApplicationId: applicationId, HomeTypeId: homeTypeId, CurrentPage: currentPage));
    }
}
