using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investments.Common.WWW.Components.InputListHeader;

public class InputListHeader : ViewComponent
{
    public IViewComponentResult Invoke(
        string? title = null,
        InputTitleType? titleType = null,
        DynamicComponentViewModel? additionalContent = null,
        string? hint = null)
    {
        return View("InputListHeader", (title, titleType ?? InputTitleType.InputTitle, additionalContent, hint));
    }
}
