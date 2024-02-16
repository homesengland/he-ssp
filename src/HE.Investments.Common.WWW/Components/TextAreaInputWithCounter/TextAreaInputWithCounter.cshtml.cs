using HE.Investments.Common.WWW.Components.InputHeader;
using HE.Investments.Common.WWW.Components.Table;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.TextAreaInputWithCounter;

public class TextAreaInputWithCounter : ViewComponent
{
    public IViewComponentResult Invoke(
        string fieldName,
        DynamicComponentViewModel? descriptionComponent = null,
        string? title = null,
        InputTitleType? titleType = null,
        string? hint = null,
        string? value = null,
        string? inputCssClass = null,
        int? rows = null,
        int? maxLength = null,
        bool? isDisplayed = null)
    {
        return View("TextAreaInputWithCounter", (fieldName, title, titleType, descriptionComponent, hint, value, inputCssClass, rows ?? 7, maxLength ?? 1500, isDisplayed ?? true));
    }
}
