using HE.Investments.Common.Contract;
using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.DateInput;

public class DateInput : ViewComponent
{
    public IViewComponentResult Invoke(string fieldName, string title, string? description = null, string? hint = null, DateDetails? value = null, bool? isDisplayed = null)
    {
        return View("DateInput", (fieldName, title, description, hint, value ?? DateDetails.Empty(), isDisplayed ?? true));
    }
}
