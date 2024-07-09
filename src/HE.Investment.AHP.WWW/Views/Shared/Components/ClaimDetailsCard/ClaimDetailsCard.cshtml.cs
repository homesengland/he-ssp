using HE.Investments.AHP.Allocation.Contract.Claims;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.ClaimDetailsCard;

public class ClaimDetailsCard : ViewComponent
{
    public IViewComponentResult Invoke(MilestoneClaim claim, string? claimMilestoneUrl = null)
    {
        return View("ClaimDetailsCard", (claim, claimMilestoneUrl ?? string.Empty));
    }
}
