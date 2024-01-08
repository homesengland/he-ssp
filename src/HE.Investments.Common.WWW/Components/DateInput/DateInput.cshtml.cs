using HE.Investments.Common.Contract;
using HE.Investments.Common.WWW.Models;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.DateInput;

public class DateInput : ViewComponent
{
    public IViewComponentResult Invoke(string fieldName, string header, string description, string hint, DateDetails value)
    {
        return View("DateInput", (fieldName, header, description, hint, value));
    }
}
