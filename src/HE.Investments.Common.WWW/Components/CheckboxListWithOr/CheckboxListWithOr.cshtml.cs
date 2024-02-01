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
        string title,
        string hint,
        string fieldName,
        IEnumerable<ExtendedSelectListItem> availableOptions,
        ExtendedSelectListItem alternativeOption,
        IEnumerable<string>? selectedValues)
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

        return View("CheckboxListWithOr", (title, hint, fieldName, availableOptionsList.AsEnumerable(), alternativeOption));
    }
}
