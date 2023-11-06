using Microsoft.AspNetCore.Mvc;

#pragma warning disable CA1716
namespace HE.Investment.AHP.WWW.Views.Shared.Components.FormButton;
#pragma warning restore CA1716

public class FormButton : ViewComponent
{
    public IViewComponentResult Invoke(string text = "Save and continue")
    {
        return View("FormButton", text);
    }
}
