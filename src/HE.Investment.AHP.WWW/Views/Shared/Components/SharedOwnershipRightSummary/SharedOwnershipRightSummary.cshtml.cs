using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.SharedOwnershipRightSummary;

public class SharedOwnershipRightSummary : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("SharedOwnershipRightSummary");
    }
}
