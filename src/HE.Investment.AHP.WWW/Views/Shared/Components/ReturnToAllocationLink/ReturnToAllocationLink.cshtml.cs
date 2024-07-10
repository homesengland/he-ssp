using HE.Investments.AHP.Allocation.Contract;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.ReturnToAllocationLink;

public class ReturnToAllocationLink : ViewComponent
{
    public IViewComponentResult Invoke(AllocationId allocationId, string? linkText = null)
    {
        return View("ReturnToAllocationLink", (allocationId, linkText));
    }
}
