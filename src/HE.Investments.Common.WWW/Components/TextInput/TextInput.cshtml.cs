using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.TextInput;

public class TextInput : ViewComponent
{
    public IViewComponentResult Invoke(string fieldName, string? value = null, string? header = null, string? title = null, string? hint = null, string? cssClass = null, bool? isDisplayed = null)
    {
        return View("TextInput", (fieldName, value, header, title, hint, cssClass ?? "govuk-input--width-full", isDisplayed ?? true));
    }
}
