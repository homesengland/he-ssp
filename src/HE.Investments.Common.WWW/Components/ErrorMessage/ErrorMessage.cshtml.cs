using HE.Investments.Common.WWW.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HE.Investments.Common.WWW.Components.ErrorMessage;

public class ErrorMessage : ViewComponent
{
    public IViewComponentResult Invoke(string fieldName)
    {
        var errors = new List<string>();
        var (_, message) = ViewData.ModelState.GetErrors(fieldName);

        errors.Add(message);
        errors.AddRange(GetMessageForDateInput(fieldName));

        return View("ErrorMessage", (fieldName, errors.Where(e => !string.IsNullOrWhiteSpace(e)).ToList()));
    }

    private List<string> GetMessageForDateInput(string fieldName)
    {
        var (_, dayInputError) = ViewData.ModelState.GetErrors($"{fieldName}.Day");
        var (_, monthInputError) = ViewData.ModelState.GetErrors($"{fieldName}.Month");
        var (_, yearInputError) = ViewData.ModelState.GetErrors($"{fieldName}.Year");

        return new List<string> { dayInputError, monthInputError, yearInputError };
    }
}
