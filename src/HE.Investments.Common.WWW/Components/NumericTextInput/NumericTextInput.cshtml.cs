using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.NumericTextInput;

public class NumericTextInput : ViewComponent
{
    public IViewComponentResult Invoke(
        string fieldName,
        string? value = null,
        string? label = null,
        InputTitleType? titleType = null,
        string? hint = null,
        string? prefix = null,
        string? suffix = null,
        string? cssClass = null,
        string? boldParagraph = null)
    {
        return View("NumericTextInput", (fieldName, value, label, titleType, hint, prefix, suffix, cssClass, boldParagraph));
    }
}
