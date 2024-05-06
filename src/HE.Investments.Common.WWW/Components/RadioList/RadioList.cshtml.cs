using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investments.Common.WWW.Components.RadioList;

public class RadioList : ViewComponent
{
    public IViewComponentResult Invoke(
        string fieldName,
        IEnumerable<ExtendedSelectListItem> availableOptions,
        string? title = null,
        InputTitleType? titleType = null,
        DynamicComponentViewModel? additionalContent = null,
        string? hint = null,
        Enum? value = null,
        string? stringValue = null)
    {
        return View(
            "RadioList",
            (fieldName, title, titleType ?? InputTitleType.InputTitle, additionalContent, hint, availableOptions, value?.ToString() ?? stringValue));
    }
}
