using Microsoft.AspNetCore.Mvc;

#pragma warning disable CA1716
namespace HE.Investment.AHP.WWW.Views.Shared.Components.FormFieldTitle;
#pragma warning restore CA1716

public class FormFieldTitle : ViewComponent
{
    public IViewComponentResult Invoke(string title, string fieldName = null)
    {
        return View("FormFieldTitle", (title, fieldName));
    }
}
