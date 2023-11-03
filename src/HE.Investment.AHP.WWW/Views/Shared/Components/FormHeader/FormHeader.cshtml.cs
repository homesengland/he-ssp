using Microsoft.AspNetCore.Mvc;

#pragma warning disable CA1716
namespace HE.Investment.AHP.WWW.Views.Shared.Components.FormHeader;
#pragma warning restore CA1716

public class FormHeader : ViewComponent
{
    public IViewComponentResult Invoke(string title, string caption = null)
    {
        return View("FormHeader", (Caption: caption, Title: title));
    }
}
