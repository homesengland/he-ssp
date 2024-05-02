using System.Collections.Generic;
using System.Linq;
using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HE.Investments.Common.WWW.Components.CheckboxList;

public class CheckboxList : ViewComponent
{
    public IViewComponentResult Invoke(
        string fieldName,
        IEnumerable<ExtendedSelectListItem> availableOptions,
        DynamicComponentViewModel? headerComponent = null,
        string? title = null,
        InputTitleType? titleType = null,
        string? hint = null,
        IEnumerable<Enum>? selectedValues = null)
    {
        var availableOptionsList = availableOptions.ToList();

        foreach (var value in selectedValues ?? [])
        {
            var option = availableOptionsList.Find(x => x.Value == value.ToString());
            if (option != null)
            {
                option.Selected = true;
            }
        }

        return View(
            "CheckboxList",
            (fieldName, title, titleType ?? InputTitleType.InputTitle, headerComponent, hint, availableOptionsList.AsEnumerable()));
    }
}
