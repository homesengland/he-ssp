using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.Common.WWW.Components.FormButton;

public class FormButton : ViewComponent
{
    public IViewComponentResult Invoke(string text = "Save and continue", bool isDisabled = false)
    {
        return View("FormButton", (text, isDisabled));
    }
}
