using Microsoft.AspNetCore.Mvc;

namespace HE.Investments.FrontDoor.WWW.Views.Shared.Components.ConfirmLocalAuthoritySelectionHeader;

public class ConfirmLocalAuthoritySelectionHeader : ViewComponent
{
    public IViewComponentResult Invoke(string header, string name, string description)
    {
        return View("ConfirmLocalAuthoritySelectionHeader", (header, name, description));
    }
}
