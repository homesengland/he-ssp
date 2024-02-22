using HE.Investments.Common.WWW.Components;
using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.ConfirmationCheckbox;

public class ConfirmationCheckbox : ViewComponent
{
    public IViewComponentResult Invoke(
        string fieldName,
        string? value,
        ExtendedSelectListItem checkbox,
        DynamicComponentViewModel? contentComponent = null)
    {
        if (value == "checked")
        {
            checkbox.Selected = true;
        }

        return View(
            "ConfirmationCheckbox",
            (fieldName, value, checkbox, contentComponent));
    }
}
