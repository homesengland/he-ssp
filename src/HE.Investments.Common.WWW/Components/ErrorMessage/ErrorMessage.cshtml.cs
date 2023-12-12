using HE.Investments.Common.WWW.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HE.Investments.Common.WWW.Components.ErrorMessage;

public class ErrorMessage : ViewComponent
{
    public IViewComponentResult Invoke(string fieldName)
    {
        var (isVisible, message) = ViewData.ModelState.GetErrors(fieldName);
        return View("ErrorMessage", (fieldName, isVisible, message));
    }
}
