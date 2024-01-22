using HE.Investments.Common.WWW.Components.Table;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.TextAreaInputWithCounter;

public class TextAreaInputWithCounter : ViewComponent
{
    public IViewComponentResult Invoke(
        string fieldName,
        string? header = null,
        DynamicComponentViewModel? descriptionComponent = null,
        string? title = null,
        string? hint = null,
        string? value = null,
        string? inputCssClass = null,
        int? rows = null,
        int? maxLength = null)
    {
        return View("TextAreaInputWithCounter", (fieldName, header, title, descriptionComponent, hint, value, inputCssClass, rows ?? 7, maxLength ?? 1500));
    }
}
