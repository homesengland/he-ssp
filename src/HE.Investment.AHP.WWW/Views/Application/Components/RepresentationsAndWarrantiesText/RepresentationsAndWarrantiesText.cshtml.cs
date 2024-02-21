using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Application.Components.RepresentationsAndWarrantiesText;

public class RepresentationsAndWarrantiesText : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("RepresentationsAndWarrantiesText");
    }
}
