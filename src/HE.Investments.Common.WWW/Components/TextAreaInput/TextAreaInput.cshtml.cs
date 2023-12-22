using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.TextAreaInput;

public class TextAreaInput : ViewComponent
{
    public IViewComponentResult Invoke(
        string fieldName,
        string? label = null,
        string? hint = null,
        string? value = null,
        string? cssClass = null,
        int? rows = null)
    {
        return View("TextAreaInput", (fieldName, label, hint, value, cssClass, rows ?? 5));
    }
}
