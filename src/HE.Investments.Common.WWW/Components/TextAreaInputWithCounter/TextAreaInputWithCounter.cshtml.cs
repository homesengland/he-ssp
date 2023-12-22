using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.TextAreaInputWithCounter;

public class TextAreaInputWithCounter : ViewComponent
{
    public IViewComponentResult Invoke(
        string fieldName,
        string? label = null,
        string? hint = null,
        string? value = null,
        string? cssClass = null,
        int? rows = null,
        int? maxLength = null)
    {
        return View("TextAreaInputWithCounter", (fieldName, label, hint, value, cssClass, rows ?? 5, maxLength ?? 1500));
    }
}
