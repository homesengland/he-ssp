using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.PracticalCompletionMilestoneDetails;

public class PracticalCompletionMilestoneDetails : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("PracticalCompletionMilestoneDetails");
    }
}
