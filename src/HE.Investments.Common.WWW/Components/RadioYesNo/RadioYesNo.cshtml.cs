using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.RadioYesNo;

public class RadioYesNo : ViewComponent
{
    public IViewComponentResult Invoke(
        string fieldName,
        string? title = null,
        InputTitleType? titleType = null,
        DynamicComponentViewModel? additionalContent = null,
        string? hint = null,
        bool? value = null,
        bool? isDisplayed = null)
    {
        return View("RadioYesNo", (fieldName, title, titleType ?? InputTitleType.InputTitle, additionalContent, hint, value, isDisplayed ?? true));
    }
}
