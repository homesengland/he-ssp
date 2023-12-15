using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.TextInput;

public class TextInput : ViewComponent
{
    public IViewComponentResult Invoke(string fieldName, string? value = null, string? label = null, string? hint = null, string? cssClass = null)
    {
        return View("TextInput", (fieldName, value, label, hint, cssClass));
    }
}
