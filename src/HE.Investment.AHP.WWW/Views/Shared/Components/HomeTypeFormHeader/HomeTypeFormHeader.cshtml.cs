using HE.Investment.AHP.Contract.HomeTypes;
using HE.Investment.AHP.Domain.HomeTypes;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable CA1716
namespace HE.Investment.AHP.WWW.Views.Shared.Components.HomeTypeFormHeader;
#pragma warning restore CA1716

public class HomeTypeFormHeader : ViewComponent
{
    public IViewComponentResult Invoke(
        HomeTypesWorkflowState currentPage,
        string? title = null,
        string? caption = null,
        bool isReadOnly = false)
    {
        return View("HomeTypeFormHeader", (Title: title, Caption: caption, CurrentPage: currentPage, isReadOnly));
    }
}
