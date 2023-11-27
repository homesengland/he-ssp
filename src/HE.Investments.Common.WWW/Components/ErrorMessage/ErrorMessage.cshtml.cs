using HE.Investments.Common.WWW.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HE.Investments.Common.WWW.Components.ErrorMessage;

public class ErrorMessage : ViewComponent
{
    public IViewComponentResult Invoke(ModelExpression aspFor)
    {
        var (isVisible, message) = ViewData.ModelState.GetErrors(aspFor.Name);
        return View("ErrorMessage", (aspFor.Name, isVisible, message));
    }
}
