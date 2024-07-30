using HE.Investments.AHP.Allocation.Contract.Claims.Enum;
using HE.Investments.Common.Gds;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.ClaimStatusTag;

public class ClaimStatusTag : ViewComponent
{
    public IViewComponentResult Invoke(MilestoneStatus claimStatus)
    {
        return View("ClaimStatusTag", (claimStatus, GetColorBaseOnStatus(claimStatus)));
    }

    private static TagColour GetColorBaseOnStatus(MilestoneStatus claimStatus)
    {
        return claimStatus switch
        {
            MilestoneStatus.DueSoon => TagColour.Pink,
            MilestoneStatus.Due => TagColour.Pink,
            MilestoneStatus.Overdue => TagColour.Pink,
            MilestoneStatus.Submitted => TagColour.Purple,
            MilestoneStatus.UnderReview => TagColour.Yellow,
            MilestoneStatus.Approved => TagColour.Green,
            MilestoneStatus.Rejected => TagColour.Red,
            MilestoneStatus.Paid => TagColour.Green,
            _ => TagColour.Grey,
        };
    }
}
