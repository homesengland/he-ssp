using HE.Investments.AHP.Allocation.Contract;
using Microsoft.AspNetCore.Mvc;

namespace HE.Investment.AHP.WWW.Views.Shared.Components.AllocationHeader;

public class AllocationHeader : ViewComponent
{
    public IViewComponentResult Invoke(IAllocation allocation)
    {
        return View("AllocationHeader", allocation);
    }
}
