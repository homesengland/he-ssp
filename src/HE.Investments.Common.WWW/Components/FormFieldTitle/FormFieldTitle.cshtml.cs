using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.FormFieldTitle;

public class FormFieldTitle : ViewComponent
{
    public IViewComponentResult Invoke(string title, string? fieldName = null)
    {
        return View("FormFieldTitle", (title, fieldName));
    }
}
