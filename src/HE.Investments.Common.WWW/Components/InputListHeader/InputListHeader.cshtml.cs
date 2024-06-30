using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.InputListHeader;

public class InputListHeader : ViewComponent
{
    public IViewComponentResult Invoke(
        string? title = null,
        InputTitleType? titleType = null,
        DynamicComponentViewModel? additionalContent = null,
        string? hint = null,
        string? paragraph = null)
    {
        return View("InputListHeader", (title, titleType ?? InputTitleType.InputTitle, additionalContent, hint, paragraph));
    }
}
