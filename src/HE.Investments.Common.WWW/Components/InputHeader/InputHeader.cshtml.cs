using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.InputHeader;

public class InputHeader : ViewComponent
{
    public IViewComponentResult Invoke(
        string fieldName,
        string? title = null,
        InputTitleType? titleType = null,
        string? hint = null,
        string? boldParagraph = null,
        string? paragraph = null)
    {
        return View("InputHeader", (fieldName, title, titleType ?? InputTitleType.InputTitle, hint, boldParagraph, paragraph));
    }
}
