using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.RepresentationsAndWarrantiesText;

public class RepresentationsAndWarrantiesText : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("RepresentationsAndWarrantiesText");
    }
}
