using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HE.Investments.Common.WWW.Components.RadioListWithOr;

public class RadioListWithOr : ViewComponent
{
    public IViewComponentResult Invoke(
        string fieldName,
        string title,
        IEnumerable<ExtendedSelectListItem> availableOptions,
        ExtendedSelectListItem alternativeOption,
        string? hint = null,
        InputTitleType? titleType = null,
        Enum? value = null)
    {
        return View("RadioListWithOr", (fieldName, title, titleType ?? InputTitleType.InputTitle, hint, availableOptions, alternativeOption, value?.ToString()));
    }
}
