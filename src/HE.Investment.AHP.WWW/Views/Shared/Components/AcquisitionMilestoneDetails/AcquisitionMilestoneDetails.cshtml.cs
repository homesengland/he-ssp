using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.AcquisitionMilestoneDetails;

public class AcquisitionMilestoneDetails : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("AcquisitionMilestoneDetails");
    }
}
