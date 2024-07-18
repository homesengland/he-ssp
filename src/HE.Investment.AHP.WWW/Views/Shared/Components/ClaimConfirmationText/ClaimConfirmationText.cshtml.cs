using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.ClaimConfirmationText;

public class ClaimConfirmationText : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("ClaimConfirmationText");
    }
}
