using System.Collections.Generic;
using System.Linq;
using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HE.Investments.Common.WWW.Components.CheckboxListWithOr;

public class CheckboxListWithOr : ViewComponent
{
    public IViewComponentResult Invoke(
        string fieldName,
        IEnumerable<ExtendedSelectListItem> availableOptions,
        ExtendedSelectListItem alternativeOption,
        DynamicComponentViewModel? headerComponent = null,
        string? header = null,
        string? title = null,
        string? hint = null,
        IEnumerable<string>? selectedValues = null)
    {
        var availableOptionsList = availableOptions.ToList();

        foreach (var value in selectedValues ?? new List<string>())
        {
            var option = availableOptionsList.Find(x => x.Value == value);
            if (option != null)
            {
                option.Selected = true;
            }

            if (alternativeOption.Value == value)
            {
                alternativeOption.Selected = true;
            }
        }

        return View("CheckboxListWithOr", (fieldName, headerComponent, header, title, hint, availableOptionsList.AsEnumerable(), alternativeOption));
    }
}
