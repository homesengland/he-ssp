using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.NumericTextInput;

public class NumericTextInput : ViewComponent
{
    public IViewComponentResult Invoke(string fieldName, string? value = null, string? label = null, string? hint = null, string? prefix = null, string? cssClass = null)
    {
        return View("NumericTextInput", (fieldName, value, label, hint, prefix, cssClass));
    }
}
