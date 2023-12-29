using HE.Investments.Common.WWW.Components.Table;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.TextAreaInputWithCounter;

public class TextAreaInputWithCounter : ViewComponent
{
    public IViewComponentResult Invoke(
        string fieldName,
        string? label = null,
        ComponentViewModel? descriptionComponent = null,
        bool? isHeadingLabel = null,
        string? hint = null,
        string? value = null,
        string? inputCssClass = null,
        int? rows = null,
        int? maxLength = null)
    {
        return View("TextAreaInputWithCounter", (fieldName, label, descriptionComponent, hint, value, inputCssClass, isHeadingLabel ?? false, rows ?? 7, maxLength ?? 1500));
    }
}
