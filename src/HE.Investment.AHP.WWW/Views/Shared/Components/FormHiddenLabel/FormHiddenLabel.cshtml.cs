using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.FormHiddenLabel;

public class FormHiddenLabel : ViewComponent
{
    public IViewComponentResult Invoke(string title, string fieldName)
    {
        return View("FormHiddenLabel", (title, fieldName));
    }
}
