using HE.Investments.AHP.Allocation.Contract;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.ReturnToAllocationLink;

public class ReturnToAllocationLink : ViewComponent
{
    public IViewComponentResult Invoke(AllocationId allocationId, bool isEditable = true)
    {
        return View("ReturnToAllocationLink", (allocationId, isEditable));
    }
}
