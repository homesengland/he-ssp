using HE.Investments.Common.Contract;
using HE.Investments.Common.Domain;
using HE.Investments.Common.WWW.Components;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.CheckboxListHeader;

public class CheckboxListHeader : ViewComponent
{
    public IViewComponentResult Invoke(
        DynamicComponentViewModel headerComponent,
        DynamicComponentViewModel? detailsComponent = null,
        string? hint = null)
    {
        return View("CheckboxListHeader", (headerComponent, detailsComponent, hint));
    }
}
