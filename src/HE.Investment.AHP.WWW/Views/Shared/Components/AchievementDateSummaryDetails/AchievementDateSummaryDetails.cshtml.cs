using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.AchievementDateSummaryDetails;

public class AchievementDateSummaryDetails : ViewComponent
{
    public IViewComponentResult Invoke(MilestoneType milestoneType)
    {
        return View("AchievementDateSummaryDetails", milestoneType);
    }
}
