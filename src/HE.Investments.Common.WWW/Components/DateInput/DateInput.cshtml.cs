using HE.Investments.Common.Contract;
using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.DateInput;

public class DateInput : ViewComponent
{
    public IViewComponentResult Invoke(string fieldName, string title, InputTitleType? titleType = null, string? description = null, string? hint = null, DateDetails? value = null, bool? isDisplayed = null, bool? isDayHidden = false)
    {
        return View("DateInput", (fieldName, title, titleType ?? InputTitleType.InputTitle, description, hint, value ?? DateDetails.Empty(), isDisplayed ?? true, isDayHidden ?? false));
    }
}
